using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class DisplayRoomInfo : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI textField; // Reference to the TMP text component

    void Start()
    {
        // Check if we are in a room
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Player is in a room. Displaying room properties.");
            DisplayRoomProperties();
        }
    }

    public override void OnJoinedRoom()
    {
        // Called when the local player successfully joins a room
        Debug.Log("Local player joined the room. Displaying room properties.");
        DisplayRoomProperties();
    }

    void DisplayRoomProperties()
    {
        // Check if the custom properties exist in the room
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("modelName") &&
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("modelDate") &&
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("modelJson"))
        {
            // Access and display the values
            string modelName = (string)PhotonNetwork.CurrentRoom.CustomProperties["modelName"];
            string modelDate = (string)PhotonNetwork.CurrentRoom.CustomProperties["modelDate"];
            string modelJson = (string)PhotonNetwork.CurrentRoom.CustomProperties["modelJson"];

            //Debug.Log($"Displaying room properties - Model Name: {modelName}, Model Date: {modelDate}, Model JSON Link: {modelJson}");

            // Format the text using Rich Text tags for size and color
            string formattedText = $"<size=28><color=white>{modelName}</color></size>\n<size=20><color=white>{modelDate}</color></size>";

            // Set the text of the TMP text component
            textField.text = formattedText;
        }
        else
        {
            Debug.Log("Room properties are missing or incomplete.");
        }
    }
}
