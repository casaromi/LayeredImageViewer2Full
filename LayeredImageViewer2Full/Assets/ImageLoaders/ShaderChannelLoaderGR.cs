using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderChannelLoaderGR : MonoBehaviour
{
	public int imageStart = 1;
	public int imageStop = 46;
	public GameObject imagePlanePRE;
	public string path1 = "Images/Germ_cells_red/";
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

	public void changeAlpha(float alphaMultiplier)
	{
		this.alphaMultiplier = alphaMultiplier;

		foreach (GameObject plane in thePlanes)
		{
			plane.GetComponent<Renderer>().material.SetFloat("_AlphaMult", alphaMultiplier);
		}
	}

	public void changeCutoff(float cutoff)
	{
		this.cutoff = cutoff;

		foreach (GameObject plane in thePlanes)
		{
			plane.GetComponent<Renderer>().material.SetFloat("_Cutoff", cutoff);
		}
	}

	public void scaleHeight(float scale)
	{
		transform.localScale = new Vector3(1, scale / (spacing[1] - spacing[0]), 1);
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

			plane.transform.parent = transform;
			Material mat = new Material(defaultMat.shader);
			mat.mainTexture = myTexture1;
			plane.transform.localPosition =
					new Vector3(0.0f, spacing[i - imageStart], 0.0f);
			plane.GetComponent<MeshRenderer>().material = mat;
			plane.GetComponent<RenderImagePlane>().setTexture(myTexture1);
			thePlanes[i - imageStart] = plane;
			thePlanes[i - imageStart].GetComponent<Renderer>().material.SetFloat("_AlphaMult", alphaMultiplier);
			thePlanes[i - imageStart].GetComponent<Renderer>().material.SetFloat("_Cutoff", cutoff);



		}
		scaleHeight(sliceHeight);

	}

	// Update is called once per frame
	void Update()
	{

	}
}
