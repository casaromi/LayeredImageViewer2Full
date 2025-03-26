using UnityEngine;
using UnityEngine.UI;

public class PointCloudToggleController : MonoBehaviour
{
    public Toggle pointCloudToggle;
    public GameObject pointCloudParent;

    void Start()
    {
        if (pointCloudToggle != null && pointCloudParent != null)
        {
            pointCloudToggle.onValueChanged.AddListener(OnToggleChanged);
            pointCloudToggle.isOn = pointCloudParent.activeSelf; // Set initial state
        }
    }

    void OnToggleChanged(bool isOn)
    {
        if (pointCloudParent != null)
        {
            pointCloudParent.SetActive(isOn);
        }
    }
}
