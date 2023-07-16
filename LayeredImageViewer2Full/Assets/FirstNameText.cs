using UnityEngine;
using TMPro;

public class FirstNameText : MonoBehaviour
{
    public TextMeshProUGUI textMesh; // Reference to the TextMeshProUGUI component

    public string variableToUpdate; // Variable to update the text with

    private void Start()
    {
        // Make sure the TextMeshProUGUI component is assigned
        if (textMesh == null)
        {
            Debug.LogError("TextMeshProUGUI component is not assigned!");
        }
    }

    private void Update()
    {
        // Update the text with the value of the variable
        textMesh.text = variableToUpdate.ToString();
    }
}
