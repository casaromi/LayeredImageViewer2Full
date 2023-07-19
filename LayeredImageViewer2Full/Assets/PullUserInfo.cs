using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullUserInfo : MonoBehaviour
{
    private string firstName;
    private string userEmail;
    private string userPassword;

    private void Start()
    {
        // Retrieve the data from PlayerPrefs
        firstName = PlayerPrefs.GetString("FirstName", "");
        userEmail = PlayerPrefs.GetString("UserEmail", "");
        userPassword = PlayerPrefs.GetString("UserPassword", "");

        // Now you can use the retrieved data as needed
        Debug.Log("Welcome, " + firstName);
        //Debug.Log(userEmail);
        //Debug.Log(userPassword);
    }
}
