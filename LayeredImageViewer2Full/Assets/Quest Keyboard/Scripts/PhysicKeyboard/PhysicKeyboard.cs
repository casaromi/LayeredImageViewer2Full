using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicKeyboard : MonoBehaviour
{
    // Start is called before the first frame update
    public static PhysicKeyboard PKB;
    public MyEvent acceptEvent;
    public Text objectiveInputField;
    public bool lowerCase = true;
    public GameObject upperCaseGo, lowerCaseGo;

    void Start()
    {
        PKB = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //adding a letter to the input
    public void AddChar(string c)
    {
        objectiveInputField.text += c;
    }

    //removing a letter from the input
    public void RemoveChar()
    {
        string actualText = objectiveInputField.text;

        if (actualText.Length > 0)
        {
            objectiveInputField.text = actualText.Remove(actualText.Length - 1);
        }
    }


    public void Accept()
    {
        acceptEvent.Invoke();
    }

    public void ChangeUperLower()
    {
        if(lowerCase)
        {
            lowerCase = false;
            
        }
        else
        {
            lowerCase = true;
        }

        upperCaseGo.SetActive(!lowerCase);
        lowerCaseGo.SetActive(lowerCase);
    }

}
