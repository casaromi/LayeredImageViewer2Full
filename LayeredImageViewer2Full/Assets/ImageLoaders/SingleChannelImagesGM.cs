using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleChannelImagesGM : MonoBehaviour
{
	public int imageStart = 1;
	public int imageStop = 50;
	public GameObject imagePlanePRE;
	public string path1 = "Images/Germ_cells_magenta/";
	//public string path1 = "https://github.com/casaromi/ImageTest/blob/e9b9e87e5e91f21c986d53115146eb85459e0ab6/Pole_cells_red/";

	public float cutoff = 0.3f;
	public float alphaMultiplier = 1.0f;
	public float sliceHeight = 0.02f;
	public float redMultiplier = 0.0f;
	public float greenMultiplier = 0.0f;
	public float blueMultiplier = 1.0f;
	// Start is called before the first frame update

	/*
	//ISSUE IS HERE, CHECK WWWLOADER AGENST PATH1
	private IEnumerator LoadFromLikeCoroutine()
	{
		Debug.Log("Loading ....");
		WWW wwwLoader = new WWW(path1);   // create WWW object pointing to the url
		yield return wwwLoader;         // start loading whatever in that url ( delay happens here )
		Debug.Log("Loaded");
	}
	*/


	void Start()
	{
		//StartCoroutine(LoadFromLikeCoroutine()); // execute the section independently

		for (int i = imageStart; i <= imageStop; i++)
		{
			Texture2D myTexture1 = Resources.Load<Texture2D>(path1 + string.Format("{0:D5}", i)) as Texture2D;
			Color[] pixelArray1 = myTexture1.GetPixels();
			for (int j = 0; j < myTexture1.height; j++)  // each row
			{
				for (int k = 0; k < myTexture1.width; k++) // each column
				{
					// force single channel acrcoss RGB

					//pixelArray1[myTexture1.width * j + k].g = pixelArray1[myTexture1.width * j + k].r;
					//pixelArray1[myTexture1.width * j + k].b = pixelArray1[myTexture1.width * j + k].r;
					float pixelValue1 = Mathf.Min(pixelArray1[myTexture1.width * j + k].r +
						pixelArray1[myTexture1.width * j + k].g + pixelArray1[myTexture1.width * j + k].b, 1);
					pixelArray1[myTexture1.width * j + k].a = alphaMultiplier * pixelValue1;

					if (pixelValue1 < cutoff)
					{
						pixelArray1[myTexture1.width * j + k].a = 0;
						pixelArray1[myTexture1.width * j + k].r = 0;
						pixelArray1[myTexture1.width * j + k].g = 0;
						pixelArray1[myTexture1.width * j + k].b = 0;
					}

				}
			}
			// EDGE DETECTION
			if (false)
			{
				for (int j = 1; j < myTexture1.height - 1; j++)
				{
					for (int k = 1; k < myTexture1.width - 1; k++)
					{
						float pixelValue = Mathf.Min(pixelArray1[myTexture1.width * j + k].r +
												pixelArray1[myTexture1.width * j + k].g + pixelArray1[myTexture1.width * j + k].b, 1);
						float pixelValueL = Mathf.Min(pixelArray1[myTexture1.width * (j - 1) + k].r +
												pixelArray1[myTexture1.width * (j - 1) + k].g +
												pixelArray1[myTexture1.width * (j - 1) + k].b, 1);
						float pixelValueR = Mathf.Min(pixelArray1[myTexture1.width * (j + 1) + k].r +
							pixelArray1[myTexture1.width * (j + 1) + k].g +
							pixelArray1[myTexture1.width * (j + 1) + k].b, 1);
						float pixelValueU = Mathf.Min(pixelArray1[myTexture1.width * (j) + k + 1].r +
							pixelArray1[myTexture1.width * (j) + k + 1].g +
							pixelArray1[myTexture1.width * (j) + k + 1].b, 1);
						float pixelValueD = Mathf.Min(pixelArray1[myTexture1.width * (j) + k - 1].r +
							pixelArray1[myTexture1.width * (j) + k - 1].g +
							pixelArray1[myTexture1.width * (j) + k - 1].b, 1);
						if (pixelValue == 0)
						{
							if (pixelValueU + pixelValueD + pixelValueR + pixelValueL > 0)
							{
								pixelArray1[myTexture1.width * j + k].a = 1;
								pixelArray1[myTexture1.width * j + k].r = 0;
								pixelArray1[myTexture1.width * j + k].g = 0;
								pixelArray1[myTexture1.width * j + k].b = 0;
							}
						}

					}
				}
			}
			myTexture1.SetPixels(pixelArray1);
			myTexture1.Apply();
			GameObject plane = Instantiate(imagePlanePRE);
			plane.transform.parent = transform;
			plane.transform.localPosition = new Vector3(0.0f, sliceHeight * i, 0.0f);
			Material mat = new Material(Shader.Find("ModShader2"));
			mat.mainTexture = myTexture1;
			plane.GetComponent<MeshRenderer>().material = mat;
			plane.GetComponent<RenderImagePlane>().setTexture(myTexture1);
		}


	}

	// Update is called once per frame
	void Update()
	{

	}
}
