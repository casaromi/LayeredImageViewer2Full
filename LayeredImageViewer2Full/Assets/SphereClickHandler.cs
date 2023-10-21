using UnityEngine;
using UnityEngine.UI;

public class SphereClickHandler : MonoBehaviour
{
    private Canvas popupCanvas;
    private Text popupText;
    private Renderer sphereRenderer;
    private Color originalColor;
    private Color highlightColor = Color.yellow;

    public void Initialize(Canvas canvas, Vector3 spherePosition)
    {
        popupCanvas = canvas;
        popupText = canvas.GetComponentInChildren<Text>();

        // Set up the text properties
        popupText.text = $"Coordinates: {spherePosition.x}, {spherePosition.y}, {spherePosition.z}";
        popupText.alignment = TextAnchor.MiddleCenter;
        popupText.fontSize = 20;
        popupText.color = Color.black;

        // Initially, hide the popup
        popupCanvas.gameObject.SetActive(false);

        // Get the sphere renderer and store its original color
        sphereRenderer = GetComponent<Renderer>();
        originalColor = sphereRenderer.material.color;
    }

    void OnMouseDown()
    {
        // Show the popup when the sphere is clicked
        ShowPopup();

        // Print debug log statement with sphere coordinates
        PrintDebugLog();
    }

    void OnMouseUp()
    {
        // Hide the popup when the mouse is released
        HidePopup();
    }

    void OnMouseEnter()
    {
        // Highlight the sphere when the mouse enters
        sphereRenderer.material.color = highlightColor;
        // Show the popup when hovering
        ShowPopup();
    }

    void OnMouseExit()
    {
        // Reset the color when the mouse exits
        sphereRenderer.material.color = originalColor;
        // Hide the popup when not hovering
        HidePopup();
    }

    void ShowPopup()
    {
        popupCanvas.gameObject.SetActive(true);
    }

    void HidePopup()
    {
        popupCanvas.gameObject.SetActive(false);
    }

    void PrintDebugLog()
    {
        // Print debug log statement with sphere coordinates
        Vector3 spherePosition = transform.position;
        Debug.Log($"Sphere clicked at Coordinates: {spherePosition.x}, {spherePosition.y}, {spherePosition.z}");
    }
}
