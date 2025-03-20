using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class ObjectSliderControl : MonoBehaviour
{
    public Slider xSlider; // Assign in Inspector
    public Slider ySlider; // Assign in Inspector
    public Slider zSlider; // Assign in Inspector (Height)

    private Vector3 originalPosition; // Stores the initial position

    public float xMovementRange = 5f; // Max movement for X
    public float yMovementRange = 3f; // Max movement for Y
    public float zMovementRange = 2f; // Max movement for Z (Height)

    void Start()
    {
        originalPosition = transform.position; // Store the original position

        // Ensure sliders update position on change
        if (xSlider) xSlider.onValueChanged.AddListener(UpdatePosition);
        if (ySlider) ySlider.onValueChanged.AddListener(UpdatePosition);
        if (zSlider) zSlider.onValueChanged.AddListener(UpdatePosition);

        // Set sliders to neutral position (0) to avoid resetting position
        if (xSlider) xSlider.value = 0;
        if (ySlider) ySlider.value = 0;
        if (zSlider) zSlider.value = 0;
    }

    void UpdatePosition(float value)
    {
        if (xSlider && ySlider && zSlider)
        {
            // Move relative to the original position with independent movement ranges
            float xOffset = xSlider.value * xMovementRange;
            float yOffset = ySlider.value * yMovementRange;
            float zOffset = zSlider.value * zMovementRange;

            transform.position = new Vector3(
                originalPosition.x + xOffset,
                originalPosition.y + yOffset,
                originalPosition.z + zOffset
            );
        }
    }
}
