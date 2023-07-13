/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;

public class AuthCodeChecker : MonoBehaviour
{
    public InputField authCodeInput;
    public Text resultText;

    private string serverName = "localhost";
    private string userName = "ConfocalViewer";
    private string password = "VCon";
    private string dbName = "ConfocalViewer";

    private void Start()
    {
        // Replace with your own database connection details
        serverName = "localhost";
        userName = "ConfocalViewer";
        password = "VCon";
        dbName = "ConfocalViewer";
    }

    public void CheckAuthCode()
    {
        string authCode = authCodeInput.text;
        if (string.IsNullOrEmpty(authCode))
        {
            resultText.text = "Please enter an authentication code.";
            return;
        }

        string connectionString = $"Server={serverName};Database={dbName};Uid={userName};Pwd={password};";
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            // Check if the authentication code is valid and not expired
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText = "SELECT UserID, Expiration FROM tokens WHERE AuthCode = @authCode";
                dbCmd.Parameters.AddWithValue("@authCode", authCode);

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string userID = reader.GetString(0);
                        DateTime expiration = reader.GetDateTime(1);

                        // Check if the token has expired
                        if (expiration > DateTime.Now)
                        {
                            // Retrieve user information based on the UserID
                            using (IDbCommand userCmd = dbConnection.CreateCommand())
                            {
                                userCmd.CommandText = "SELECT FName, Email, Password FROM users WHERE UserID = @userID";
                                userCmd.Parameters.AddWithValue("@userID", userID);

                                using (IDataReader userReader = userCmd.ExecuteReader())
                                {
                                    if (userReader.Read())
                                    {
                                        string fName = userReader.GetString(0);
                                        string email = userReader.GetString(1);
                                        string password = userReader.GetString(2);

                                        // Retrieve additional information from ConfocalData table
                                        using (IDbCommand confocalCmd = dbConnection.CreateCommand())
                                        {
                                            confocalCmd.CommandText = "SELECT ModelName, JsonLink, NumImg, CreationDateTime FROM ConfocalData WHERE UserID = @userID";
                                            confocalCmd.Parameters.AddWithValue("@userID", userID);

                                            using (IDataReader confocalReader = confocalCmd.ExecuteReader())
                                            {
                                                if (confocalReader.Read())
                                                {
                                                    string modelName = confocalReader.GetString(0);
                                                    string jsonLink = confocalReader.GetString(1);
                                                    int numImg = confocalReader.GetInt32(2);
                                                    DateTime creationDateTime = confocalReader.GetDateTime(3);

                                                    // Display all the retrieved information
                                                    resultText.text = $"User Information:\n\nName: {fName}\nEmail: {email}\nPassword: {password}\n\n";
                                                    resultText.text += $"Confocal Data:\n\nModel Name: {modelName}\nJSON Link: {jsonLink}\nNumber of Images: {numImg}\nCreation Date: {creationDateTime}";
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            resultText.text = "Authentication code has expired.";
                            return;
                        }
                    }
                }
            }

            resultText.text = "Invalid authentication code.";
        }
    }
}
*/

