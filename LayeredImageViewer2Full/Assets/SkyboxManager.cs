/*
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{

    public Material morningSkybox;
    public Material nightSkybox;

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
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for raycaster fix

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
        mainCamera = Camera.main;

        if (skyboxToggle != null)
        {
            // Set initial toggle state
            skyboxToggle.isOn = enableSkybox;
            skyboxToggle.onValueChanged.AddListener(ToggleSkybox);
        }

        UpdateSkybox();
    }

    private void ToggleSkybox(bool isOn)
    {
        enableSkybox = isOn;
        UpdateSkybox();

        // **Fix VR raycaster sticking issue**
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void UpdateSkybox()
    {
        if (!enableSkybox)
        {
            RenderSettings.skybox = null;
            if (mainCamera != null)
                mainCamera.clearFlags = CameraClearFlags.SolidColor;

            if (environment != null)
                environment.SetActive(false);

            if (bounds != null)
                bounds.SetActive(true); // Enable bounds

            return;
        }

        if (mainCamera != null)
            mainCamera.clearFlags = CameraClearFlags.Skybox;

        if (environment != null)
            environment.SetActive(true);

        if (bounds != null)
            bounds.SetActive(false); // Disable bounds

        float currentHour = System.DateTime.Now.Hour;
        RenderSettings.skybox = (currentHour >= 9 && currentHour < 18) ? morningSkybox : nightSkybox;
    }
}

