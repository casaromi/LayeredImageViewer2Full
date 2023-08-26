using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastKey : MonoBehaviour
{
    // Start is called before the first frame update
    Text charTxt;
  


    void Start()
    {
        charTxt = transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggered()
    {
        Keyboard.KB.AddChar(charTxt.text);
    }

    public void Delete()
    {
        Keyboard.KB.RemoveChar();
    }

    public void Accept()
    {
        Keyboard.KB.Accept();
        Debug.Log("Accepted text: "+ Keyboard.KB.objectiveInputField.text);
    }

}
