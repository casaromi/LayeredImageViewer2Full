using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManageApp : MonoBehaviour
{
   
    public GameObject webAppCentroids;
    public GameObject pointPRE;
    public GameObject parentObject;
    public TextMeshProUGUI warning;
    public TextMeshProUGUI pointsCountText;
    public int nx = 152;
    public int ny = 152;
    public int nz = 89;

    public Transform parentTransform;

    // Array of sprites for input from editor
    public Sprite [] sprites;

    bool centroidsRequested = false;


    private DownloadImages downloadImages;



    IEnumerator Start()
    {
        // Get reference to DownloadImages script
        downloadImages = FindObjectOfType<DownloadImages>();

        // Wait until DownloadImages has finished downloading and populating sprites
        while (downloadImages == null || !downloadImages.spritesReady)
        {
            yield return null;
        }

        // Assign sprites from DownloadImages
        sprites = downloadImages.sprites;

        // Ensure sprites array is properly assigned
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("No sprites available from DownloadImages!");
            yield break;
        }

        InitializePC();
    }


    // Start is called before the first frame update
    void InitializePC()
    {
        // initial test, just package up some simple JSON and send it
        // to an app that parses it, multiplies it by 2, jsonifies the result
        // and sends it back

        Debug.Log(sprites[0].rect);

        centroidsRequested = false;
        warning.enabled = false;

    }

    public void callCentroidsApp()
	{
        if(centroidsRequested == false)
		{
            webAppCentroids.GetComponent<WebAppCentroids>().StartCentroidsCall(sprites);
            centroidsRequested = true;
            warning.enabled = true;
        }
	}

    // Update is called once per frame
    void Update()
    {
        if(centroidsRequested)
		{
            if(!webAppCentroids.GetComponent<WebAppCentroids>().CallRunning)
			{
                foreach(GameObject point in GameObject.FindGameObjectsWithTag("pointPRE"))
				{
                    GameObject.Destroy(point);
				}
                Vector4 [] centroids = webAppCentroids.GetComponent<WebAppCentroids>().centroids;
                for (int i=0;i< centroids.Length;i++)
				{
                    GameObject point = Instantiate(pointPRE);
                    point.transform.parent = parentObject.transform;
                    // pixels count down from top
                    point.transform.localPosition = new Vector3(centroids[i].x/(nx-1)-0.5f,10*centroids[i].z/(nz-1)-0.5f,
                        -(centroids[i].y/(ny-1)-0.5f));
				}

                Debug.Log("Number of points generated: " + centroids.Length);
                pointsCountText.gameObject.SetActive(true);
                pointsCountText.text = "Points Generated: " + centroids.Length;
                parentTransform.GetComponent<ObjectSliderControl>().RefreshSpheres();

                centroidsRequested = false;
                warning.enabled = false;
            }
		}
    }

    

}
