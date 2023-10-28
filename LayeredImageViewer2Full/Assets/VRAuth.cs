using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using System.Globalization;

public class VRAuth : MonoBehaviour
{
    public TMP_InputField AuthCodeInput;    // Changed to TMP_InputField
    public Button submitButton;

    public Text resultText;

    // Variables to store the data from PHP
    public string fName;

    public List<string> modelNames;
    public List<string> jsonLinks;
    public List<string> creationDateTimes;

    public List<string> XYZLinks;

    public GameObject HeaderFeild;
    public GameObject AuthFeild;
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

    public static string selectedXYZLink;

    public static string firstName;
    public static string userAuthCode;


    public Button AllButton;
    public Button NumberButton;
    public Button AtoGButton;
    public Button HtoNButton;
    public Button OtoZButton;

    public Button NextButton;
    public Button PreButton;

    private string phpURL = "https://davidjoiner.net/~confocal/uAuth.php";

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
        if (string.IsNullOrEmpty(userAuthCode))
        {
            form.AddField("AuthCode", AuthCodeInput.text);

            Debug.Log("!!!NEW USER!!!");
        }
        else
        {
            // Retrieve the data from PlayerPrefs
            form.AddField("AuthCode", AuthCodeInput.text);

            Debug.Log("!?!? Welcome back to lobby");
            Debug.Log(userAuthCode);
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
                    AuthFeild.SetActive(false);
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
        public string xyzLink;
        public DateTime creationDate;

        public ModelData(string name, string link, string xyz, string date)
        {
            modelName = name;
            jsonLink = link;
            xyzLink = xyz;
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

            ModelData modelData = new ModelData(modelNames[originalIndex], jsonLinks[originalIndex], XYZLinks[originalIndex], creationDateTimes[originalIndex]);

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
            ModelData modelData = new ModelData(modelNames[i], jsonLinks[i], XYZLinks[i], creationDateTimes[i]);
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
                ModelData modelData = new ModelData(modelNames[i], jsonLinks[i], XYZLinks[i], creationDateTimes[i]);
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

                selectedXYZLink = XYZLinks[originalIndex];
                Debug.Log("Selected XYZLink: " + selectedXYZLink);
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
        if (string.IsNullOrEmpty(userAuthCode) && string.IsNullOrEmpty(firstName))
        {
            Debug.Log("!!!!!!!!!");

            firstName = fName;
            userAuthCode = AuthCodeInput.text;
            

            // Save the data using PlayerPrefs
            PlayerPrefs.SetString("FirstName", firstName);
            PlayerPrefs.SetString("userAuthCode", userAuthCode);
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
