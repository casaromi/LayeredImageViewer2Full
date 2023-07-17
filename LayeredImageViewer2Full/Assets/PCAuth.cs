/*
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
*/





/*
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
    public GameObject RoomUI;

    public GameObject ModelMenu;

    public GameObject instruct;
    public GameObject JoinRoom;

    public GameObject ButtonPrefab;
    public Transform ButtonParent;

    public GameObject Holder;

    private int currentPage = 0;
    private int buttonsPerPage = 2;
    private List<GameObject> instantiatedButtons = new List<GameObject>();

    public static string selectedJsonLink;

    private string phpURL = "https://davidjoiner.net/~confocal/PCuAuth.php";

    public void CallAuth()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        modelNames.Clear();
        jsonLinks.Clear();
        ClearButtons();

        WWWForm form = new WWWForm();
        form.AddField("Email", Email.text);
        form.AddField("Password", Password.text);

        UnityWebRequest request = UnityWebRequest.Post(phpURL, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Web request error: " + request.error);
        }
        else
        {
            string response = request.downloadHandler.text;
            resultText.text = response;

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
                    string creationDateTime = line.Substring(18);
                    creationDateTimes.Add(creationDateTime);
                }
            }

            modelNames.Sort();

            if (fName != null && fName != "")
            {
                HeaderFeild.SetActive(false);
                EmailFeild.SetActive(false);
                PasswordFeild.SetActive(false);
                sButton.SetActive(false);
                Invalid.SetActive(false);
                RoomUI.SetActive(false);

                instruct.SetActive(false);
                JoinRoom.SetActive(true);

                ModelMenu.SetActive(true);

                Holder.SetActive(true);

                DisplayButtons();
            }
            else
            {
                Debug.Log("Invalid email or password");
                Invalid.SetActive(true);
                RoomUI.SetActive(false);
            }
        }
    }

    private void DisplayButtons()
    {
        ClearButtons();

        int startIndex = currentPage * buttonsPerPage;
        int endIndex = Mathf.Min(startIndex + buttonsPerPage, modelNames.Count);

        if (startIndex >= modelNames.Count)
        {
            currentPage = (modelNames.Count - 1) / buttonsPerPage;
            startIndex = currentPage * buttonsPerPage;
            endIndex = Mathf.Min(startIndex + buttonsPerPage, modelNames.Count);
        }

        for (int i = startIndex; i < endIndex; i++)
        {
            string modelName = modelNames[i];

            GameObject buttonObj = Instantiate(ButtonPrefab, ButtonParent);
            instantiatedButtons.Add(buttonObj);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            buttonText.text = modelName;

            int index = i;
            button.onClick.AddListener(() => SelectJsonLink(index));
        }
    }

    private void ClearButtons()
    {
        foreach (GameObject buttonObj in instantiatedButtons)
        {
            Destroy(buttonObj);
        }

        instantiatedButtons.Clear();
    }

    private void SelectJsonLink(int index)
    {
        selectedJsonLink = jsonLinks[index];
        Debug.Log("Selected JsonLink: " + selectedJsonLink);
        RoomUI.SetActive(true);
    }

    public void NextPage()
    {
        currentPage++;
        DisplayButtons();
    }

    public void PreviousPage()
    {
        currentPage--;
        if (currentPage < 0)
            currentPage = 0;
        DisplayButtons();
    }

    public void ShowAllModels()
    {
        modelNames.Sort();
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithNumber()
    {
        List<string> filteredModelNames = new List<string>();
        foreach (string modelName in modelNames)
        {
            if (char.IsNumber(modelName[0]))
            {
                filteredModelNames.Add(modelName);
            }
        }

        filteredModelNames.Sort();
        modelNames = filteredModelNames;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithAtoG()
    {
        List<string> filteredModelNames = new List<string>();
        foreach (string modelName in modelNames)
        {
            if (modelName[0] >= 'A' && modelName[0] <= 'G')
            {
                filteredModelNames.Add(modelName);
            }
        }

        filteredModelNames.Sort();
        modelNames = filteredModelNames;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithHtoN()
    {
        List<string> filteredModelNames = new List<string>();
        foreach (string modelName in modelNames)
        {
            if (modelName[0] >= 'H' && modelName[0] <= 'N')
            {
                filteredModelNames.Add(modelName);
            }
        }

        filteredModelNames.Sort();
        modelNames = filteredModelNames;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithOtoZ()
    {
        List<string> filteredModelNames = new List<string>();
        foreach (string modelName in modelNames)
        {
            if (modelName[0] >= 'O' && modelName[0] <= 'Z')
            {
                filteredModelNames.Add(modelName);
            }
        }

        filteredModelNames.Sort();
        modelNames = filteredModelNames;
        currentPage = 0;
        DisplayButtons();
    }

    private void OnDestroy()
    {
        ClearButtons();
    }
}
*/




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
    public GameObject RoomUI;

    public GameObject ModelMenu;


    public GameObject instruct;
    public GameObject JoinRoom;

    public GameObject ButtonPrefab;
    public Transform ButtonParent;

    public GameObject Holder;


    //BUTTONS Per-Page
    private int currentPage = 0;
    private int buttonsPerPage = 6;
    private List<GameObject> instantiatedButtons = new List<GameObject>();

    public static string selectedJsonLink;

    public static string firstName;
    public static string userEmail;
    public static string userPassword;


    public Button AllButton;
    public Button NumberButton;
    public Button AtoGButton;
    public Button HtoNButton;
    public Button OtoZButton;


    private string phpURL = "https://davidjoiner.net/~confocal/PCuAuth.php";

    private enum FilterType
    {
        All,
        Number,
        AtoG,
        HtoN,
        OtoZ
    }

    private FilterType currentFilter = FilterType.All;

    public void CallAuth()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        modelNames.Clear();
        jsonLinks.Clear();
        ClearButtons();

        WWWForm form = new WWWForm();
        if (string.IsNullOrEmpty(userEmail) && string.IsNullOrEmpty(userPassword))
        {
            form.AddField("Email", Email.text);
            form.AddField("Password", Password.text);

            Debug.Log("!!!NEW USER!!!");
        }
        else
        {
            form.AddField("Email", userEmail);
            form.AddField("Password", userPassword);
            
            Debug.Log("!?!? Welcome back to lobby");
            Debug.Log("Email" + userEmail);
            Debug.Log("Password" + userPassword);
        }

        UnityWebRequest request = UnityWebRequest.Post(phpURL, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Web request error: " + request.error);
        }
        else
        {
            string response = request.downloadHandler.text;
            resultText.text = response;

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
                    string creationDateTime = line.Substring(18);
                    creationDateTimes.Add(creationDateTime);
                }
            }

            modelNames.Sort();

            if (fName != null && fName != "")
            {
                HeaderFeild.SetActive(false);
                EmailFeild.SetActive(false);
                PasswordFeild.SetActive(false);
                sButton.SetActive(false);
                Invalid.SetActive(false);
                RoomUI.SetActive(false);

                instruct.SetActive(false);
                JoinRoom.SetActive(true);

                ModelMenu.SetActive(true);
               

                Holder.SetActive(true);

                DisplayButtons();
            }
            else
            {
                Debug.Log("Invalid email or password");
                Invalid.SetActive(true);
                RoomUI.SetActive(false);
            }
        }
    }

    private void DisplayButtons()
    {
        ClearButtons();

        List<string> filteredModelNames = GetFilteredModelNames();
        int startIndex = currentPage * buttonsPerPage;
        int endIndex = Mathf.Min(startIndex + buttonsPerPage, filteredModelNames.Count);

        if (startIndex >= filteredModelNames.Count)
        {
            currentPage = (filteredModelNames.Count - 1) / buttonsPerPage;
            startIndex = currentPage * buttonsPerPage;
            endIndex = Mathf.Min(startIndex + buttonsPerPage, filteredModelNames.Count);
        }

        for (int i = startIndex; i < endIndex; i++)
        {
            string modelName = filteredModelNames[i];

            GameObject buttonObj = Instantiate(ButtonPrefab, ButtonParent);
            instantiatedButtons.Add(buttonObj);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            buttonText.text = modelName;

            int index = i;
            button.onClick.AddListener(() => SelectJsonLink(index));
        }


        // Set the color of the filter buttons based on the currentFilter
        AllButton.GetComponent<Image>().color = currentFilter == FilterType.All ? Color.blue : Color.white;
        NumberButton.GetComponent<Image>().color = currentFilter == FilterType.Number ? Color.blue : Color.white;
        AtoGButton.GetComponent<Image>().color = currentFilter == FilterType.AtoG ? Color.blue : Color.white;
        HtoNButton.GetComponent<Image>().color = currentFilter == FilterType.HtoN ? Color.blue : Color.white;
        OtoZButton.GetComponent<Image>().color = currentFilter == FilterType.OtoZ ? Color.blue : Color.white;
    }

    private void ClearButtons()
    {
        foreach (GameObject buttonObj in instantiatedButtons)
        {
            Destroy(buttonObj);
        }

        instantiatedButtons.Clear();
    }

    private List<string> GetFilteredModelNames()
    {
        List<string> filteredModelNames = new List<string>();

        switch (currentFilter)
        {
            case FilterType.All:
                filteredModelNames = modelNames;
                break;
            case FilterType.Number:
                filteredModelNames = modelNames.FindAll(modelName => char.IsNumber(modelName[0]));
                break;
            case FilterType.AtoG:
                filteredModelNames = modelNames.FindAll(modelName => modelName[0] >= 'A' && modelName[0] <= 'G');
                break;
            case FilterType.HtoN:
                filteredModelNames = modelNames.FindAll(modelName => modelName[0] >= 'H' && modelName[0] <= 'N');
                break;
            case FilterType.OtoZ:
                filteredModelNames = modelNames.FindAll(modelName => modelName[0] >= 'O' && modelName[0] <= 'Z');
                break;
        }

        return filteredModelNames;
    }

    private void SelectJsonLink(int index)
    {
        selectedJsonLink = jsonLinks[index];
        Debug.Log("Selected JsonLink: " + selectedJsonLink);
        RoomUI.SetActive(true);
    }


    private void StoreUserInfo(int index)
    {
        firstName = fName;
        Debug.Log("Welcome user: " + selectedJsonLink);

        userEmail = Email.text;
        userPassword = Password.text;
    }


    public void NextPage()
    {
        currentPage++;
        DisplayButtons();
    }

    public void PreviousPage()
    {
        currentPage--;
        if (currentPage < 0)
            currentPage = 0;
        DisplayButtons();
    }

    public void ShowAllModels()
    {
        currentFilter = FilterType.All;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithNumber()
    {
        currentFilter = FilterType.Number;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithAtoG()
    {
        currentFilter = FilterType.AtoG;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithHtoN()
    {
        currentFilter = FilterType.HtoN;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithOtoZ()
    {
        currentFilter = FilterType.OtoZ;
        currentPage = 0;
        DisplayButtons();
    }

    private void OnDestroy()
    {
        ClearButtons();
    }
}
