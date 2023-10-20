using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;

// Json data format
/*
      {
        "baseURL" : "..." ,
        "numImgs" : "..."
      }
*/


//Intialize JSON Data Feilds
public struct ConfigImgData
{
	public string baseURL;
	public int numImgs;
}


//Main Class
public class ImgLoaderTest : MonoBehaviourPunCallbacks
{
	//Image Paths

	//public GameObject buildFile;

	//Intialize Loop Indexing
	public int index;

	//Get JSON File
	//string jsonURL = "https://davidjoiner.net/~confocal/UserBuildScripts/1_RedCellsTest";

	//string jsonURL = "https://drive.google.com/uc?export=download&id=10g7MWCq2en2bufGgMC7mY0pvZt2diM0z";
	


	//Image Start and Stop
	public int imageStart = 1;
	public int imageStop;


	//Shaderes and Such
	public GameObject imagePlanePRE;
	public float cutoff = 0.3f;
	public float alphaMultiplier = 1.0f;
	public float sliceHeight = 0.02f;
	public float redMultiplier = 0.0f;
	public float greenMultiplier = 0.0f;
	public float blueMultiplier = 1.0f;
	float[] spacing;
	GameObject[] thePlanes;
	public bool edgeDetection = false;
	public Material defaultMat;


	//Height
	float[] linspace(float min, float max, int n)
	{
		float[] return_value = new float[n];
		return_value[0] = min;
		return_value[n - 1] = max;
		float h = (max - min) / (n - 1);
		for (int i = 1; i < n - 1; i++)
		{
			return_value[i] = min + i * h;
		}
		return return_value;
	}



	//Change the Alpha/ Transparency
	public void changeAlpha(float alphaMultiplier)
	{
		this.alphaMultiplier = alphaMultiplier;

		foreach (GameObject plane in thePlanes)
		{
			plane.GetComponent<Renderer>().material.SetFloat("_AlphaMult", alphaMultiplier);
		}
	}



	//Change the Cutoff
	public void changeCutoff(float cutoff)
	{
		this.cutoff = cutoff;

		foreach (GameObject plane in thePlanes)
		{
			plane.GetComponent<Renderer>().material.SetFloat("_Cutoff", cutoff);
		}
	}



	//Chage the spacing between images 
	public void scaleHeight(float scale)
	{
		transform.localScale = new Vector3(1, scale / (spacing[1] - spacing[0]), 1);
	}



	//Chage the Height of the model 
	/*
	//Move the model up or down
	public void changePosition(float modelPosition)
	{
		transform.localScale = new Vector3(1, modelPosition, 1);
	}
	*/









	//Main Method to Create Model 
	void Start()
	{
		//Check if we are in a room (IF JOINING)
		if (PhotonNetwork.InRoom)
		{
			Debug.Log("YO PHASE 1 >>>> complete.");
			GetRoomProperties();
		}
		//Else (IF CREATING)
        else
        {
			StartCoroutine(GetConfigImgData(PCAuth.selectedJsonLink));
			Debug.Log(PCAuth.selectedJsonLink);
		}
	}

	public override void OnJoinedRoom()
	{
		// Called when the local player successfully joins a room
		Debug.Log("!!!YEET");
		GetRoomProperties();
	}


	void GetRoomProperties()
	{
		//Check if the custom properties exist in the room
		if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("url"))
		{
			//Access and display the values
			string url = (string)PhotonNetwork.CurrentRoom.CustomProperties["url"];
			Debug.Log("YO HERE YO LINK...." + url); 

			GetConfigImgData(url);
		}
		else
		{
			Debug.Log("Room properties are missing or incomplete.");
		}
	}



	//Method for CALLING Images
	IEnumerator GetConfigImgData(string url)
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
			ConfigImgData data = JsonUtility.FromJson<ConfigImgData>(request.downloadHandler.text);

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
				StartCoroutine(GetImage(currentURL, baseURL, index, numImgs));

				// Clean up any resources it is using.
				request.Dispose();
			}
		}

	}



	//Method for SETTING Images
	IEnumerator GetImage(string url, string baseURL, int index, int numImgs)
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
			Debug.Log("Success!");

			//Define Stop Image
			imageStop = numImgs;


			//Initialize plane and spacing 
			thePlanes = new GameObject[imageStop - imageStart + 1];
			spacing = linspace(0, 1, imageStop - imageStart + 1);



			//Cycle though images
			GameObject plane = Instantiate(imagePlanePRE);

			//Load in Images
			//Texture2D myTexture1 = Resources.Load<Texture2D>(baseURL + string.Format("{0:D5}", index)) as Texture2D;
			Texture2D myTexture1 = ((DownloadHandlerTexture)request.downloadHandler).texture;

			Color[] pixelArray1 = myTexture1.GetPixels();
			plane.GetComponent<RenderImagePlane>().setRawPixels(pixelArray1);

			plane.transform.parent = transform;

			Material mat = new Material(defaultMat.shader);
			mat.mainTexture = myTexture1;

			plane.transform.localPosition = new Vector3(0.0f, spacing[index - imageStart], 0.0f);
			plane.GetComponent<MeshRenderer>().material = mat;
			plane.GetComponent<RenderImagePlane>().setTexture(myTexture1);

			thePlanes[index - imageStart] = plane;
			thePlanes[index - imageStart].GetComponent<Renderer>().material.SetFloat("_AlphaMult", alphaMultiplier);
			thePlanes[index - imageStart].GetComponent<Renderer>().material.SetFloat("_Cutoff", cutoff);

			//Change Height
			scaleHeight(sliceHeight);
		}
		// Clean up any resources it is using.
		request.Dispose();
	}

}
