using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

// Json data format
/*
      {
        "Name"     : "..." ,
        "ImageURL" : "..."
      }
*/

public struct Data2
{
    //Set 1
    public string Name;
    public string ImageURL;
}

public class WebLoader2 : MonoBehaviour
{
    //Set 1
    [SerializeField] Text uiNameText;
    [SerializeField] RawImage uiRawImage;

    string jsonURL = "https://drive.google.com/uc?export=download&id=1Kh7_iENAejroYAQFTFfcKG6IzFA6Y74I";


    void Start()
    {
        StartCoroutine(GetData(jsonURL));
    }

    IEnumerator GetData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            // error ...
        }
        else
        {
            // success...
            Data2 data = JsonUtility.FromJson<Data2>(request.downloadHandler.text);


            //Set 1
            // print data in UI
            uiNameText.text = data.Name;

            // Load image:
            StartCoroutine(GetImage(data.ImageURL));
        }

        // Clean up any resources it is using.
        request.Dispose();
    }


    //Load Image 1
    IEnumerator GetImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            // error ...
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
