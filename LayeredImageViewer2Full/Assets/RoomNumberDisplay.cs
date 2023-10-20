using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RoomNumberDisplay : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI RN; // Reference to the TMP text component

    void Start()
    {
        // Check if we are in a room
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Player is in a room. Displaying room properties.");
            DisplayRoomProp();
        }
    }

    public override void OnJoinedRoom()
    {
        // Called when the local player successfully joins a room
        Debug.Log("Local player joined the room. Displaying room properties.");
        DisplayRoomProp();
    }

    void DisplayRoomProp()
    {
        // Check if the custom properties exist in the room
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("roomName"))
        {
            // Access and display the values
            string roomName = (string)PhotonNetwork.CurrentRoom.CustomProperties["roomName"];

            //Debug.Log($"Displaying room properties - Model Name: {modelName}, Model Date: {modelDate}, Model JSON Link: {modelJson}");
            string formattedText = $"<size=28><color=white>Room Name: </color></size> <size=28><b><color=lightblue>{roomName}</color></b></size>";

            // Set the text of the TMP text component
            RN.text = formattedText;
        }
        else
        {
            Debug.Log("Room properties are missing or incomplete.");
        }
    }
}
