using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PCAuth : MonoBehaviour
{
    public InputField Email;
    public InputField Password;
    public Button submitButton;

    public Text resultText;

    // Variables to store the data from PHP
    public string fName;
    public List<string> modelNames;
    public List<string> jsonLinks;
    public List<string> creationDateTimes;


    public GameObject HeaderFeild;
    public GameObject EmailFeild;
    public GameObject PasswordFeild;
    public GameObject sButton;
    public GameObject ResultText;
    public GameObject Invalid;

    // The URL of your PHP file on the server
    private string phpURL = "https://davidjoiner.net/~confocal/PCuAuth.php";

    public void CallAuth()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        // Create a WWWForm to send data to the PHP script
        WWWForm form = new WWWForm();

        // Add any parameters you need to send to the PHP script
        form.AddField("Email", Email.text);
        form.AddField("Password", Password.text);

        // Create a UnityWebRequest instance
        UnityWebRequest request = UnityWebRequest.Post(phpURL, form);

        // Send the web request
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Web request error: " + request.error);
        }
        else
        {
            // Get the response from the PHP script
            string response = request.downloadHandler.text;

            // Update the result text with the response
            resultText.text = response;


            // Clear the previous values
            modelNames.Clear();
            jsonLinks.Clear();
            creationDateTimes.Clear();

            // Parse the response and store the variables
            string[] lines = response.Split('\n');
            foreach (string line in lines)
            {
                if (line.StartsWith("FName: "))
                {
                    fName = line.Substring(7);
                }
                else if (line.StartsWith("ModelName: "))
                {
                    string modelName = line.Substring(11);
                    modelNames.Add(modelName);
                }
                else if (line.StartsWith("JsonLink: "))
                {
                    string jsonLink = line.Substring(10);
                    jsonLinks.Add(jsonLink);
                }
                else if (line.StartsWith("CreationDateTime: "))
                {
                    string creationDateTime = line.Substring(10);
                    creationDateTimes.Add(creationDateTime);
                }
            }


            // Check if the email and password are valid
            if (fName != null && fName != "")
            {
                //Turn off login panel 
                HeaderFeild.SetActive(false);
                EmailFeild.SetActive(false);
                PasswordFeild.SetActive(false);
                sButton.SetActive(false);
                Invalid.SetActive(false);
                //Show results
                ResultText.SetActive(true);
                ResultText.SetActive(true);

                // You can access the stored values as arrays
                foreach (string modelName in modelNames)
                {
                    Debug.Log("ModelName: " + modelName);
                }

                foreach (string jsonLink in jsonLinks)
                {
                    Debug.Log("JsonLink: " + jsonLink);
                }
                foreach (string creationDateTime in creationDateTimes)
                {
                    Debug.Log("CreationDateTime: " + creationDateTime);
                }

                // You can further process the response here as needed
            }

            else
            {
                // Invalid email or password
                Debug.Log("Invalid email or password");

                Invalid.SetActive(true);
            }


            

        }
    }
}
