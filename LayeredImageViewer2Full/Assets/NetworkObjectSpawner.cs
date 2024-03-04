using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NetworkObjectSpawner : MonoBehaviour
{
    public string prefabName = "WebModelV"; // Replace with the name of your prefab

    void Start()
    {
        // Check if we are the master client before instantiating the object
        if (PhotonNetwork.IsMasterClient)
        {
            // Instantiate the object across the network
            PhotonNetwork.Instantiate(prefabName, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
