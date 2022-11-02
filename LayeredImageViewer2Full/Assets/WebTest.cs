using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;


// Json data format
/*
      {
        "baseURL" : "..." ,
        "numImgs" : "..."
      }
*/


public struct ConfigData
{
    public string baseURL;
    public int numImgs;
}


public class WebTest : MonoBehaviour
{
    //Set 1
    [SerializeField] RawImage uiRawImage;

    string jsonURL = "https://drive.google.com/uc?export=download&id=10g7MWCq2en2bufGgMC7mY0pvZt2diM0z";


    //Call JSON File
    void Start()
    {
        StartCoroutine(GetConfigData(jsonURL));
    }


    IEnumerator GetConfigData(string url)
    {
        //Data Request
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        //Check Connection
        if (request.isNetworkError || request.isHttpError)
        {
            //error...
        }
        else
        {
            //success...

            //Pull JSON Data
            ConfigData data = JsonUtility.FromJson<ConfigData>(request.downloadHandler.text);

            //Set Base URL
            string baseURL = data.baseURL;
            int numImgs = data.numImgs;

            //jpeg file extention
            string jpgExt = ".jpg";
            string currentURL;


            //Cycle through Images
            for (int i = 1; i < numImgs; i++)
            {
                //Set Current URL and print for debug
                currentURL = baseURL + i + jpgExt;
                Debug.Log(currentURL);

                // Load image
                StartCoroutine(GetImage(currentURL));


                //SET Images
                IEnumerator GetImage(string url)
                {
                    UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

                    yield return request.SendWebRequest();

                    if (request.isNetworkError || request.isHttpError)
                    {
                        //error...
                    }
                    else
                    {
                        //success...
                        uiRawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    }

                    // Clean up any resources it is using.
                    request.Dispose();
                }

            }
        }
        // Clean up any resources it is using.
        request.Dispose();
    }


}
