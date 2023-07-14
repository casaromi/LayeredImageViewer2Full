using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class PCAuth : MonoBehaviour
{
    public InputField Email;
    public InputField Password;
    public Button submitButton;

    public Text resultText;


    public GameObject HeaderFeild;
    public GameObject EmailFeild;
    public GameObject PasswordFeild;
    public GameObject sButton;
    public GameObject ResultText;

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
            //Turn off login panel 
            HeaderFeild.SetActive(false);
            EmailFeild.SetActive(false);
            PasswordFeild.SetActive(false);
            sButton.SetActive(false);

            //Show results
            ResultText.SetActive(true);

            // Get the response from the PHP script
            string response = request.downloadHandler.text;

            // Update the result text with the response
            resultText.text = response;

            // You can further process the response here as needed
        }
    }
}
