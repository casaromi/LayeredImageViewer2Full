using UnityEngine;
using TMPro; // Ensure you have TextMeshPro installed (if not, you can import it through Unity's Package Manager)
using Photon.Pun;

public class NetworkPlayer : MonoBehaviourPun
{
    public Transform head;
    public TextMeshPro firstNameTextMesh;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            // Disable the rendering of the player's model for remote players
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
        else
        {
            // Get the value of the FirstName variable from PlayerPrefs
            string firstName = PlayerPrefs.GetString("FirstName", "");

            // Assign the value to the TextMeshPro component
            firstNameTextMesh.text = firstName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            // Update the position and rotation of the local player's name text
            MapPosition(firstNameTextMesh.transform, head);
        }
    }

    void MapPosition(Transform target, Transform source)
    {
        target.position = source.position + Vector3.up * 2.0f; // You may need to adjust the height offset here
        target.rotation = Quaternion.LookRotation(target.position - Camera.main.transform.position);
    }
}
