using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObjectSliderControl : MonoBehaviour
{
    // Sliders to assign in Inspector
    public Slider xSlider;
    public Slider ySlider;
    public Slider zSlider;
    public Slider scaleSlider;
    public Slider colorSlider;

    public float xMovementRange = 5f;
    public float yMovementRange = 3f;
    public float zMovementRange = 2f;
    public float maxScale = 100f;

    private Vector3 originalPosition;
    private List<Transform> childSpheres = new List<Transform>();
    private List<Vector3> originalScales = new List<Vector3>();
    private List<Renderer> renderers = new List<Renderer>();

    void Start()
    {
        originalPosition = transform.position;

        // Set up slider listeners
        if (xSlider) xSlider.onValueChanged.AddListener(UpdatePosition);
        if (ySlider) ySlider.onValueChanged.AddListener(UpdatePosition);
        if (zSlider) zSlider.onValueChanged.AddListener(UpdatePosition);
        if (scaleSlider) scaleSlider.onValueChanged.AddListener(UpdateScale);
        if (colorSlider) colorSlider.onValueChanged.AddListener(UpdateColor);

        // Set initial values
        if (xSlider) xSlider.value = 0;
        if (ySlider) ySlider.value = 0;
        if (zSlider) zSlider.value = 0;
        if (scaleSlider) scaleSlider.value = 0;
        if (colorSlider) colorSlider.value = 0;
    }

    // ?? Call this method after spawning your spheres!
    public void RefreshSpheres()
    {
        Debug.Log("Refreshing spheres under: " + gameObject.name);

        childSpheres.Clear();
        originalScales.Clear();
        renderers.Clear();

        foreach (Transform child in transform)
        {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend != null)
            {
                childSpheres.Add(child);
                originalScales.Add(child.localScale);

                // Create a unique material instance per object to control color
                Material newMat = new Material(rend.material);
                rend.material = newMat;
                renderers.Add(rend);
            }
        }

        Debug.Log("Found " + childSpheres.Count + " child spheres.");
    }

    void UpdatePosition(float _)
    {
        if (xSlider && ySlider && zSlider)
        {
            float xOffset = xSlider.value * xMovementRange;
            float yOffset = ySlider.value * yMovementRange;
            float zOffset = zSlider.value * zMovementRange;

            transform.position = originalPosition + new Vector3(xOffset, yOffset, zOffset);
        }
    }

    void UpdateScale(float value)
    {
        float scale = Mathf.Lerp(1f, maxScale, value);

        for (int i = 0; i < childSpheres.Count; i++)
        {
            childSpheres[i].localScale = originalScales[i] * scale;
        }
    }

    void UpdateColor(float value)
    {
        Color color = Color.Lerp(Color.white, Color.black, value);

        foreach (Renderer rend in renderers)
        {
            rend.material.color = color;
        }
    }
}
