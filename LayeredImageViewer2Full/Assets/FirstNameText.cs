using UnityEngine;
using TMPro; // Ensure you have TextMeshPro installed (if not, you can import it through Unity's Package Manager)

public class FirstNameText : MonoBehaviour
{
    // Reference to the 3D Text object
    public TextMeshPro firstNameTextMesh;

    void Start()
    {
        // Get the value of the FirstName variable from PlayerPrefs
        string firstName = PlayerPrefs.GetString("FirstName", "");

        // Assign the value to the 3D Text component
        firstNameTextMesh.text = firstName;
    }
}
