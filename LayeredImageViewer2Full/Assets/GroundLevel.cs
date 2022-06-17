/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLevel : MonoBehaviour
{
    
    void update()
    {
        //Move Ground up

        //Returns true if the primary button (“A”) is currently pressed.
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //Print Debug to consol
            Debug.Log("Button A was pressed!");

            //Move Ground Up
            transform.position += new Vector3(0, 0.2f, 0);
        }
        
        //Returns true if the button (“F3”) is currently pressed.
        if (Input.GetKey (KeyCode.F3)) {  
            transform.position += new Vector3(0, 0.2f, 0);  
            Debug.Log("F3 was pressed!");
        }  
       

        //Move Ground Down

        //Returns true if the primary button (“B”) is currently pressed.
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            //Print Debug to consol
            Debug.Log("Button B was pressed!");

            //Move Ground Up
            transform.position += new Vector3(0, -0.2f, 0);
        }

        //Returns true if the button (“F4”) is currently pressed.
        if (Input.GetKey (KeyCode.F4)) {  
            transform.position += new Vector3(0, -0.2f, 0);    
            Debug.Log("F4 was pressed!");
        }

    }
}
*/