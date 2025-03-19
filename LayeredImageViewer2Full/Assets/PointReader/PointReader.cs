/*
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

*/


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointReader : MonoBehaviour
{
    // List to store the Vector3 and float values
    private List<Vector3> positionList = new();
    private List<float> sizeList = new();
    public GameObject PointPRE = null;


    public List<float> cen = WebAppRunner.Centroidnumbers;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForList());
    }



    IEnumerator WaitForList()
    {
        while (cen == null || cen.Count == 0)
        {
            yield return null;
        }

        formatPoints();
    }



    private void formatPoints()
    {
        for (int i = 0; i < cen.Count; i += 4)
        {
            if (i + 3 < cen.Count)
            {
                // Parse the values to floats
                float x = cen[i];
                float y = cen[i + 1];
                float z = cen[i + 2];
                float r = cen[i + 3];

                // Add the values to the lists
                positionList.Add(new Vector3(x, y, z));
                sizeList.Add(r);
            }
            else
            {
                Debug.LogError("Invalid data length in cen list.");
            }
        }

        // Display the data for debugging
        for (int i = 0; i < positionList.Count; i++)
        {
            //Debug.Log("Position: " + positionList[i] + ", Size: " + sizeList[i]);
            Debug.Log("Total number of points found: " + positionList.Count);
        }

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
}

*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointReader : MonoBehaviour
{
    // List to store the Vector3 and float values
    private List<Vector3> positionList = new();
    private List<float> sizeList = new();
    public GameObject PointPRE = null;
    public GameObject layeredImageLoader = null; // Reference to the layeredImageLoader

    public List<float> cen = WebAppRunner.Centroidnumbers;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForList());
    }

    IEnumerator WaitForList()
    {
        while (cen == null || cen.Count == 0)
        {
            yield return null;
        }

        formatPoints();
    }

    private void formatPoints()
    {
        // Find min and max values for normalization
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;
        float minZ = float.MaxValue, maxZ = float.MinValue;

        for (int i = 0; i < cen.Count; i += 4)
        {
            if (i + 3 < cen.Count)
            {
                // Parse the values to floats
                float x = cen[i];
                float y = cen[i + 1];
                float z = cen[i + 2];
                float r = cen[i + 3];

                // Update min and max values
                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
                if (y < minY) minY = y;
                if (y > maxY) maxY = y;
                if (z < minZ) minZ = z;
                if (z > maxZ) maxZ = z;

                // Add the values to the lists
                positionList.Add(new Vector3(x, y, z));
                sizeList.Add(r);
            }
            else
            {
                Debug.LogError("Invalid data length in cen list.");
            }
        }

        // Normalize the positions
        for (int i = 0; i < positionList.Count; i++)
        {
            Vector3 pos = positionList[i];

            pos.x = (pos.x - minX) / (maxX - minX); // Normalize x to 0-1
            pos.y = (pos.y - minY) / (maxY - minY); // Normalize y to 0-1
            pos.z = (pos.z - minZ) / (maxZ - minZ); // Normalize z to 0-1

            // Update the position list with normalized values
            positionList[i] = pos;
        }

        // Display the data for debugging
        for (int i = 0; i < positionList.Count; i++)
        {
            //Debug.Log("Position: " + positionList[i] + ", Size: " + sizeList[i]);
        }

        // Log the total number of points found
        Debug.Log("Total number of points found: " + positionList.Count);

        InstantiateGameObjects();
    }

    private void InstantiateGameObjects()
    {
        for (int i = 0; i < positionList.Count; i++)
        {
            // Instantiate a GameObject from the prefab and parent it to the current object
            GameObject obj = Instantiate(PointPRE, layeredImageLoader.transform); // Parent to layeredImageLoader

            // Set the local position and scale
            obj.transform.localPosition = positionList[i];
            obj.transform.localScale = PointPRE.transform.localScale * sizeList[i];
        }
    }
}
