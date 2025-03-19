using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public abstract class WebAppCaller<TRequest, TResponse> : MonoBehaviour
    where TRequest : class
    where TResponse : class
{
    public bool CallRunning { get; private set; }
    public TResponse responseData;

    // Define a method to start the request process, to be called from inheriting classes or externally
    public void StartRequest(string url, TRequest requestData)
    {
        StartCoroutine(SendRequest(url, requestData));
    }

    public abstract void ParseResponse();

    // Implement the web request sending logic here, in the abstract class
    protected IEnumerator SendRequest(string url, TRequest requestData)
    {
        CallRunning = true;

        string json = JsonUtility.ToJson(requestData);

        using (UnityWebRequest www = UnityWebRequest.Put(url, json))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {www.error}");
            }
            else
            {
                // Use the abstract method to parse the response
                responseData = JsonUtility.FromJson<TResponse>(www.downloadHandler.text);
                ParseResponse();
            }
        }

        CallRunning = false;
    }

}
