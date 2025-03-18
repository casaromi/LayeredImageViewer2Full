/*
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
        string modelXYZ = DisplayRoomInfo.modelXYZ;

        // Wait until modelJson is not empty
        while (string.IsNullOrEmpty(modelXYZ))
        {
            yield return null; // Wait for one frame
            modelXYZ = DisplayRoomInfo.modelXYZ; // Update modelJson
        }


        Debug.Log("XXXYYYZZZ LINK: " + modelXYZ);

        if (modelXYZ != null && modelXYZ != "null")
        {
            yield return StartCoroutine(LoadFile(modelXYZ));
        }
        else
        {
            Debug.Log("No point cloud data!");
        }

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
*/


using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;

public class PointCloud : MonoBehaviourPunCallbacks
{
    public Color sphereColor = Color.blue;

    public Slider heightSlider;   // Adjust Y position (0 to 2)
    public Slider xPositionSlider; // Adjust X position (-5 to 5)
    public Slider sizeSlider;     // Adjust size (0.1 to 0.5)
    public Toggle pointCloudToggle; // Toggle to show/hide point cloud

    private Canvas popupCanvas;
    private Text popupText;
    private List<GameObject> spawnedSpheres = new List<GameObject>(); // Track all spheres

    private float heightAdjustment = 0f; // Default Y offset
    private float xPositionAdjustment = 0f; // Default X offset
    private float sphereSize = 0.2f; // Default sphere size
    private bool isPointCloudActive = true; // Default ON

    void Start()
    {
        popupCanvas = CreatePopupCanvas();

        // Setup sliders
        if (heightSlider != null)
        {
            heightSlider.minValue = 0f;
            heightSlider.maxValue = 2f;
            heightSlider.value = 0f;
            heightSlider.onValueChanged.AddListener(UpdateHeightAdjustment);
        }

        if (xPositionSlider != null)
        {
            xPositionSlider.minValue = -5f;
            xPositionSlider.maxValue = 5f;
            xPositionSlider.value = 0f;
            xPositionSlider.onValueChanged.AddListener(UpdateXPositionAdjustment);
        }

        if (sizeSlider != null)
        {
            sizeSlider.minValue = 0.1f;
            sizeSlider.maxValue = 0.5f;
            sizeSlider.value = 0.2f;
            sizeSlider.onValueChanged.AddListener(UpdateSizeAdjustment);
        }

        // Setup toggle
        if (pointCloudToggle != null)
        {
            pointCloudToggle.isOn = true; // Default ON
            pointCloudToggle.onValueChanged.AddListener(TogglePointCloud);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room OPEN!!!");
        StartCoroutine(WaitForScript1());
    }

    IEnumerator WaitForScript1()
    {
        while (!DisplayRoomInfo.IsScript1Finished)
        {
            yield return null;
        }
        Debug.Log("DRI SCRIPT COMPLETED!");

        StartCoroutine(GetRoomProperties());
    }

    IEnumerator GetRoomProperties()
    {
        string modelXYZ = DisplayRoomInfo.modelXYZ;

        while (string.IsNullOrEmpty(modelXYZ))
        {
            yield return null;
            modelXYZ = DisplayRoomInfo.modelXYZ;
        }

        Debug.Log("XYZ LINK: " + modelXYZ);

        if (modelXYZ != null && modelXYZ != "null")
        {
            yield return StartCoroutine(LoadFile(modelXYZ));
        }
        else
        {
            Debug.Log("No point cloud data!");
        }
    }

    IEnumerator LoadFile(string modelXYZ)
    {
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
                    if (float.TryParse(values[0], out float x) &&
                        float.TryParse(values[1], out float y) &&
                        float.TryParse(values[2], out float z))
                    {
                        Vector3 position = new Vector3(x, y, z + 2);
                        InstantiateSphere(position);
                    }
                    else
                    {
                        Debug.LogError("Invalid data in line: " + line);
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
        sphere.transform.position = new Vector3(position.x + xPositionAdjustment, position.y + heightAdjustment, position.z);

        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material.color = sphereColor;

        sphere.transform.localScale = Vector3.one * sphereSize; // Set sphere size
        sphere.AddComponent<BoxCollider>();

        SphereClickHandler clickHandler = sphere.AddComponent<SphereClickHandler>();
        clickHandler.Initialize(popupCanvas, position);

        spawnedSpheres.Add(sphere); // Track spawned spheres
    }

    void UpdateHeightAdjustment(float newHeight)
    {
        heightAdjustment = newHeight;
        AdjustSpheres();
    }

    void UpdateXPositionAdjustment(float newXPosition)
    {
        xPositionAdjustment = newXPosition;
        AdjustSpheres();
    }

    void UpdateSizeAdjustment(float newSize)
    {
        sphereSize = newSize;
        AdjustSpheres();
    }

    void AdjustSpheres()
    {
        foreach (GameObject sphere in spawnedSpheres)
        {
            if (sphere != null)
            {
                sphere.transform.position = new Vector3(
                    sphere.transform.position.x + xPositionAdjustment,
                    sphere.transform.position.y + heightAdjustment,
                    sphere.transform.position.z
                );

                sphere.transform.localScale = Vector3.one * sphereSize; // Adjust sphere size
            }
        }
    }

    void TogglePointCloud(bool isActive)
    {
        isPointCloudActive = isActive;
        foreach (GameObject sphere in spawnedSpheres)
        {
            if (sphere != null)
            {
                sphere.SetActive(isPointCloudActive); // Enable/Disable spheres
            }
        }
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
