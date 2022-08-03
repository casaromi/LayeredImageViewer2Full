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
public struct Data
{
    //Set 1
    public string Name;
    public string ImageURL;

    //Set 2
    public string Name2;
    public string ImageURL2;

    //Set 3
    public string Name3;
    public string ImageURL3;
}

public class WebLoader : MonoBehaviour
{
    //Set 1
    [SerializeField] Text uiNameText;
    [SerializeField] RawImage uiRawImage;

    //Set 2
    [SerializeField] Text uiNameText2;
    [SerializeField] RawImage uiRawImage2;

    //Set 3
    [SerializeField] Text uiNameText3;
    [SerializeField] RawImage uiRawImage3;

    string jsonURL = "https://drive.google.com/uc?export=download&id=18mvcVm6iLaiKgpYi6A3mdp3ww8cSzgD0";
    

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
            Data data = JsonUtility.FromJson<Data>(request.downloadHandler.text);


            //Set 1
            // print data in UI
            uiNameText.text = data.Name;

            // Load image:
            StartCoroutine(GetImage(data.ImageURL));


            //Set 2
            // print data in UI
            uiNameText2.text = data.Name2;

            // Load image:
            StartCoroutine(GetImage(data.ImageURL2));
            
            
            //Set 3
            // print data in UI
            uiNameText3.text = data.Name3;

            // Load image:
            StartCoroutine(GetImage(data.ImageURL3));
        }

        // Clean up any resources it is using.
        request.Dispose();
    }

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

            uiRawImage2.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            uiRawImage3.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }

        // Clean up any resources it is using.
        request.Dispose();
    }
}

