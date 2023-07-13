/*
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using SQLite4Unity3d;

public class AuthCodeChecker : MonoBehaviour
{
    private SQLiteConnection dbConnection;
    public InputField authCodeInput;

    private void Start()
    {
        // Set up the database connection
        string databasePath = Path.Combine(Application.streamingAssetsPath, "ConfocalViewer.db");
        dbConnection = new SQLiteConnection(databasePath);
    }

    public void CheckAuthCode()
    {
        string authCode = authCodeInput.text;

        // Prepare the SQL query to check the auth code
        string sqlQuery = "SELECT users.UserID, users.FName, users.Email " +
                          "FROM users " +
                          "JOIN tokens ON users.UserID = tokens.UserID " +
                          "WHERE tokens.AuthCode = ? AND tokens.Expiration > ?";

        // Execute the SQL query
        var result = dbConnection.Query<User>(sqlQuery, authCode, DateTime.Now);

        if (result.Count > 0)
        {
            // Auth code is valid and not expired
            User user = result[0];
            Debug.Log("User ID: " + user.UserID);
            Debug.Log("First Name: " + user.FName);
            Debug.Log("Email: " + user.Email);
        }
        else
        {
            // Auth code is invalid or expired
            Debug.Log("Invalid or expired auth code");
        }
    }
}

public class User
{
    public int UserID { get; set; }
    public string FName { get; set; }
    public string Email { get; set; }
}
*/


