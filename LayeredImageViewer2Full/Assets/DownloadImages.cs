using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;



//Intialize JSON Data Feilds
public struct ConfigImgDataNewModel
{
	public string baseURL;
	public int numImgs;
}



public class DownloadImages : MonoBehaviourPunCallbacks
{

	//Intialize Loop Indexing
	public int index;


	//Intialize Vars
	public int iSkip = 1;
	public Sprite[] sprites;


	private bool spritesReady = false;

	//public GameObject webAppRunner;




	//Intialize Room Settings  
	public override void OnJoinedRoom()
	{
		// Called when the local player successfully joins a room
		Debug.Log("Room OPEN!!!");
		StartCoroutine(WaitForScript1());
	}



	IEnumerator WaitForScript1()
	{
		// Wait until DisplayRoomInfo script has finished running
		while (!DisplayRoomInfo.IsScript1Finished)
		{
			yield return null; // Wait for one frame
		}
		Debug.Log("DRI SCRIP COMPLETER imgs!!!");

		// Continue with the rest of the script
		StartCoroutine(GetRoomProperties());
	}



	IEnumerator GetRoomProperties()
	{
		//Check if the custom properties exist in the room

		//Access and display the values
		string modelJson = DisplayRoomInfo.modelJson;

		// Wait until modelJson is not empty
		while (string.IsNullOrEmpty(modelJson))
		{
			yield return null; // Wait for one frame
			modelJson = DisplayRoomInfo.modelJson; // Update modelJson
		}


		Debug.Log("HERE YO LINK: " + modelJson);

		yield return StartCoroutine(GetConfigImgData(modelJson));

	}





	//Get Images from Web
	IEnumerator GetConfigImgData(string modelJson)
	{
		//Data Request
		Debug.Log("Starting Download: " + modelJson);

		UnityWebRequest request = UnityWebRequest.Get(modelJson);
		yield return request.SendWebRequest();

		//Check Connection
		if (request.isNetworkError || request.isHttpError)
		{
			//error...
			Debug.Log("WEB ERROR: " + modelJson);
		}
		else
		{
			//success...

			//Pull JSON Data
			ConfigImgDataNewModel data = JsonUtility.FromJson<ConfigImgDataNewModel>(request.downloadHandler.text);

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
				//Debug.Log(currentURL);

				//Set Current Index
				index = i;

				// Load image
				yield return StartCoroutine(GetImage(currentURL, baseURL, index, numImgs));

				// Clean up any resources it is using.
				request.Dispose();
			}
		}

	}


	//Populate Sprite array with images
	IEnumerator GetImage(string url, string baseURL, int index, int numImgs)
	{
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

		yield return request.SendWebRequest();

		if (request.isNetworkError || request.isHttpError)
		{
			//error...
			Debug.Log("ERROR");
		}
		else
		{
			//success...
			Debug.Log("Success!");

			// Convert the downloaded texture to a Sprite
			Texture2D texture = DownloadHandlerTexture.GetContent(request);
			Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

			// Check if the sprites array is initialized and has enough space for the new sprite
			if (sprites == null || sprites.Length < numImgs)
			{
				sprites = new Sprite[numImgs];
			}

			// Store the sprite in the array at the correct index
			sprites[index - 1] = sprite;


			// Print the length of the sprites array to the console
			Debug.Log("!!ARRAY OF IMAGES - Sprites array length: " + sprites.Length);

			// Set the spritesReady flag to true
			if (index == numImgs)
			{
				Debug.Log("!!ARRAY Fully Populated ");
				spritesReady = true;
			}
			else
			{
				yield return null;
			}


			// Clean up any resources it is using.
			request.Dispose();
		}
	}





	//Create and Configure Model
	void Start()
	{
		// Wait until the sprites array is ready
		Debug.Log("Waiting for Sprites to populate... ");
		StartCoroutine(WaitForSprites());
	}





	IEnumerator WaitForSprites()
	{
		while (sprites == null || !spritesReady)
		{
			yield return null;
		}


		// Continue with the rest of the Start function after the sprites array is populated
		Debug.Log("SPRITE ARRAY POPULATED!!! ");
		Debug.Log("Starting to Create Model - stand by... ");
	}



	// Update is called once per frame
	void Update()
	{

	}
}