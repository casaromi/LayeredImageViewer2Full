using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomImagePlane : MonoBehaviour
{

	Color [] originalPixels;
	Color [] pixels;

	// Start is called before the first frame update
	void Start()
    {
        originalPixels = GetComponent<RenderImagePlane>().getPixels();
		pixels = new Color[originalPixels.Length];
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = originalPixels[i];
		}
	}

	public void erase(Vector3 center, float radius)
	{

		int width = GetComponent<RenderImagePlane>().getWidth();
		int height = GetComponent<RenderImagePlane>().getHeight();
		Vector3 scale = transform.localScale*10;
		Vector3 lpdebug = new Vector3(1, 0, 1);
		Vector3 worlddebug = transform.TransformPoint(lpdebug);
		bool pixelsChanged = false;
		float r2 = radius*radius;
		for(int ip=0;ip<height;ip++) {
			float zp = (ip - height / 2) * scale.z / height;
			for (int jp=0;jp<width;jp++) { 
				int i=ip*width+jp;
				if (pixels[i].a > 0) { 
					float xp = (jp - width / 2) * scale.x/width;
					Vector3 world = transform.TransformPoint(-xp,0,-zp);
					if((world-center).sqrMagnitude<r2)
					{
						pixels[i] = Color.black;
						pixels[i].a=0;
						pixelsChanged= true;
					}
				}
			}
		}
		if(pixelsChanged)
		{
			GetComponent<RenderImagePlane>().setPixels(pixels);
		}

	}

    // Update is called once per frame
    void Update()
    {
		
	}
}
