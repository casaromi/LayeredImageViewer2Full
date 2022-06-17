using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject loginKey;
    public GameObject chatKey;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            chatKey.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            chatKey.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            loginKey.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.F1))
        {
            loginKey.SetActive(true);
        }
        
    }
}
