using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExampleCallTest : WebAppCaller<ExampleCallTest.RequestData, ExampleCallTest.ResponseData>
{
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


    bool callCompleted = false;
    // Unity's Start method - Coroutine
    void Start()
    {// Example URL and Request Data for demonstration
        float value = 5.0f;
        string valueString = string.Format("{0:f}", value);
        RequestData testData = new RequestData { data = valueString };
        string url = "https://davidjoiner.net/myapp/parse_double";
        // Start the web request
        StartRequest(url, testData);
    }

	private void Update()
	{
		if(!callCompleted)
		{
            if(!CallRunning)
			{
                callCompleted = true;
                Debug.Log(responseData);
			}
		}
	}

	public override void ParseResponse()
	{
	}



}
