/*
using UnityEngine;
using System;
using System.Data;
using MySql.Data.MySqlClient;


public class AuthTest : MonoBehaviour
{
    private string servername = "localhost";
    private string username = "ConfocalViewer";
    private string password = "VCon";
    private string dbname = "ConfocalViewer";

    private void Start()
    {
        // Establish connection
        string connectionString = "Server=" + servername + ";Database=" + dbname + ";Uid=" + username + ";Pwd=" + password + ";";
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

            // Retrieve AuthCode and UserID from the user
            string enteredAuthCode = "EnteredAuthCode";
            int enteredUserID = 123; // Replace with the actual UserID entered by the user

            // Check if the AuthCode is valid and not expired
            string tokenQuery = "SELECT * FROM tokens WHERE UserID = @UserID AND AuthCode = @AuthCode AND Expiration > NOW()";
            MySqlCommand tokenCommand = new MySqlCommand(tokenQuery, connection);
            tokenCommand.Parameters.AddWithValue("@UserID", enteredUserID);
            tokenCommand.Parameters.AddWithValue("@AuthCode", enteredAuthCode);
            MySqlDataReader tokenReader = tokenCommand.ExecuteReader();

            if (tokenReader.Read())
            {
                // AuthCode is valid and not expired

                // Retrieve user information based on the UserID
                string userQuery = "SELECT * FROM users WHERE UserID = @UserID";
                MySqlCommand userCommand = new MySqlCommand(userQuery, connection);
                userCommand.Parameters.AddWithValue("@UserID", enteredUserID);
                MySqlDataReader userReader = userCommand.ExecuteReader();

                if (userReader.Read())
                {
                    // Retrieve user information
                    int userID = userReader.GetInt32(0);
                    string fName = userReader.GetString(1);
                    string email = userReader.GetString(2);
                    string password = userReader.GetString(3);

                    Debug.Log("User ID: " + userID);
                    Debug.Log("First Name: " + fName);
                    Debug.Log("Email: " + email);
                    Debug.Log("Password: " + password);
                }
                userReader.Close();

                // Retrieve ConfocalData information based on the UserID
                string confocalDataQuery = "SELECT ModelName, JsonLink, NumImg, CreationDateTime FROM ConfocalData WHERE UserID = @UserID";
                MySqlCommand confocalDataCommand = new MySqlCommand(confocalDataQuery, connection);
                confocalDataCommand.Parameters.AddWithValue("@UserID", enteredUserID);
                MySqlDataReader confocalDataReader = confocalDataCommand.ExecuteReader();

                while (confocalDataReader.Read())
                {
                    // Retrieve ConfocalData information
                    string modelName = confocalDataReader.GetString(0);
                    string jsonLink = confocalDataReader.GetString(1);
                    int numImg = confocalDataReader.GetInt32(2);
                    DateTime creationDateTime = confocalDataReader.GetDateTime(3);

                    Debug.Log("Model Name: " + modelName);
                    Debug.Log("JSON Link: " + jsonLink);
                    Debug.Log("Number of Images: " + numImg);
                    Debug.Log("Creation Date and Time: " + creationDateTime);
                }
                confocalDataReader.Close();
            }
            else
            {
                Debug.Log("Invalid or expired AuthCode");
            }
            tokenReader.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Database Error: " + e.Message);
        }
        finally
        {
            connection.Close();
        }
    }
}
*/