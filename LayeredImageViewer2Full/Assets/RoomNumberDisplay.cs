using UnityEngine;
using TMPro;

public class RoomNumberDisplay : MonoBehaviour
{

    public TextMeshProUGUI RN;

    void Start()
    {
        RN.text = "Room Name: " + NetworkManager.roomName;
    }
}
