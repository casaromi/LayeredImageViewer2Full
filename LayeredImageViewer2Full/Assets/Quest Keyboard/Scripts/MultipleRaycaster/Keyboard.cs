using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Import TextMeshPro namespace
using UnityEngine.Events;
using System;

[Serializable]
public class MyEvent : UnityEvent { }

public class Keyboard : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField objectiveInputField;  // Use TMP_InputField for TextMeshPro
    public static Keyboard KB;

    public MyEvent acceptEvent;

    void Start()
    {
        KB = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Your update logic here
    }

    // Adding a letter to the input
    public void AddChar(string c)
    {
        objectiveInputField.text += c;
    }

    // Removing a letter from the input
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
}
