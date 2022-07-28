using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPUChannelLoader : MonoBehaviour
{
	public int imageStart = 1;
	public int imageStop = 47;
	public GameObject imagePlanePRE;
	public string path1 = "Images/Pole_cells_red/";
	public float cutoff = 0.3f;
	public float alphaMultiplier = 1.0f;
	public float sliceHeight = 0.02f;
	public float redMultiplier = 0.0f;
	public float greenMultiplier = 0.0f;
	public float blueMultiplier = 1.0f;
	float[] spacing;
	GameObject[] thePlanes;
	// Start is called before the first frame update
	public bool edgeDetection = false;
	public Material defaultMat;


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


	public void changeCutoff(float cutoff)
	{
		this.cutoff = cutoff;

		//foreach (GameObject plane in thePlanes)
		//{
		//	plane.GetComponent<Renderer>().material.SetFloat("_AlphaMult", alphaMultiplier);
		//}
		reload();
	}
	public void changeAlpha(float alphaMultiplier)
	{
		this.alphaMultiplier = alphaMultiplier;

		//foreach (GameObject plane in thePlanes)
		//{
		//	plane.GetComponent<Renderer>().material.SetFloat("_AlphaMult", alphaMultiplier);
		//}
		reload();
	}

	public void scaleHeight(float scale)
	{
		transform.localScale = new Vector3(1, scale / (spacing[1] - spacing[0]), 1);
	}

	public void edgeChange()
	{
		edgeDetection = !edgeDetection;
		reload();
	}

	public void reload()
	{
		for (int i = imageStart; i <= imageStop; i++)
		{
			thePlanes[i - imageStart].GetComponent<RenderImagePlane>().resetPixelsToRaw();
			Color[] pixelArray1 = thePlanes[i - imageStart].GetComponent<RenderImagePlane>().getPixels();
			int height = thePlanes[i - imageStart].GetComponent<RenderImagePlane>().getHeight();
			int width = thePlanes[i - imageStart].GetComponent<RenderImagePlane>().getWidth();
			for (int j = 0; j < height; j++)  // each row
			{
				for (int k = 0; k < width; k++) // each column
				{
					// force single channel acrcoss RGB

					//pixelArray1[myTexture1.width * j + k].g = pixelArray1[myTexture1.width * j + k].r;
					//pixelArray1[myTexture1.width * j + k].b = pixelArray1[myTexture1.width * j + k].r;
					float pixelValue1 = Mathf.Min(pixelArray1[width * j + k].r +
						pixelArray1[width * j + k].g + pixelArray1[width * j + k].b, 1);
					pixelArray1[width * j + k].a = pixelValue1*alphaMultiplier;

					if (pixelValue1 < cutoff)
					{
						pixelArray1[width * j + k].a = 0;
						pixelArray1[width * j + k].r = 0;
						pixelArray1[width * j + k].g = 0;
						pixelArray1[width * j + k].b = 0;
					}

				}
			}
			// EDGE DETECTION
			if (edgeDetection)
			{
				for (int j = 1; j < height - 1; j++)
				{
					for (int k = 1; k < width - 1; k++)
					{
						float pixelValue = Mathf.Min(pixelArray1[width * j + k].r +
												pixelArray1[width * j + k].g + pixelArray1[width * j + k].b, 1);
						float pixelValueL = Mathf.Min(pixelArray1[width * (j - 1) + k].r +
												pixelArray1[width * (j - 1) + k].g +
												pixelArray1[width * (j - 1) + k].b, 1);
						float pixelValueR = Mathf.Min(pixelArray1[width * (j + 1) + k].r +
							pixelArray1[width * (j + 1) + k].g +
							pixelArray1[width * (j + 1) + k].b, 1);
						float pixelValueU = Mathf.Min(pixelArray1[width * (j) + k + 1].r +
							pixelArray1[width * (j) + k + 1].g +
							pixelArray1[width * (j) + k + 1].b, 1);
						float pixelValueD = Mathf.Min(pixelArray1[width * (j) + k - 1].r +
							pixelArray1[width * (j) + k - 1].g +
							pixelArray1[width * (j) + k - 1].b, 1);
						if (pixelValue == 0)
						{
							if (pixelValueU + pixelValueD + pixelValueR + pixelValueL > 0)
							{
								pixelArray1[width * j + k].a = 1;
								pixelArray1[width * j + k].r = 0;
								pixelArray1[width * j + k].g = 0;
								pixelArray1[width * j + k].b = 0;
							}
						}

					}
				}
			}
			thePlanes[i - imageStart].GetComponent<RenderImagePlane>().setPixels(pixelArray1);
			//thePlanes[i - imageStart].GetComponent<Renderer>().material.SetFloat("_AlphaMult", alphaMultiplier);

		}
	}

	void Start()
	{
		thePlanes = new GameObject[imageStop - imageStart + 1];
		spacing = linspace(0, 1, imageStop - imageStart + 1);

		for (int i = imageStart; i <= imageStop; i++)
		{
			GameObject plane = Instantiate(imagePlanePRE);
			Texture2D myTexture1 = Resources.Load<Texture2D>(path1 + string.Format("{0:D5}", i)) as Texture2D;
			Color[] pixelArray1 = myTexture1.GetPixels();
			plane.GetComponent<RenderImagePlane>().setRawPixels(pixelArray1);

			for (int j = 0; j < myTexture1.height; j++)  // each row
			{
				for (int k = 0; k < myTexture1.width; k++) // each column
				{
					// force single channel across RGB

					//pixelArray1[myTexture1.width * j + k].g = pixelArray1[myTexture1.width * j + k].r;
					//pixelArray1[myTexture1.width * j + k].b = pixelArray1[myTexture1.width * j + k].r;
					float pixelValue1 = Mathf.Min(pixelArray1[myTexture1.width * j + k].r +
						pixelArray1[myTexture1.width * j + k].g + pixelArray1[myTexture1.width * j + k].b, 1);
					pixelArray1[myTexture1.width * j + k].a = pixelValue1*alphaMultiplier;

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
			if (edgeDetection)
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
			plane.transform.parent = transform;
			Material mat = new Material(defaultMat.shader);
			mat.mainTexture = myTexture1;
			plane.transform.localPosition =
					new Vector3(0.0f, spacing[i - imageStart], 0.0f);
			plane.GetComponent<MeshRenderer>().material = mat;
			plane.GetComponent<RenderImagePlane>().setTexture(myTexture1);
			thePlanes[i - imageStart] = plane;
			//thePlanes[i - imageStart].GetComponent<Renderer>().material.SetFloat("_AlphaMult", alphaMultiplier);



		}
		scaleHeight(sliceHeight);

	}

	// Update is called once per frame
	void Update()
	{

	}
}
