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
    [SerializeField] RawImage uiRawImage2;
    [SerializeField] RawImage uiRawImage3;

    string jsonURL = "https://drive.google.com/uc?export=download&id=10g7MWCq2en2bufGgMC7mY0pvZt2diM0z";

    //Intialize Loop Indexing
    public int index;


    //Call JSON File
    void Start()
    {
        StartCoroutine(GetConfigData(jsonURL));
    }


    //Method for CALLING Images
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
            for (int i = 1; i <= numImgs; i++)
            {
                //Set Current URL and print for debug
                currentURL = baseURL + i + jpgExt;
                Debug.Log(currentURL);

                //Set Current Index
                index = i;

                // Load image
                StartCoroutine(GetImage(currentURL, index));
               
                // Clean up any resources it is using.
                request.Dispose();
            }
        }
        
    }



    //Method for SETTING Images
    IEnumerator GetImage(string url, int index)
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
            // uiRawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            //THIS IS EXTRA CODE FOR TESTING
            if (index == 1)
            {
                uiRawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Debug.Log("Display 1");
            }
            else if (index == 2)
            {
                uiRawImage2.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Debug.Log("Display 2");
            }
            else if (index == 3)
            {
                uiRawImage3.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Debug.Log("Display 3");
            }

        }
        // Clean up any resources it is using.
        request.Dispose();
    }


}
