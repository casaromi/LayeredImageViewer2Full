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

    public Text roomNumberText;

    public static string roomName;

    private Dictionary<string, RoomInfo> customRoomList = new Dictionary<string, RoomInfo>();

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

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);


        // Load Scene locally
        StartCoroutine(LoadSceneAsync(roomSettings.sceneIndex));
    }


    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadingScreen.SetActive(false);
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        customRoomList.Clear();
        foreach (RoomInfo room in roomList)
        {
            if (room.CustomProperties.TryGetValue("RoomNumber", out object roomNumObj))
            {
                string roomNum = roomNumObj.ToString();
                if (!customRoomList.ContainsKey(roomNum))
                {
                    customRoomList.Add(roomNum, room);
                }
            }
        }
    }

    public void JoinRoom(string roomNumber)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Not connected to the server.");
            return;
        }

        if (customRoomList.TryGetValue(roomNumber, out RoomInfo roomToJoin))
        {
            PhotonNetwork.JoinRoom(roomToJoin.Name);
        }
        else
        {
            Debug.LogWarning("Room with number " + roomNumber + " does not exist.");
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
            string roomNum = roomNumObj.ToString();
            PlayerPrefs.SetString("RoomNumber", roomNum);
            LoadNextScene(); // Load the next scene after storing the room number
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
        loadingScreen.SetActive(false);
    }
}

