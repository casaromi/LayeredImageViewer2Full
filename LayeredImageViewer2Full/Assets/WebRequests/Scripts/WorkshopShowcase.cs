using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using CodeMonkey;
using CodeMonkey.Utils;

public class WorkshopShowcase : MonoBehaviour {
    
    private List<Texture2D> workshopThumbnailList = new List<Texture2D>();
    private Transform container;
    private Transform thumbnailTemplate;

    private void Awake() {
        container = transform.Find("containerMask").Find("container");
        thumbnailTemplate = container.Find("thumbnailTemplate");
    }

    private void Start() {
        DownloadWorkshopShowcase();
    }

    private void PrintThumbnails() {
        // Clear Previous Thumbnails
        foreach (Transform child in container) {
            if (child == thumbnailTemplate) continue;
            Destroy(child.gameObject);
        }

        // Create Thumbnails
        float totalWidth = workshopThumbnailList.Count * 108;
        for (int i=0; i<workshopThumbnailList.Count; i++) {
            Transform thumbnailTransform = Instantiate(thumbnailTemplate, container);
            thumbnailTransform.gameObject.SetActive(true);

            Vector2 startingPos = new Vector2(108 * i, 0);
            Vector2 pos = startingPos;
            RectTransform rectTransform = thumbnailTransform.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = startingPos;
            thumbnailTransform.GetComponent<RawImage>().texture = workshopThumbnailList[i];

            FunctionUpdater.Create(delegate () {
                if (rectTransform == null) return true;
                pos += new Vector2(-100, 0) * Time.deltaTime;
                if (pos.x < -430) pos += new Vector2(totalWidth, 0);
                rectTransform.anchoredPosition = pos;
                return false;
            });
        }
    }

    private void RandomizeList() {
        if (workshopThumbnailList == null || workshopThumbnailList.Count == 0) return;
        // Randomize list
        for (int i=0; i<50; i++) {
            int rnd = UnityEngine.Random.Range(0, workshopThumbnailList.Count);
            Texture2D tmp = workshopThumbnailList[0];
            workshopThumbnailList[0] = workshopThumbnailList[rnd];
            workshopThumbnailList[rnd] = tmp;
        }
    }
    
	private void DownloadWorkshopShowcase() {
        // Manually curated collection
        //string url = "http://steamcommunity.com/sharedfiles/filedetails/?id=1222955566"; // Hyper Knights: Battles
        //string url = "http://steamcommunity.com/sharedfiles/filedetails/?id=1314094214"; // Ninja Tycoon
        string url = "http://steamcommunity.com/sharedfiles/filedetails/?id=1849057378"; // Battle Royale Tycoon
        
        Debug.Log("Downloading Workshop Showcase...");
        Get(url, (string error) => {
			Debug.Log("Could not contact Steam Workshop Showcase");
            Debug.Log("Error: " + error);
        }, (string htmlCode) => { 
			Debug.Log("Steam Workshop Showcase downloaded");
            // Download images
            string textToFind;
            int cycleProtection = 0;
            while (htmlCode.IndexOf("<img class=\"workshopItemPreviewImage") != -1 && cycleProtection < 100) {
                cycleProtection++;
                textToFind = "<img class=\"workshopItemPreviewImage";
                htmlCode = htmlCode.Substring(htmlCode.IndexOf(textToFind) + textToFind.Length);
                textToFind = "src=\"";
                htmlCode = htmlCode.Substring(htmlCode.IndexOf(textToFind) + textToFind.Length);
                string imageUrl = htmlCode.Substring(0, htmlCode.IndexOf("\""));

                GetTexture(imageUrl, (string error) => {
                    Debug.Log("Failed to download thumbnail");
                    Debug.Log("Error: " + error);
                }, (Texture2D texture) => { 
                    workshopThumbnailList.Add(texture);
                    RandomizeList();
                    PrintThumbnails();
                    Debug.Log("Workshop showcase amount: " + workshopThumbnailList.Count);
                });
            }
        });
	}
    
    public void Get(string url, Action<string> onError, Action<string> onSuccess) {
        WebRequests.Get(url, onError, onSuccess);
    }

    public void GetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess) {
        WebRequests.GetTexture(url, onError, onSuccess);
    }

}
