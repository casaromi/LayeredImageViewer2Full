using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;

public class PointCloud : MonoBehaviourPunCallbacks
{
    //public string url = "https://davidjoiner.net/~confocal/UserXYZdata/New.txt"; // Replace with your actual URL
    //public string url = DisplayRoomInfo.modelXYZ;

    public Color sphereColor = Color.blue;
    public float sphereScale = 0.2f; // Adjust this value to change the size of the spheres

    private Canvas popupCanvas;
    private Text popupText;

    void Start()
    {
        popupCanvas = CreatePopupCanvas();
    }



    public override void OnJoinedRoom()
    {
        // Called when the local player successfully joins a room
        Debug.Log("Room OPEN!!!");
        StartCoroutine(GetRoomProperties());
    }

    IEnumerator GetRoomProperties()
    {
        //Check if the custom properties exist in the room

        //Access and display the values
        string modelXYZ = DisplayRoomInfo.modelXYZ;

        // Wait until modelJson is not empty
        while (string.IsNullOrEmpty(modelXYZ))
        {
            yield return null; // Wait for one frame
            modelXYZ = DisplayRoomInfo.modelXYZ; // Update modelJson
        }


        Debug.Log("XXXYYYZZZ LINK: " + modelXYZ);

        yield return StartCoroutine(LoadFile(modelXYZ));

    }


    IEnumerator LoadFile(string modelXYZ)
    {
        //Data Request
        Debug.Log("Starting Download: " + modelXYZ);

        UnityWebRequest www = UnityWebRequest.Get(modelXYZ);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string[] lines = www.downloadHandler.text.Split('\n');

            foreach (string line in lines)
            {
                string[] values = line.Split(',');

                if (values.Length == 3)
                {
                    float x, y, z;

                    if (float.TryParse(values[0], out x) &&
                        float.TryParse(values[1], out y) &&
                        float.TryParse(values[2], out z))
                    {
                        Vector3 position = new Vector3(x, y, z + 2);
                        InstantiateSphere(position);
                    }
                    else
                    {
                        Debug.LogError("Invalid data in the line: " + line);
                    }
                }
                else
                {
                    Debug.LogError("Invalid line format: " + line);
                }
            }
        }
        else
        {
            Debug.LogError("Failed to download XYZ file. Error: " + www.error);
        }
    }

    void InstantiateSphere(Vector3 position)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;

        // Set the color of the sphere to blue
        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material.color = sphereColor;

        // Adjust the scale of the sphere
        sphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);

        // Add a collider to the sphere to detect clicks
        sphere.AddComponent<BoxCollider>();

        // Add a script to handle click events on the sphere
        SphereClickHandler clickHandler = sphere.AddComponent<SphereClickHandler>();
        clickHandler.Initialize(popupCanvas, position);
    }

    Canvas CreatePopupCanvas()
    {
        GameObject canvasObject = new GameObject("PopupCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        popupText = new GameObject("PopupText").AddComponent<Text>();
        popupText.transform.SetParent(canvas.transform);

        return canvas;
    }
}
