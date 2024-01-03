using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PointReader : MonoBehaviour
{

    // List to store the Vector3 and float values
    private List<Vector3> positionList = new();
    private List<float> sizeList = new();
    public string filename = "PointSet.txt";
    public GameObject PointPRE = null;

    // Start is called before the first frame update
    void Start()
    {
        ReadFileFromStreamingAssets(filename);
        InstantiateGameObjects();
    }

    private void InstantiateGameObjects()
    {
        for (int i = 0; i < positionList.Count; i++)
        {
            // Instantiate a GameObject from the prefab and parent it to the current object
            GameObject obj = Instantiate(PointPRE, transform);

            // Set the local position and scale
            obj.transform.localPosition = positionList[i];
            obj.transform.localScale = PointPRE.transform.localScale * sizeList[i];
        }
    }

    private void ReadFileFromStreamingAssets(string fileName)
    {
        // Path to the file
        string path = Path.Combine(Application.streamingAssetsPath, fileName);

        // Check if file exists
        if (!File.Exists(path))
        {
            Debug.LogError("File not found at " + path);
            return;
        }

        // Read the file
        string[] lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++) // Start from 1 to skip the header
        {
            string line = lines[i];
            // Split the line by whitespace
            string[] values = line.Split(new char[0], System.StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 4)
            {
                // Parse the values to floats
                float x = float.Parse(values[0]);
                float z = float.Parse(values[1]);
                float y = float.Parse(values[2]);
                float r = float.Parse(values[3]);

                // Add the values to the lists
                positionList.Add(new Vector3(x, y, z));
                sizeList.Add(r);
            }
            else
            {
                Debug.LogError("Invalid line format: " + line);
            }
        }

        // Display the data for debugging
        for (int i = 0; i < positionList.Count; i++)
        {
            Debug.Log("Position: " + positionList[i] + ", Size: " + sizeList[i]);
        }
    }
}

