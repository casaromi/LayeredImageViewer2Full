using System;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class DisplayRoomInfo : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI textField; // Reference to the TMP text component


    public static string modelJson;
    public static string modelXYZ;

    private static bool isScript1Finished = false;

    public static bool IsScript1Finished
    {
        get { return isScript1Finished; }
    }


    void Start()
    {
        isScript1Finished = false;

        // Check if we are in a room
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Player is in a room. Displaying RP.");
            DisplayRoomProperties();
        }
    }

    public override void OnJoinedRoom()
    {
        isScript1Finished = false;

        // Called when the local player successfully joins a room
        Debug.Log("Local player joined the room. Displaying RP.");
        DisplayRoomProperties();
    }

    void DisplayRoomProperties()
    {
        // Check if the custom properties exist in the room
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("modelName") &&
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("modelDate") &&
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("modelJson") &&
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("modelXYZ"))
        {
            // Access and display the values
            string modelName = (string)PhotonNetwork.CurrentRoom.CustomProperties["modelName"];
            string modelDateString = (string)PhotonNetwork.CurrentRoom.CustomProperties["modelDate"];
            modelJson = (string)PhotonNetwork.CurrentRoom.CustomProperties["modelJson"];
            modelXYZ = (string)PhotonNetwork.CurrentRoom.CustomProperties["modelXYZ"];

            // Parse the date string into a DateTime object
            if (DateTime.TryParse(modelDateString, out DateTime modelDate))
            {
                // Format the date in "Month Day, Year" format
                modelDateString = modelDate.ToString("MMMM dd, yyyy");
            }
            else
            {
                Debug.LogError("Failed to parse the model date string.");
            }

            // Format the text using Rich Text tags for size and color
            string formattedText = $"<size=28><color=white>{modelName}</color></size>\n<size=20><color=white>{modelDateString}</color></size>";

            // Set the text of the TMP text component
            textField.text = formattedText;
        }
        else
        {
            Debug.Log("Room properties are missing or incomplete.");
        }

        isScript1Finished = true;
    }
}
