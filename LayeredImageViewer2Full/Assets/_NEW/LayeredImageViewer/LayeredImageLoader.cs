/*
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LayeredImageLoader : MonoBehaviour
{

	public int iSkip = 2;
	public Sprite [] sprites;

	Texture2D[] images;
	Texture2D [] sideImages;
	Texture2D [] frontImages;
	Color[,,] allPixels;
	public GameObject layeredImagePRE;
	GameObject[] layeredImagesObjects;
	GameObject[] frontImagesObjects;
	GameObject[] sideImagesObjects;
	int nImages;
	public Slider alphaSlider;
	public Slider cutoffSlider;
	int width = 0;
	int height = 0;

	GameObject topView;
	GameObject frontView;
	GameObject sideView;

	public float alphaInit = 0.3f;
	public float cutoffInit = 1.0f;
	public float normalInit = 1.0f;
	public float saturationInit = 1.0f;

	public bool extraPlanes = false;

	bool showSide;
	bool showFront;

	public Vector3 ClipOffset = Vector3.zero;
	public Vector3 ClipNormal = Vector3.one;

	void Start()
	{

		showSide = extraPlanes;
		showFront = extraPlanes;
		topView = new GameObject();
		topView.transform.parent = transform;
		topView.transform.localPosition = Vector3.zero;
		topView.transform.localScale = Vector3.one;
		topView.transform.localRotation = Quaternion.Euler(0, 0, 0);

		bool firstPass = true;
		nImages=sprites.Length/iSkip;
		if(sprites.Length%iSkip!=0) nImages += 1;
		
		images = new Texture2D[nImages];
		layeredImagesObjects = new GameObject[nImages];
		
		for (int k = 0; k < nImages; k++)
		{
			int imageNumber = iSkip*k;
			
			Texture2D spriteTex = sprites[imageNumber].texture;
			images[k] = new Texture2D(spriteTex.width,spriteTex.height);
			

			if (firstPass)
			{
				firstPass = false;
				width = images[k].width;
				height = images[k].height;
				if(extraPlanes) allPixels = new Color[width,height,nImages];
			}
			Color [] pixels = spriteTex.GetPixels();
			for(int i=0;i<width && extraPlanes;i++)
			{
				for(int j=0;j<height;j++)
				{
					allPixels[i,j,k] = pixels[j*width+i];

				}
			}
			images[k].SetPixels(pixels);
			images[k].Apply();
			layeredImagesObjects[k] = Instantiate(layeredImagePRE);
			layeredImagesObjects[k].transform.parent = topView.transform;
			layeredImagesObjects[k].transform.localPosition = new Vector3(0, (float)k / (float)(nImages - 1) - 0.5f, 0);
			layeredImagesObjects[k].transform.localScale = Vector3.one;
			layeredImagesObjects[k].transform.localRotation = Quaternion.Euler(90, 0, 0);

			layeredImagesObjects[k].GetComponent<Renderer>().material.mainTexture = images[k];
		}

		if(showFront) { 
			// front view
			frontImages = new Texture2D[height];
			frontImagesObjects = new GameObject[height];
			frontView = new GameObject();
			frontView.transform.parent = transform;
			frontView.transform.localPosition = Vector3.zero;
			frontView.transform.localScale = Vector3.one;
			frontView.transform.localRotation = Quaternion.Euler(0, 0, 0);

			for (int j=0;j<height;j++)
			{
				frontImages[j] = new Texture2D(width,nImages);
				Color []pixels = new Color[width*nImages];
				for(int k=0;k<nImages;k++)
				{
					for(int i=0;i<width;i++)
					{
						pixels[k*width+i] = allPixels[i, j, k];
					}
				}
				frontImages[j].SetPixels(pixels);
				frontImages[j].Apply();
				frontImagesObjects[j] = Instantiate(layeredImagePRE);
				frontImagesObjects[j].transform.parent = frontView.transform;
				frontImagesObjects[j].transform.localPosition = new Vector3(0, 0, (float)j / (float)(height - 1)-0.5f);
				frontImagesObjects[j].transform.localRotation = Quaternion.Euler(0, 0, 0);
				frontImagesObjects[j].transform.localScale = Vector3.one;
				frontImagesObjects[j].GetComponent<Renderer>().material.mainTexture = frontImages[j];
			}
		}

		if(showSide) { 
			// side view
			sideImages = new Texture2D[width];
			sideImagesObjects = new GameObject[width];
			sideView = new GameObject();
			sideView.transform.parent = transform;
			sideView.transform.localPosition = Vector3.zero;
			sideView.transform.localScale = Vector3.one;
			sideView.transform.localRotation = Quaternion.Euler(0, 0, 0);

			for (int i = 0; i < width; i++)
			{
				sideImages[i] = new Texture2D(height, nImages);
				Color[] pixels = new Color[height * nImages];
				for (int k = 0; k < nImages; k++)
				{
					for (int j = 0; j < height; j++)
					{
						pixels[k * height + j] = allPixels[i, j, k];
					}
				}
				sideImages[i].SetPixels(pixels);
				sideImages[i].Apply();
				sideImagesObjects[i] = Instantiate(layeredImagePRE);
				sideImagesObjects[i].transform.parent = sideView.transform;
				sideImagesObjects[i].transform.localPosition = new Vector3((float)i / (float)(width - 1) - 0.5f,0,0);
				sideImagesObjects[i].transform.localScale = Vector3.one;
				sideImagesObjects[i].transform.localRotation = Quaternion.Euler(180,90,180);
				//sideImagesObjects[i].transform.Rotate(sideImagesObjects[i].transform.right, 90);
				//sideImagesObjects[i].transform.Rotate(sideImagesObjects[i].transform.up, -90);
				sideImagesObjects[i].GetComponent<Renderer>().material.mainTexture = sideImages[i];
			}
		}

		alphaSlider.value = alphaInit;
		cutoffSlider.value = cutoffInit;
		setAlpha(alphaInit);
		setCutoff(cutoffInit);
		setNormal(normalInit);
		setSaturation(saturationInit);

		setAllRenderers("_ClipBase", transform.position+ClipOffset);
		setAllRenderers("_ClipNormal", ClipNormal);

	}


	public void setAllRenderers(string property, Vector3 value)
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer rend in renderers)
		{
			rend.material.SetVector(property, value);
		}
	}

	public void setAllRenderers(string property, float value)
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer rend in renderers)
		{
			rend.material.SetFloat(property, value);
		}
	}

	public void setSaturation(float saturation)
	{
		setAllRenderers("_Saturation", saturation);

	}

	public void setNormal(float normal)
	{
		setAllRenderers("_NormalCutoff", normal);

	}

	public void setAlpha(float alpha)
	{

		setAllRenderers("_Alpha", alpha);
	}

	public void setCutoff(float cutoff)
	{
		setAllRenderers("_Cutoff", cutoff);


		
	}

	// Update is called once per frame
	void Update()
	{

	}
}
*/


//WEB LOADED IMAGES 



using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;



//Intialize JSON Data Feilds
public struct ConfigImgDataNew
{
	public string baseURL;
	public int numImgs;
}



public class LayeredImageLoader : MonoBehaviourPunCallbacks
{

	//Intialize Loop Indexing
	public int index;


	//Intialize Vars
	public int iSkip = 1;
	public Sprite [] sprites;

	Texture2D[] images;
	Texture2D [] sideImages;
	Texture2D [] frontImages;
	Color[,,] allPixels;
	public GameObject layeredImagePRE;
	GameObject[] layeredImagesObjects;
	GameObject[] frontImagesObjects;
	GameObject[] sideImagesObjects;
	int nImages;
	public Slider alphaSlider;
	public Slider cutoffSlider;
	int width = 0;
	int height = 0;

	GameObject topView;
	GameObject frontView;
	GameObject sideView;

	public float alphaInit = 0.3f;
	public float cutoffInit = 1.0f;
	public float normalInit = 1.0f;
	public float saturationInit = 1.0f;

	public bool extraPlanes = false;

	private bool spritesReady = false;

	bool showSide;
	bool showFront;


	public Vector3 ClipOffset = Vector3.zero;
	public Vector3 ClipNormal = Vector3.one;





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
			ConfigImgDataNew data = JsonUtility.FromJson<ConfigImgDataNew>(request.downloadHandler.text);

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
		Debug.Log("Starting to Create Model - stand by... ");


		//Intialize Vars
		showSide = extraPlanes;
		showFront = extraPlanes;
		topView = new GameObject();
		topView.transform.parent = transform;
		topView.transform.localPosition = Vector3.zero;
		topView.transform.localScale = Vector3.one;
		topView.transform.localRotation = Quaternion.Euler(0, 0, 0);



		//Set up model
		bool firstPass = true;
		nImages = sprites.Length / iSkip;
		if (sprites.Length % iSkip != 0) nImages += 1;

		images = new Texture2D[nImages];
		layeredImagesObjects = new GameObject[nImages];

		for (int k = 0; k < nImages; k++)
		{
			int imageNumber = iSkip * k;


			if (sprites[imageNumber] != null)
			{

				if (imageNumber < sprites.Length)
				{

					Texture2D spriteTex = sprites[imageNumber].texture;
					images[k] = new Texture2D(spriteTex.width, spriteTex.height);


					if (firstPass)
					{
						firstPass = false;
						width = images[k].width;
						height = images[k].height;
						if (extraPlanes) allPixels = new Color[width, height, nImages];
					}
					Color[] pixels = spriteTex.GetPixels();
					for (int i = 0; i < width && extraPlanes; i++)
					{
						for (int j = 0; j < height; j++)
						{
							allPixels[i, j, k] = pixels[j * width + i];

						}
					}
					images[k].SetPixels(pixels);
					images[k].Apply();
					layeredImagesObjects[k] = Instantiate(layeredImagePRE);
					layeredImagesObjects[k].transform.parent = topView.transform;
					layeredImagesObjects[k].transform.localPosition = new Vector3(0, (float)k / (float)(nImages - 1) - 0.5f, 0);
					layeredImagesObjects[k].transform.localScale = Vector3.one;
					layeredImagesObjects[k].transform.localRotation = Quaternion.Euler(90, 0, 0);

					layeredImagesObjects[k].GetComponent<Renderer>().material.mainTexture = images[k];
				}
				//else: imageNumber < sprites.Length
				else
				{
					// Handle the case where imageNumber is out of bounds
					Debug.LogError("Image number out of bounds: " + imageNumber);
				}
			}

			//sprites[imageNumber] != null
			else
			{
				Debug.LogError("Sprite is null at index: " + imageNumber);
			}
		}

		//Function to show Front of cells
		if (showFront)
		{
			// front view
			frontImages = new Texture2D[height];
			frontImagesObjects = new GameObject[height];
			frontView = new GameObject();
			frontView.transform.parent = transform;
			frontView.transform.localPosition = Vector3.zero;
			frontView.transform.localScale = Vector3.one;
			frontView.transform.localRotation = Quaternion.Euler(0, 0, 0);

			for (int j = 0; j < height; j++)
			{
				frontImages[j] = new Texture2D(width, nImages);
				Color[] pixels = new Color[width * nImages];
				for (int k = 0; k < nImages; k++)
				{
					for (int i = 0; i < width; i++)
					{
						pixels[k * width + i] = allPixels[i, j, k];
					}
				}
				frontImages[j].SetPixels(pixels);
				frontImages[j].Apply();
				frontImagesObjects[j] = Instantiate(layeredImagePRE);
				frontImagesObjects[j].transform.parent = frontView.transform;
				frontImagesObjects[j].transform.localPosition = new Vector3(0, 0, (float)j / (float)(height - 1) - 0.5f);
				frontImagesObjects[j].transform.localRotation = Quaternion.Euler(0, 0, 0);
				frontImagesObjects[j].transform.localScale = Vector3.one;
				frontImagesObjects[j].GetComponent<Renderer>().material.mainTexture = frontImages[j];
			}
		}



		//Function to show Sides of cells
		if (showSide)
		{
			// side view
			sideImages = new Texture2D[width];
			sideImagesObjects = new GameObject[width];
			sideView = new GameObject();
			sideView.transform.parent = transform;
			sideView.transform.localPosition = Vector3.zero;
			sideView.transform.localScale = Vector3.one;
			sideView.transform.localRotation = Quaternion.Euler(0, 0, 0);

			for (int i = 0; i < width; i++)
			{
				sideImages[i] = new Texture2D(height, nImages);
				Color[] pixels = new Color[height * nImages];
				for (int k = 0; k < nImages; k++)
				{
					for (int j = 0; j < height; j++)
					{
						pixels[k * height + j] = allPixels[i, j, k];
					}
				}
				sideImages[i].SetPixels(pixels);
				sideImages[i].Apply();
				sideImagesObjects[i] = Instantiate(layeredImagePRE);
				sideImagesObjects[i].transform.parent = sideView.transform;
				sideImagesObjects[i].transform.localPosition = new Vector3((float)i / (float)(width - 1) - 0.5f, 0, 0);
				sideImagesObjects[i].transform.localScale = Vector3.one;
				sideImagesObjects[i].transform.localRotation = Quaternion.Euler(180, 90, 180);
				//sideImagesObjects[i].transform.Rotate(sideImagesObjects[i].transform.right, 90);
				//sideImagesObjects[i].transform.Rotate(sideImagesObjects[i].transform.up, -90);
				sideImagesObjects[i].GetComponent<Renderer>().material.mainTexture = sideImages[i];
			}
		}



		//Set changable values 
		alphaSlider.value = alphaInit;
		cutoffSlider.value = cutoffInit;
		setAlpha(alphaInit);
		setCutoff(cutoffInit);
		setNormal(normalInit);
		setSaturation(saturationInit);

		setAllRenderers("_ClipBase", transform.position + ClipOffset);
		setAllRenderers("_ClipNormal", ClipNormal);
	}






	public void setAllRenderers(string property, Vector3 value)
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer rend in renderers)
		{
			rend.material.SetVector(property, value);
		}
	}


	public void setAllRenderers(string property, float value)
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer rend in renderers)
		{
			rend.material.SetFloat(property, value);
		}
	}



	public void setSaturation(float saturation)
	{
		setAllRenderers("_Saturation", saturation);

	}


	public void setNormal(float normal)
	{
		setAllRenderers("_NormalCutoff", normal);

	}


	public void setAlpha(float alpha)
	{

		setAllRenderers("_Alpha", alpha);
	}


	public void setCutoff(float cutoff)
	{
		setAllRenderers("_Cutoff", cutoff);


		
	}



	// Update is called once per frame
	void Update()
	{

	}
}
