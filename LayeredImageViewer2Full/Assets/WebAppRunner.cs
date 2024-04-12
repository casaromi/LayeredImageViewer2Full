using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebAppRunner : MonoBehaviour
{

    public bool test1Running = false;
    public bool test2Running = false;
    // some basic test stuff
    [System.Serializable]
    public class RequestData
    {
        public string data;
    }

    [System.Serializable]
    public class ResponseData
    {
        public string status;
        public string message;
    }

    // class to JSONify the web app payload
    [System.Serializable]
    public class ImagePayload
    {
        public string modelName;
        public ImageItem[] images;
    }

    // class to package up image and metadata
    [System.Serializable]
    public class ImageItem
    {
        public string filename;
        public string content;
    }


    [System.Serializable]
    public class ServerResponse
    {
        public string baseURL;
        public List<string> centroids; // Assuming each centroid is a list of floats
        public List<string> imageNames;
        public int numImgs;
        public string url;
    }


    // this is just for basic testing
    IEnumerator TestRequest(string url, RequestData requestData)
    {
        test1Running = true;

        // Convert your data to a JSON string
        string json = JsonUtility.ToJson(requestData);

        // Create a UnityWebRequest to send the JSON payload
        using (UnityWebRequest www = UnityWebRequest.Put(url, json))
        {
            www.method = UnityWebRequest.kHttpVerbPOST; // Use POST method
            www.SetRequestHeader("Content-Type", "application/json"); // Set content type to JSON

            // Send the request and wait for the response
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
            }
            else
            {
                // Handle the response
                TestResponse(www.downloadHandler.text);
            }
        }
        test1Running = false;
    }

    // more basic testing
    void TestResponse(string jsonResponse)
    {
        // Convert the JSON response to your C# class
        ResponseData responseData = JsonUtility.FromJson<ResponseData>(jsonResponse);

        // Use the response (just logging it here)
        Debug.Log($"Status: {responseData.status}, Message: {responseData.message}");
    }


    // Start is called before the first frame update
    public int StartTest1(double value)
    {
        if(test1Running) return 1;
        // initial test, just package up some simple JSON and send it
        // to an app that parses it, multiplies it by 2, jsonifies the result
        // and sends it back
        string valueString = string.Format("{0:f}",value);
        RequestData requestData = new RequestData { data = valueString };
        string url = "https://davidjoiner.net/myapp/parse_double";
        // NOTE THAT WEB APP CALLS ARE ASYNCHRONOUS!!!!
        //  they need a coroutine, same as JavaScript using AJAX or Fetch
        StartCoroutine(TestRequest(url, requestData));
        return 0;
    }

    public int StartTest2(Sprite [] sprites) { 
        if(test2Running) return 1;
        // Here is the real test
        ImagePayload payload = new ImagePayload();
        payload.modelName = "Foo";
        payload.images = new ImageItem[sprites.Length];
        // pack up the sprites as "ImageItem"s (see conversion routine)
        for (int i = 0; i < sprites.Length; i++)
        {
            payload.images[i] = SpriteToImageItem(sprites[i]);

        }

        string image_url = "https://davidjoiner.net/confocal_flask/upload_app/";
        StartCoroutine(SendImages(image_url, payload));
        return 0;
    }

    // sprites need to be uuencoded, and the image object also needs a filename
    // because currently the web app wants a file name for temporary storage purposes
    //   this is needed to match the current expectations of the app but might be
    //   simplified in the future
    public static ImageItem SpriteToImageItem(Sprite sprite)
    {
        // turn the sprite object into a Texture -- make sure that
        //   format is 3 channels, not 4, as the AI model expects this
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGB24, false);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                  (int)sprite.textureRect.y,
                                                  (int)sprite.textureRect.width,
                                                  (int)sprite.textureRect.height);
        texture.SetPixels(pixels);
        texture.Apply();

        // we need to encode the image in a standard format
        byte[] imageBytes = texture.EncodeToJPG();
        // and then uuencode that into a base 64 string for sending
        string base64Content = System.Convert.ToBase64String(imageBytes);

        // package it all up into the JSON format expected by the web app
        return new ImageItem
        {
            filename = sprite.name + ".jpg",
            content = base64Content
        };
    }

    Vector4 getCentroidValues(string centroid_string)
	{
        string [] values = centroid_string.Split(',');
        if (values.Length == 4)
        {
            float x, y, z, w;

            if (float.TryParse(values[0], out x) &&
                float.TryParse(values[1], out y) &&
                float.TryParse(values[2], out z) &&
                float.TryParse(values[3], out w))
            {
                Vector4 vector = new Vector4(x, y, z, w);
                return vector;
            }
            else
            {
                Debug.LogError("Failed to parse server response values into floats.");
            }
        }
        else
        {
            Debug.LogError("Server response does not contain 4 values.");
        }
        return Vector4.zero;
    }


    // coroutine for sending the images
    IEnumerator SendImages(string url, ImagePayload payload)
    {
        test2Running = true;
        // convert the serializable object to a JSON string
        string json = JsonUtility.ToJson(payload);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        // use UnityWebRequest to set up an asynchronous web call
        using (UnityWebRequest www = UnityWebRequest.Put(url, jsonToSend))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");

            // while this is still in process use yield to return control to program
            yield return www.SendWebRequest();

            // handle result when complete
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
            }
            else
            {
                // just print it out for now, but we will need to dig
                //  the data out at some point and use it more meaningfully
                Debug.Log($"Server response: {www.downloadHandler.text}");
                string jsonString = www.downloadHandler.text;
                ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(jsonString);
                Debug.Log(getCentroidValues(serverResponse.centroids[0]));


            }
        }
        test2Running = false;
    }


}
