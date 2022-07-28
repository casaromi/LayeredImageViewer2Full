using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    LobbyUI lobby;

    public GameObject loginKey;
    public GameObject chatKey;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        //Open and Close Menus
        if(Input.GetKeyDown(KeyCode.F1))
        {
            loginKey.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            loginKey.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            chatKey.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            chatKey.SetActive(false);
        }

        /*
        //Send Message
        if (Input.GetKeyDown(KeyCode.Return))
        {
            lobby.Send_Event_Message();
        }
        */
    }
}
