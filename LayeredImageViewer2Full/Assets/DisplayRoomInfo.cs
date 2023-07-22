using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayRoomInfo : MonoBehaviour
{
    public TextMeshProUGUI textField; // Reference to the TMP text component

    void Start()
    {
        // Assuming PCAuth.selectedModelName and PCAuth.selectedModelDate are accessible here

        string modelName = PCAuth.selectedModelName;
        string modelDate = PCAuth.selectedModelDate;

        // Format the text using Rich Text tags for size and color
        string formattedText = $"<size=28><color=blue>{modelName}</color></size>\n<size=20><color=blue>{modelDate}</color></size>";

        // Set the text of the TMP text component
        textField.text = formattedText;
    }
}
