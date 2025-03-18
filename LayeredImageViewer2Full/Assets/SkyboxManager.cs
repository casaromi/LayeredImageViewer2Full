/*
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{

    public Material morningSkybox;
    public Material nightSkybox;s

    private void Start()
    {
        // Call the UpdateSkybox method once initially
        UpdateSkybox();
    }

    private void Update()
    {
        // Check the current system time and update the skybox accordingly
        UpdateSkybox();
    }

    private void UpdateSkybox()
    {
        // Get the current system time
        float currentHour = System.DateTime.Now.Hour;

        // Determine which skybox material to use based on the time of day
        Material skyboxMaterial;
        if (currentHour >= 9 && currentHour < 18)
        {
            skyboxMaterial = morningSkybox;
        }
        else
        {
            skyboxMaterial = nightSkybox;
        }

        // Update the skybox material
        RenderSettings.skybox = skyboxMaterial;
    }
}
*/

using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class SkyboxManager : MonoBehaviour
{
    public Material morningSkybox;
    public Material nightSkybox;
    public GameObject environment;
    public GameObject bounds;
    public bool enableSkybox = true; // Default ON
    public Toggle skyboxToggle; // UI Toggle reference (Optional)

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; // Get the main camera

        if (skyboxToggle != null)
        {
            skyboxToggle.isOn = enableSkybox;
            skyboxToggle.onValueChanged.AddListener(ToggleSkybox);
        }

        UpdateSkybox();
    }

    private void ToggleSkybox(bool isOn)
    {
        enableSkybox = isOn;
        UpdateSkybox();
    }

    private void UpdateSkybox()
    {
        if (!enableSkybox)
        {
            RenderSettings.skybox = null; // Remove skybox
            if (mainCamera != null)
                mainCamera.clearFlags = CameraClearFlags.SolidColor; // Set background to solid color

            if (environment != null)
                environment.SetActive(false); // Hide environment objects
                bounds.SetActive(true);// enable bounds
            return;
        }

        if (mainCamera != null)
            mainCamera.clearFlags = CameraClearFlags.Skybox; // Enable skybox

        if (environment != null)
            environment.SetActive(true); // Show environment objects
            bounds.SetActive(false);// remove bounds

        float currentHour = System.DateTime.Now.Hour;
        RenderSettings.skybox = (currentHour >= 9 && currentHour < 18) ? morningSkybox : nightSkybox;
    }
}


