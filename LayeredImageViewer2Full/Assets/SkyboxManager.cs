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
