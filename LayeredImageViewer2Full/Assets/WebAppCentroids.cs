using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebAppCentroids : WebAppCaller<WebAppCentroids.ImagePayload, WebAppCentroids.ServerResponse>
{
    public Vector4[] centroids = null;

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

    public int StartCentroidsCall(Sprite[] sprites)
    {
        if (CallRunning) return 1;
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
        StartRequest(image_url, payload);
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
        string[] values = centroid_string.Split(',');
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

    public override void ParseResponse() { 
        centroids = new Vector4[responseData.centroids.Count];
        for (int i = 0; i< responseData.centroids.Count; i++)
        {
            centroids[i] = getCentroidValues(responseData.centroids[i]);
        }
    }

}
