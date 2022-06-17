using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderImagePlane : MonoBehaviour
{
	RenderTexture renderTexture; // renderTextuer that you will be rendering stuff on
	Renderer renderer; // renderer in which you will apply changed texture
	public Texture2D inputTexture;
	Texture2D texture;
	Color [] pixels;
	bool forceDraw = false;
	bool requestDraw = true;

	public IEnumerator coroutine;

	void Start()
	{
		renderer = GetComponent<Renderer>();
		setTexture(inputTexture);
		coroutine = slowDraw();
		StartCoroutine(coroutine);
	}

	public void setTexture(Texture2D inputTexture)
	{
		renderer = GetComponent<Renderer>();

		this.inputTexture = inputTexture;
		renderTexture = new RenderTexture(inputTexture.width, inputTexture.height, 32);
		Debug.Log(renderTexture.width);
		pixels = inputTexture.GetPixels();
		texture = new Texture2D(renderTexture.width, renderTexture.height);
		renderer.material.mainTexture = texture;
		invalidate();
	}

	public IEnumerator slowDraw()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f * (0.5f + Random.RandomRange(0, 0.5f)));
			if(requestDraw)
			{
				forceDraw = true;
				requestDraw = false;
			}
		}
	}

	public void setPixels(Color [] pixels)
	{
		this.pixels = pixels;
		invalidate();
	}

	public void setPixel(int i, int j, Color pixel)
	{
		pixels[i*texture.width+j] = pixel;
		invalidate();
	}

	public Color [] getPixels()
	{
		return pixels;
	}

	public int getWidth()
	{
		return texture.width;
	}

	public int getHeight()
	{
		return texture.height;
	}

	public Color getPixel(int i, int j)
	{
		return pixels[i*texture.width+j];
	}

	public void invalidate()
	{
		requestDraw = true;
	}

	// Update is called once per frame
	void Update()
	{
		if(forceDraw)
		{
			RenderTexture.active = renderTexture;
			// rendertexture displays its pixels in a reverse order from regular textures
			texture.SetPixels(pixels);
			texture.Apply();
			RenderTexture.active = null;
			forceDraw=false;
		}
	}
}