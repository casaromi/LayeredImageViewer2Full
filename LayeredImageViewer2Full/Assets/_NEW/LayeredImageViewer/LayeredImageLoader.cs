using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LayeredImageLoader : MonoBehaviour
{

	public int iSkip = 1;
	public Sprite[] sprites;

	Texture2D[] images;
	Texture2D[] sideImages;
	Texture2D[] frontImages;
	Color[,,] allPixels;
	public GameObject layeredImagePRE;
	GameObject[] layeredImagesObjects;
	GameObject[] frontImagesObjects;
	GameObject[] sideImagesObjects;
	int nImages;
	public Slider alphaSlider;
	public Slider cutoffSlider;
	public Slider saturationSlider;
	int width = 0;
	int height = 0;

	GameObject topView;
	GameObject frontView;
	GameObject sideView;

	public float alphaInit = 0.3f;
	public float cutoffInit = 0.0f;
	public float normalInit = 1.0f;
	public float saturationInit = 1.0f;

	public bool extraPlanes = false;

	bool showSide;
	bool showFront;

	public Vector3 ClipOffset = Vector3.zero;
	public Vector3 ClipNormal = Vector3.one;
	public bool useClipPlane = true;

	void Start()
	{

		showSide = extraPlanes;
		showFront = extraPlanes;
		topView = new GameObject();
		topView.name = "TopView";
		topView.transform.parent = transform;
		topView.transform.localPosition = Vector3.zero;
		topView.transform.localScale = Vector3.one;
		topView.transform.localRotation = Quaternion.Euler(0, 0, 0);

		bool firstPass = true;
		nImages = sprites.Length / iSkip;
		if (sprites.Length % iSkip != 0) nImages += 1;

		images = new Texture2D[nImages];
		layeredImagesObjects = new GameObject[nImages];

		for (int k = 0; k < nImages; k++)
		{
			int imageNumber = iSkip * k;

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

		if (showFront)
		{
			// front view
			frontImages = new Texture2D[height];
			frontImagesObjects = new GameObject[height];
			frontView = new GameObject();
			frontView.name = "FrontView";
			frontView.transform.parent = transform;
			frontView.transform.localPosition = Vector3.zero;
			frontView.transform.localScale = new Vector3(1, 1, 1);
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
				frontImagesObjects[j].transform.localPosition = new Vector3(0, 0, ((float)j / (float)(height - 1) - 0.5f));
				frontImagesObjects[j].transform.localRotation = Quaternion.Euler(0, 0, 0);
				frontImagesObjects[j].transform.localScale = Vector3.one;
				frontImagesObjects[j].GetComponent<Renderer>().material.mainTexture = frontImages[j];
			}
		}

		if (showSide)
		{
			// side view
			sideImages = new Texture2D[width];
			sideImagesObjects = new GameObject[width];
			sideView = new GameObject();
			sideView.name = "SideView";
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
				sideImagesObjects[i].transform.localPosition = new Vector3(((float)i / (float)(width - 1) - 0.5f), 0, 0);
				sideImagesObjects[i].transform.localScale = Vector3.one;
				sideImagesObjects[i].transform.localRotation = Quaternion.Euler(0, -90, 0);
				//sideImagesObjects[i].transform.Rotate(sideImagesObjects[i].transform.right, 90);
				//sideImagesObjects[i].transform.Rotate(sideImagesObjects[i].transform.up, -90);
				sideImagesObjects[i].GetComponent<Renderer>().material.mainTexture = sideImages[i];
			}
		}

		if (alphaSlider != null)
		{
			alphaSlider.value = alphaInit;

		}

		if (cutoffSlider != null)
		{
			cutoffSlider.value = cutoffInit;
		}

		if (saturationSlider != null)
		{
			saturationSlider.value = saturationInit;
		}
		setAlpha(alphaInit);
		setCutoff(cutoffInit);
		setNormal(normalInit);
		setSaturation(saturationInit);

		setAllRenderers("_ClipBase", transform.position + ClipOffset);
		setAllRenderers("_ClipNormal", ClipNormal);

		setClippingAllRenderers(useClipPlane);

	}

	public void setClippingAllRenderers(bool clip)
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer rend in renderers)
		{
			if (clip)
			{
				rend.material.EnableKeyword("CLIPPING_ON");
			}
			else
			{
				rend.material.DisableKeyword("CLIPPING_ON");
			}
		}
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

	Vector4 nonLin;
	public void setNonLin1(float nl1)
	{
		nonLin.x = nl1;
		setAllRenderers("_Nonlinear", nonLin);
	}

	public void setNonLin2(float nl2)
	{
		nonLin.y = nl2;
		setAllRenderers("_Nonlinear", nonLin);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
