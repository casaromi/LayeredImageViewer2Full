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







/*
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;

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
    private int buttonsPerPage = 4;
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
            // Retrieve the data from PlayerPrefs
            form.AddField("Email", userEmail);
            form.AddField("Password", userPassword);
            
            Debug.Log("!?!? Welcome back to lobby");
            Debug.Log(userEmail);
            Debug.Log(userPassword);
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
                // Call StoreUserInfo to store user information in PlayerPrefs
                StoreUserInfo(0);


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
            string creationDateTimeString = creationDateTimes[i];

            // Parse the CreationDateTime string into a DateTime object
            if (DateTime.TryParse(creationDateTimeString, out DateTime creationDateTime))
            {
                // Format the date as "month, day, year"
                string formattedDate = creationDateTime.ToString("MMMM dd, yyyy");

                GameObject buttonObj = Instantiate(ButtonPrefab, ButtonParent);
                instantiatedButtons.Add(buttonObj);
                Button button = buttonObj.GetComponent<Button>();
                TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();

                // Use rich text formatting to apply different colors and font size to modelName and formattedDate
                string formattedText = $"<size=20><color=blue>{modelName}</color></size>\n<size=10><color=red>{formattedDate}</color></size>";
                buttonText.text = formattedText;

                int index = i;
                button.onClick.AddListener(() => SelectJsonLink(index));
            }
            else
            {
                Debug.LogWarning("Invalid date format: " + creationDateTimeString);
            }
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
        if (string.IsNullOrEmpty(userEmail) && string.IsNullOrEmpty(userPassword) && string.IsNullOrEmpty(firstName))
        {
            Debug.Log("!!!!!!!!!");

            firstName = fName;
            userEmail = Email.text;
            userPassword = Password.text;

            // Save the data using PlayerPrefs
            PlayerPrefs.SetString("FirstName", firstName);
            PlayerPrefs.SetString("UserEmail", userEmail);
            PlayerPrefs.SetString("UserPassword", userPassword);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("!!!!!User Already logged in");
        }
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
    


    // Add this method to clear player preferences on application quit
    private void OnApplicationQuit()
    {
        ClearPlayerPrefs();
    }

    // Method to clear player preferences
    private void ClearPlayerPrefs()
    {
        // Clear all player preferences
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
*/




/*
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;
using System.Linq;

public class PCAuth : MonoBehaviour
{
    public TMP_InputField Email;    // Changed to TMP_InputField
    public TMP_InputField Password; // Changed to TMP_InputField
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
    private int buttonsPerPage = 4;
    private List<GameObject> instantiatedButtons = new List<GameObject>();

    public static string selectedJsonLink;

    public static string selectedModelName;
    public static string selectedModelDate;

    public static string firstName;
    public static string userEmail;
    public static string userPassword;


    public Button AllButton;
    public Button NumberButton;
    public Button AtoGButton;
    public Button HtoNButton;
    public Button OtoZButton;


    public Button NameButton;
    public Button DateButton;
    public Button AscendingButton;
    public Button DescendingButton;


    private string phpURL = "https://davidjoiner.net/~confocal/PCuAuth.php";

    private enum PrimaryFilterType
    {
        All,
        Number,
        AtoG,
        HtoN,
        OtoZ
    }

    private enum SecondaryFilterType
    {
        Date,
        Name
    }

    private enum SortType
    {
        Ascending,
        Descending
    }

    private PrimaryFilterType currentPrimaryFilter = PrimaryFilterType.All;
    private SecondaryFilterType currentSecondaryFilter = SecondaryFilterType.Name; // Default to Name filter
    private SortType currentSortType = SortType.Ascending; // Default to Ascending


    internal bool isInputFieldSelected;

    public void CallAuth()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        modelNames.Clear();
        jsonLinks.Clear();
        creationDateTimes.Clear();
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
            // Retrieve the data from PlayerPrefs
            form.AddField("Email", userEmail);
            form.AddField("Password", userPassword);

            Debug.Log("!?!? Welcome back to lobby");
            Debug.Log(userEmail);
            Debug.Log(userPassword);
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
                // Call StoreUserInfo to store user information in PlayerPrefs
                StoreUserInfo(0);


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



    // Create a data structure to hold all relevant information together
    private class ModelData
    {
        public string modelName;
        public string creationDateTime;
        public string jsonLink;
    }

    private List<ModelData> modelDataList = new List<ModelData>();


    private void DisplayButtons()
    {
        ClearButtons();

        List<ModelData> filteredModelData = GetFilteredModelData();
        int startIndex = currentPage * buttonsPerPage;
        int endIndex = Mathf.Min(startIndex + buttonsPerPage, filteredModelData.Count);

        if (startIndex >= filteredModelData.Count)
        {
            currentPage = (filteredModelData.Count - 1) / buttonsPerPage;
            startIndex = currentPage * buttonsPerPage;
            endIndex = Mathf.Min(startIndex + buttonsPerPage, filteredModelData.Count);
        }

        for (int i = startIndex; i < endIndex; i++)
        {
            ModelData modelData = filteredModelData[i];
            string modelName = modelData.modelName;
            string creationDateTimeString = modelData.creationDateTime;

            // Parse the CreationDateTime string into a DateTime object
            if (DateTime.TryParse(creationDateTimeString, out DateTime creationDateTime))
            {
                // Format the date as "month, day, year"
                string formattedDate = creationDateTime.ToString("MMMM dd, yyyy");

                GameObject buttonObj = Instantiate(ButtonPrefab, ButtonParent);
                instantiatedButtons.Add(buttonObj);
                Button button = buttonObj.GetComponent<Button>();
                TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();

                // Use rich text formatting to apply different colors and font size to modelName and formattedDate
                string formattedText = $"<size=20><color=blue>{modelName}</color></size>\n<size=10><color=red>{formattedDate}</color></size>";
                buttonText.text = formattedText;

                // Set the text overflow mode to truncate
                buttonText.overflowMode = TextOverflowModes.Truncate;

                // Set the margin for the truncation area (left, right, top, bottom)
                buttonText.margin = new Vector4(20f, 5f, 20f, 5f); // Adjust the values as needed

                int index = i;
                button.onClick.AddListener(() => StoreUserInfo(index));
            }
            else
            {
                Debug.LogWarning("Invalid date format: " + creationDateTimeString);
            }
        }

        // Set the color of the filter buttons based on the currentFilter
        AllButton.GetComponent<Image>().color = currentPrimaryFilter == PrimaryFilterType.All ? Color.blue : Color.white;
        NumberButton.GetComponent<Image>().color = currentPrimaryFilter == PrimaryFilterType.Number ? Color.blue : Color.white;
        AtoGButton.GetComponent<Image>().color = currentPrimaryFilter == PrimaryFilterType.AtoG ? Color.blue : Color.white;
        HtoNButton.GetComponent<Image>().color = currentPrimaryFilter == PrimaryFilterType.HtoN ? Color.blue : Color.white;
        OtoZButton.GetComponent<Image>().color = currentPrimaryFilter == PrimaryFilterType.OtoZ ? Color.blue : Color.white;

        NameButton.GetComponent<Image>().color = currentSecondaryFilter == SecondaryFilterType.Name ? Color.blue : Color.white;
        DateButton.GetComponent<Image>().color = currentSecondaryFilter == SecondaryFilterType.Date ? Color.blue : Color.white;

        AscendingButton.GetComponent<Image>().color = currentSortType == SortType.Ascending ? Color.blue : Color.white;
        DescendingButton.GetComponent<Image>().color = currentSortType == SortType.Descending ? Color.blue : Color.white;
    }





    private void ClearButtons()
    {
        foreach (GameObject buttonObj in instantiatedButtons)
        {
            Destroy(buttonObj);
        }

        instantiatedButtons.Clear();
    }

    private List<ModelData> GetFilteredModelData()
    {
        List<ModelData> filteredModelData = new List<ModelData>();

        // Apply the primary filter
        switch (currentPrimaryFilter)
        {
            case PrimaryFilterType.All:
                filteredModelData.AddRange(modelDataList);
                break;
            case PrimaryFilterType.Number:
                filteredModelData.AddRange(modelDataList.FindAll(data => char.IsNumber(data.modelName[0])));
                break;
            case PrimaryFilterType.AtoG:
                filteredModelData.AddRange(modelDataList.FindAll(data => data.modelName[0] >= 'A' && data.modelName[0] <= 'G'));
                break;
            case PrimaryFilterType.HtoN:
                filteredModelData.AddRange(modelDataList.FindAll(data => data.modelName[0] >= 'H' && data.modelName[0] <= 'N'));
                break;
            case PrimaryFilterType.OtoZ:
                filteredModelData.AddRange(modelDataList.FindAll(data => data.modelName[0] >= 'O' && data.modelName[0] <= 'Z'));
                break;
        }

        // Apply the secondary filter
        switch (currentSecondaryFilter)
        {
            case SecondaryFilterType.Date:
                filteredModelData.Sort((data1, data2) =>
                {
                    if (DateTime.TryParse(data1.creationDateTime, out DateTime date1) &&
                        DateTime.TryParse(data2.creationDateTime, out DateTime date2))
                    {
                        return currentSortType == SortType.Ascending ? date1.CompareTo(date2) : date2.CompareTo(date1);
                    }
                    else
                    {
                        Debug.LogWarning("Invalid date format in model data.");
                        return 0;
                    }
                });
                break;

            case SecondaryFilterType.Name:
                filteredModelData.Sort((data1, data2) =>
                {
                    return currentSortType == SortType.Ascending ?
                        string.Compare(data1.modelName, data2.modelName, StringComparison.OrdinalIgnoreCase) :
                        string.Compare(data2.modelName, data1.modelName, StringComparison.OrdinalIgnoreCase);
                });
                break;
        }

        return filteredModelData;
    }


    private void StoreUserInfo(int index)
    {
        // ... Existing code ...
        selectedJsonLink = jsonLinks[index];
        Debug.Log("Selected JsonLink: " + selectedJsonLink);
        RoomUI.SetActive(true);

        selectedModelName = modelNames[index];
        selectedModelDate = creationDateTimes[index];
        Debug.Log("Selected Model Name: " + selectedModelName);
        Debug.Log("Selected Model Date: " + selectedModelDate);

        // Clear previous data to avoid duplicates
        modelDataList.Clear();

        for (int i = 0; i < modelNames.Count; i++)
        {
            ModelData modelData = new ModelData();
            modelData.modelName = modelNames[i];
            modelData.creationDateTime = i < creationDateTimes.Count ? creationDateTimes[i] : string.Empty;
            modelData.jsonLink = i < jsonLinks.Count ? jsonLinks[i] : string.Empty;
            modelDataList.Add(modelData);
        }


        // Add the corresponding dates and JSON links
        for (int i = 0; i < modelDataList.Count; i++)
        {
            if (i < creationDateTimes.Count)
            {
                modelDataList[i].creationDateTime = creationDateTimes[i];
            }

            if (i < jsonLinks.Count)
            {
                modelDataList[i].jsonLink = jsonLinks[i];
            }
        }
    }





    private List<string> GetFilteredModelNames()
    {
        List<string> filteredModelNames = new List<string>(modelNames);

        // Apply the primary filter
        switch (currentPrimaryFilter)
        {
            case PrimaryFilterType.Number:
                filteredModelNames = filteredModelNames.FindAll(modelName => char.IsNumber(modelName[0]));
                break;
            case PrimaryFilterType.AtoG:
                filteredModelNames = filteredModelNames.FindAll(modelName => modelName[0] >= 'A' && modelName[0] <= 'G');
                break;
            case PrimaryFilterType.HtoN:
                filteredModelNames = filteredModelNames.FindAll(modelName => modelName[0] >= 'H' && modelName[0] <= 'N');
                break;
            case PrimaryFilterType.OtoZ:
                filteredModelNames = filteredModelNames.FindAll(modelName => modelName[0] >= 'O' && modelName[0] <= 'Z');
                break;
        }

        // Apply the secondary filter
        switch (currentSecondaryFilter)
        {
            case SecondaryFilterType.Date:
                // Sort by date
                List<KeyValuePair<string, DateTime>> modelNameDateTimePairs = new List<KeyValuePair<string, DateTime>>();

                for (int i = 0; i < filteredModelNames.Count; i++)
                {
                    if (DateTime.TryParse(creationDateTimes[i], out DateTime creationDateTime))
                    {
                        modelNameDateTimePairs.Add(new KeyValuePair<string, DateTime>(filteredModelNames[i], creationDateTime));
                    }
                    else
                    {
                        Debug.LogWarning("Invalid date format: " + creationDateTimes[i]);
                    }
                }

                // Sort based on the SortType
                if (currentSortType == SortType.Ascending)
                {
                    modelNameDateTimePairs.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
                }
                else
                {
                    modelNameDateTimePairs.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
                }

                // Extract the sorted model names from the sorted KeyValuePairs
                filteredModelNames = modelNameDateTimePairs.Select(pair => pair.Key).ToList();
                break;

            case SecondaryFilterType.Name:
                // Sort by model name
                // Sort based on the SortType
                if (currentSortType == SortType.Ascending)
                {
                    filteredModelNames.Sort();
                }
                else
                {
                    filteredModelNames.Sort((name1, name2) => name2.CompareTo(name1));
                }
                break;
        }

        return filteredModelNames;
    }






    private void SelectJsonLink(int index)
    {
        
        selectedJsonLink = jsonLinks[index];
        Debug.Log("Selected JsonLink: " + selectedJsonLink);
        RoomUI.SetActive(true);

        selectedModelName = modelNames[index];
        selectedModelDate = creationDateTimes[index];
        Debug.Log("Selected Model Name: " + selectedModelName);
        Debug.Log("Selected Model Date: " + selectedModelDate);
        
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
        currentPrimaryFilter = PrimaryFilterType.All;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithNumber()
    {
        currentPrimaryFilter = PrimaryFilterType.Number;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithAtoG()
    {
        currentPrimaryFilter = PrimaryFilterType.AtoG;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithHtoN()
    {
        currentPrimaryFilter = PrimaryFilterType.HtoN;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsStartingWithOtoZ()
    {
        currentPrimaryFilter = PrimaryFilterType.OtoZ;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsByDate()
    {
        currentSecondaryFilter = SecondaryFilterType.Date;
        currentPage = 0;
        DisplayButtons();
    }

    public void ShowModelsByName()
    {
        currentSecondaryFilter = SecondaryFilterType.Name;
        currentPage = 0;
        DisplayButtons();
    }

    public void SortByAscending()
    {
        currentSortType = SortType.Ascending;
        currentPage = 0;
        DisplayButtons();
    }

    public void SortByDescending()
    {
        currentSortType = SortType.Descending;
        currentPage = 0;
        DisplayButtons();
    }


    private void OnDestroy()
    {
        ClearButtons();
    }



    // Add this method to clear player preferences on application quit
    private void OnApplicationQuit()
    {
        ClearPlayerPrefs();
    }

    // Method to clear player preferences
    private void ClearPlayerPrefs()
    {
        // Clear all player preferences
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
*/


//Latest Script Version

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using System.Globalization;

public class PCAuth : MonoBehaviour
{
    public TMP_InputField Email;    // Changed to TMP_InputField
    public TMP_InputField Password; // Changed to TMP_InputField
    public Button submitButton;

    public Text resultText;

    // Variables to store the data from PHP
    public string fName;

    public List<string> modelNames;
    public List<string> jsonLinks;
    public List<string> creationDateTimes;

    public List<string> XYZLinks;

    public GameObject HeaderFeild;
    public GameObject EmailFeild;
    public GameObject PasswordFeild;
    public GameObject sButton;
    public GameObject ResultText;
    public GameObject Invalid;
    public GameObject RoomUI;

    public GameObject ModelMenu;

    public GameObject waitMsg;

    public GameObject instruct;
    public GameObject JoinRoom;

    public GameObject ButtonPrefab;
    public Transform ButtonParent;

    public GameObject Holder;

    private int currentPage = 0;
    private int buttonsPerPage = 4;
    private List<GameObject> instantiatedButtons = new List<GameObject>();

    public static string selectedJsonLink;

    public static string selectedModelName;
    public static string selectedModelDate;

    public static string firstName;
    public static string userEmail;
    public static string userPassword;


    public Button AllButton;
    public Button NumberButton;
    public Button AtoGButton;
    public Button HtoNButton;
    public Button OtoZButton;

    public Button NextButton;
    public Button PreButton;

    private string phpURL = "https://davidjoiner.net/~confocal/PCuAuth.php";

    private List<int> filteredIndices = new List<int>();

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
        creationDateTimes.Clear();
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
            // Retrieve the data from PlayerPrefs
            form.AddField("Email", userEmail);
            form.AddField("Password", userPassword);

            Debug.Log("!?!? Welcome back to lobby");
            Debug.Log(userEmail);
            Debug.Log(userPassword);
        }


        UnityWebRequest request = UnityWebRequest.Post(phpURL, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Web request error: " + request.error);
        }
        else
        {
            if (!string.IsNullOrEmpty(request.downloadHandler.text)) // Check if the response is not null or empty
            {
                string response = request.downloadHandler.text;
                resultText.text = response;

                string[] lines = response.Split('\n');
                XYZLinks.Clear();

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
                    else if (line.StartsWith("XYZLink: "))
                    {
                        string XYZLink = line.Substring(9);
                        if (!string.IsNullOrEmpty(XYZLink))
                        {
                            // Clear the list before populating it to avoid duplication
                            XYZLinks.Add(XYZLink);
                        }
                        else
                        {
                            Debug.LogWarning("XYZLink value is null or empty in the response.");
                            XYZLinks.Add("null");
                        }
                    }
                    else if (line.StartsWith("CreationDateTime: "))
                    {
                        string creationDateTime = line.Substring(18);
                        creationDateTimes.Add(creationDateTime);
                    }
                }

                if (fName != null && fName != "")
                {
                    // ... Rest of the code
                    HeaderFeild.SetActive(false);
                    EmailFeild.SetActive(false);
                    PasswordFeild.SetActive(false);
                    sButton.SetActive(false);
                    Invalid.SetActive(false);
                    RoomUI.SetActive(false);


                    waitMsg.SetActive(true);

                    yield return new WaitForSeconds(3f);

                    instruct.SetActive(false);

                    waitMsg.SetActive(false);

                    JoinRoom.SetActive(true);

                    ModelMenu.SetActive(true);


                    Holder.SetActive(true);

                    DisplayButtons();

                    // Call StoreUserInfo to store user information in PlayerPrefs
                    StoreUserInfo(0);

                    //selectedJsonLink = "01";
                    //Debug.Log("Selected JsonLink: " + selectedJsonLink);

                    //selectedModelName = "01";
                    //Debug.Log("Selected ModelName: " + selectedModelName);

                    //selectedModelDate = "2023-01-01 05:30:00";
                    //Debug.Log("Selected ModelDate: " + selectedModelDate);
                }
                else
                {
                    Debug.Log("Invalid email or password");
                    Invalid.SetActive(true);
                    RoomUI.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("Empty or null response received.");
            }
        }
    }






    // Define a data structure to hold model information
    public class ModelData
    {
        public string modelName;
        public string jsonLink;
        public DateTime creationDate;

        public ModelData(string name, string link, string date)
        {
            modelName = name;
            jsonLink = link;
            if (DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                creationDate = parsedDate;
            }
            else
            {
                Debug.LogError("Invalid date format: " + date);
            }
        }

        public string GetFormattedDate()
        {
            return creationDate.ToString("MMMM dd, yyyy");
        }
    }




    private void DisplayButtons()
    {
        ClearButtons();
        filteredIndices.Clear();

        // Filter the models based on the current filter
        for (int i = 0; i < modelNames.Count; i++)
        {
            bool modelPassesFilter = false;

            switch (currentFilter)
            {
                case FilterType.All:
                    modelPassesFilter = true;
                    break;
                case FilterType.Number:
                    modelPassesFilter = char.IsNumber(modelNames[i][0]);
                    break;
                case FilterType.AtoG:
                    modelPassesFilter = modelNames[i][0] >= 'A' && modelNames[i][0] <= 'G';
                    break;
                case FilterType.HtoN:
                    modelPassesFilter = modelNames[i][0] >= 'H' && modelNames[i][0] <= 'N';
                    break;
                case FilterType.OtoZ:
                    modelPassesFilter = modelNames[i][0] >= 'O' && modelNames[i][0] <= 'Z';
                    break;
            }

            if (modelPassesFilter)
            {
                filteredIndices.Add(i);
            }
        }

        // Calculate the total number of pages based on the filtered indices count and buttons per page
        int totalPages = (filteredIndices.Count - 1) / buttonsPerPage + 1;

        // Update the current page if it exceeds the total number of pages
        if (currentPage >= totalPages)
        {
            currentPage = totalPages - 1;
        }

        // Calculate the start and end indices based on the current page
        int startIndex = currentPage * buttonsPerPage;
        int endIndex = Mathf.Min(startIndex + buttonsPerPage, filteredIndices.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            // Get the original index from the filtered indices list
            int originalIndex = filteredIndices[i];

            ModelData modelData = new ModelData(modelNames[originalIndex], jsonLinks[originalIndex], creationDateTimes[originalIndex]);

            // Update the button text to show the formatted date
            string buttonText = $"<size=20><color=white>{modelData.modelName}</color></size>\n<size=10><color=red>{modelData.GetFormattedDate()}</color></size>";

            GameObject buttonObj = Instantiate(ButtonPrefab, ButtonParent);
            buttonObj.name = modelData.modelName;
            instantiatedButtons.Add(buttonObj);
            Button button = buttonObj.GetComponent<Button>();
            TMP_Text buttonTextComponent = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonTextComponent.text = buttonText;

            // Set the text overflow mode to truncate
            buttonTextComponent.overflowMode = TextOverflowModes.Truncate;

            // Set the margin for the truncation area (left, right, top, bottom)
            buttonTextComponent.margin = new Vector4(20f, 5f, 20f, 5f); // Adjust the values as needed

            int index = i;
            button.onClick.AddListener(() => SelectJsonLink(index));
        }

        // Set the color of the filter buttons based on the currentFilter
        AllButton.GetComponent<Image>().color = currentFilter == FilterType.All ? Color.blue : Color.white;
        NumberButton.GetComponent<Image>().color = currentFilter == FilterType.Number ? Color.blue : Color.white;
        AtoGButton.GetComponent<Image>().color = currentFilter == FilterType.AtoG ? Color.blue : Color.white;
        HtoNButton.GetComponent<Image>().color = currentFilter == FilterType.HtoN ? Color.blue : Color.white;
        OtoZButton.GetComponent<Image>().color = currentFilter == FilterType.OtoZ ? Color.blue : Color.white;


        // Disable the "Previous Page" button if the current page is the first page
        if (currentPage <= 0)
        {
            PreButton.interactable = false;
        }
        else
        {
            PreButton.interactable = true;
        }

        // Disable the "Next Page" button if the current page is the last page
        if (currentPage >= totalPages - 1)
        {
            NextButton.interactable = false;
        }
        else
        {
            NextButton.interactable = true;
        }
    }





    private List<ModelData> GetFilteredModels()
    {
        List<ModelData> filteredModels = new List<ModelData>();

        switch (currentFilter)
        {
            case FilterType.All:
                filteredModels = GetModelsForFilter(modelNames);
                break;
            case FilterType.Number:
                filteredModels = GetModelsForFilter(modelNames, '0', '9');
                break;
            case FilterType.AtoG:
                filteredModels = GetModelsForFilter(modelNames, 'A', 'G');
                break;
            case FilterType.HtoN:
                filteredModels = GetModelsForFilter(modelNames, 'H', 'N');
                break;
            case FilterType.OtoZ:
                filteredModels = GetModelsForFilter(modelNames, 'O', 'Z');
                break;
        }

        return filteredModels;
    }

    private List<ModelData> GetModelsForFilter(List<string> sourceList)
    {
        List<ModelData> filteredModels = new List<ModelData>();

        for (int i = 0; i < sourceList.Count; i++)
        {
            ModelData modelData = new ModelData(modelNames[i], jsonLinks[i], creationDateTimes[i]);
            filteredModels.Add(modelData);
        }

        return filteredModels;
    }

    private List<ModelData> GetModelsForFilter(List<string> sourceList, char startChar, char endChar)
    {
        List<ModelData> filteredModels = new List<ModelData>();

        for (int i = 0; i < sourceList.Count; i++)
        {
            char firstChar = char.ToUpper(sourceList[i][0]);
            if (firstChar >= startChar && firstChar <= endChar)
            {
                ModelData modelData = new ModelData(modelNames[i], jsonLinks[i], creationDateTimes[i]);
                filteredModels.Add(modelData);
            }
        }

        return filteredModels;
    }






    private void SelectJsonLink(int filteredIndex)
    {
        if (filteredIndex >= 0 && filteredIndex < filteredIndices.Count)
        {
            int originalIndex = filteredIndices[filteredIndex];
            if (originalIndex >= 0 && originalIndex < jsonLinks.Count)
            {
                selectedJsonLink = jsonLinks[originalIndex];
                Debug.Log("Selected JsonLink: " + selectedJsonLink);


                RoomUI.SetActive(true);

                selectedModelName = modelNames[originalIndex];
                Debug.Log("Selected ModelName: " + selectedModelName);

                selectedModelDate = creationDateTimes[originalIndex];
                Debug.Log("Selected ModelDate: " + selectedModelDate);
            }
            else
            {
                Debug.LogError("Invalid index in SelectJsonLink: " + originalIndex);
            }
        }
        else
        {
            Debug.LogError("Invalid filtered index in SelectJsonLink: " + filteredIndex);
        }
    }



    private void StoreUserInfo(int index)
    {
        if (string.IsNullOrEmpty(userEmail) && string.IsNullOrEmpty(userPassword) && string.IsNullOrEmpty(firstName))
        {
            Debug.Log("!!!!!!!!!");

            firstName = fName;
            userEmail = Email.text;
            userPassword = Password.text;

            // Save the data using PlayerPrefs
            PlayerPrefs.SetString("FirstName", firstName);
            PlayerPrefs.SetString("UserEmail", userEmail);
            PlayerPrefs.SetString("UserPassword", userPassword);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("!!!!!User Already logged in");
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

    
    private void OnApplicationQuit()
    {
        ClearPlayerPrefs();
    }

    // Method to clear player preferences
    private void ClearPlayerPrefs()
    {
        // Clear all player preferences
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
