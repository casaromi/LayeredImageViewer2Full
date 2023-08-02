using UnityEngine;

public class LoadingWheelSpinner : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the loading wheel continuously
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
