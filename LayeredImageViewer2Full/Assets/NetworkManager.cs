/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject roomUI;

    // Update is called once per frame
    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined the Lobby");

        roomUI.SetActive(true);
    }

    public void InitiliazeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //Load Scene
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);

        //Create Room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
*/






/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject roomUI;
    public GameObject loadingScreen;
    public Slider loadingBar;

    // Update is called once per frame
    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined the Lobby");

        roomUI.SetActive(true);
    }

    public void InitiliazeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        StartCoroutine(LoadRoomAsync(roomSettings));
    }

    private IEnumerator LoadRoomAsync(DefaultRoom roomSettings)
    {
        loadingScreen.SetActive(true); // Show the loading screen
        loadingBar.value = 0f; // Set the initial value of the loading bar to 0

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(roomSettings.sceneIndex); // Load the room scene asynchronously
        asyncLoad.allowSceneActivation = false; // Prevent the scene from activating immediately

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Calculate the progress based on time

            if (progress >= 0.9f)
            {
                // The progress is close to 90% or higher, so we'll simulate a delay
                // to give the impression of a gradual loading process

                float timeElapsed = 0f;
                while (timeElapsed < 1f) // Simulate a 1-second delay
                {
                    timeElapsed += Time.deltaTime;
                    progress = Mathf.Lerp(0.9f, 1f, timeElapsed); // Gradually increase the progress

                    loadingBar.value = progress; // Update the loading bar UI

                    yield return null;
                }

                asyncLoad.allowSceneActivation = true; // Allow the scene to activate
            }
            else
            {
                loadingBar.value = progress; // Update the loading bar UI
            }

            yield return null;
        }
    }



    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
*/


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject roomUI;

    // Update is called once per frame
    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined the Lobby");

        roomUI.SetActive(true);
    }

    public void InitiliazeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //Load Scene
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);

        //Create Room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
*/





/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject roomUI;
    public GameObject loadingScreen;


    // Update is called once per frame
    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined the Lobby");

        //roomUI.SetActive(true);
    }

    public void InitiliazeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //Load Scene
        loadingScreen.SetActive(true);
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);

        //Create Room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
        loadingScreen.SetActive(false);
    }
}

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject roomUI;
    public GameObject loadingScreen;
    public GameObject LoginFeilds;
    public GameObject ButtonParent;
    public GameObject invalidJoin;

    public PCAuth pcAuthScript;

    public TMP_InputField roomNumberText;

    public static string roomName;

    private Dictionary<string, RoomInfo> customRoomList = new Dictionary<string, RoomInfo>();
    

    public static string modelName;
    public static string modelDate;
    public static string modelJson;
    public static string modelXYZ;


    // Update is called once per frame
    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined the Lobby");
        //roomUI.SetActive(true);
        //LoginFeilds.SetActive(false);
        //ButtonParent.SetActive(true);
        //PCAuth.CallAuth();
        pcAuthScript.CallAuth();
    }





    public void LoadNextScene()
    {
        SceneManager.LoadScene("NextSceneName"); // Replace "NextSceneName" with the name of your second scene
    }

    public void InitializeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        // Generate a unique room name by appending a random 6-digit number
        roomName = roomSettings.Name + "_" + Random.Range(100000, 1000000);

        Debug.Log("Room Name: " + roomName);

        // Create Room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        //Room settings
        modelName = PCAuth.selectedModelName;
        modelDate = PCAuth.selectedModelDate;
        modelJson = PCAuth.selectedJsonLink;
        modelXYZ = PCAuth.selectedXYZLink;


        // Convert the variables to a Hashtable -> god tier command
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable
    {
        { "roomName", roomName },
        { "modelName", modelName },
        { "modelDate", modelDate },
        { "modelJson", modelJson },
        { "modelXYZ", modelXYZ }
    };

        roomOptions.CustomRoomProperties = customRoomProperties;
        roomOptions.CustomRoomPropertiesForLobby = new[] { "roomName", "modelName", "modelDate", "modelJson", "modelXYZ" };

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);

        // Load Scene locally immediately after joining the room
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);
    }



    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        customRoomList.Clear();

        foreach (RoomInfo room in roomList)
        {
            customRoomList.Add(room.Name, room);
        }
    }


    public void JoinRoom(int defaultRoomIndex)
    {
        // Get the 6-digit number from the TMP Input Field
        string roomNumber = roomNumberText.text;
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        // Check if the room number is valid (6 digits)
        if (roomNumber.Length != 6 || !int.TryParse(roomNumber, out int roomNumberInt))
        {
            Debug.Log("Invalid room number format. Please enter a 6-digit number.");
            invalidJoin.SetActive(true);
            return;
        }

        // Generate the room name by adding the 6 digits to the prefix
        string roomNameToJoin = "Lab_" + roomNumber;
        Debug.Log("Attempting to Join: " + roomNameToJoin);

        // Check if the room exists in the customRoomList dictionary
        if (customRoomList.TryGetValue(roomNameToJoin, out RoomInfo roomInfo))
        {
            // Join the existing room
            PhotonNetwork.JoinRoom(roomNameToJoin);
            PhotonNetwork.LoadLevel(roomSettings.sceneIndex);
        }
        else
        {
            Debug.Log("Room does not exist.");
            // Display a message to the user that the room does not exist (you can show/hide a UI element here)
            invalidJoin.SetActive(true);
        }
    }






    public void LeaveRoomAndGoToLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        loadingScreen.SetActive(true);
        PhotonNetwork.LoadLevel(0); // Load the lobby scene (scene 0)

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("RoomNumber", out object roomNumObj))
        {
            // Remove the following line (you don't need it)
            // PlayerPrefs.SetString("RoomNumber", roomNumObj.ToString());

            // LoadNextScene(); // Don't load the next scene here, as it's already loaded in InitializeRoom method
        }
    }



    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
        loadingScreen.SetActive(false);
    }
}

