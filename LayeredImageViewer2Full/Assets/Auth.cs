using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Auth : MonoBehaviour
{
    public InputField AuthCodeInput;
    public Button submitButton;

    public Text resultText;


    public void CallAuth()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("AuthCode", AuthCodeInput.text);

        //WWW www = new WWW("https://davidjoiner.net/home/confocal/public_html/Auth.php");
       
        WWW www = new WWW("https://davidjoiner.net/~confocal/uAuth.php");
    
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Welcome");

        }
        else
        {
            Debug.Log("Error!" + www.text);
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (AuthCodeInput.text.Length == 6);
    }


}
