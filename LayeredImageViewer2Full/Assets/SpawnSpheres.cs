/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SpawnSpheres : MonoBehaviour
{
    public string url = "https://davidjoiner.net/~confocal/UserXYZdata/1_XYZ_Test.txt"; // Replace with your actual URL
    public GameObject spherePrefab; // Prefab for the spheres
    public Slider sizeSlider;
    public Slider colorSlider;

    void Start()
    {
        StartCoroutine(LoadFile());
    }

    IEnumerator LoadFile()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

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
                        Vector3 position = new Vector3(x, y, z);
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
            Debug.LogError("Failed to download file. Error: " + www.error);
        }
    }

    void InstantiateSphere(Vector3 position)
    {
        GameObject sphere = Instantiate(spherePrefab, position, Quaternion.identity);
        Renderer sphereRenderer = sphere.GetComponent<Renderer>();

        // Set the initial color and size based on sliders
        float colorValue = colorSlider.value;
        Color initialColor = Color.HSVToRGB(colorValue, 1.0f, 1.0f);
        sphereRenderer.material.color = initialColor;

        float initialSize = sizeSlider.value;
        sphere.transform.localScale = new Vector3(initialSize, initialSize, initialSize);
    }



    public void UpdateSphereSize()
    {
        float newSize = sizeSlider.value;

        foreach (GameObject sphere in GameObject.FindGameObjectsWithTag("SpawnedSphere"))
        {
            sphere.transform.localScale = new Vector3(newSize, newSize, newSize);
        }
    }

    public void UpdateSphereColor()
    {
        float colorValue = colorSlider.value;

        foreach (GameObject sphere in GameObject.FindGameObjectsWithTag("SpawnedSphere"))
        {
            sphere.GetComponent<Renderer>().material.color = Color.HSVToRGB(colorValue, 1.0f, 1.0f);
        }
    }
}
*/




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SpawnSpheres : MonoBehaviour
{
    public string url = "https://davidjoiner.net/~confocal/UserXYZdata/test.txt"; // Replace with your actual URL
    public Color sphereColor = Color.blue;
    public float sphereScale = 0.25f; // Adjust this value to change the size of the spheres

    void Start()
    {
        StartCoroutine(LoadFile());
    }

    IEnumerator LoadFile()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

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
                        Vector3 position = new Vector3(x, y, z);
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
            Debug.LogError("Failed to download file. Error: " + www.error);
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
    }

}
