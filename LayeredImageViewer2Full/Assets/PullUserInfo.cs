using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullUserInfo : MonoBehaviour
{
    public static string firstName;
    public static string userEmail;
    public static string userPassword;

    private void Start()
    {
        // Retrieve the data from PlayerPrefs
        firstName = PlayerPrefs.GetString("FirstName", "");
        userEmail = PlayerPrefs.GetString("UserEmail", "");
        userPassword = PlayerPrefs.GetString("UserPassword", "");


        // Now you can use the retrieved data as needed
        Debug.Log("Welcome, " + firstName);
        Debug.Log(userEmail);
        Debug.Log(userPassword);
    }
}
