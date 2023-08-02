/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    private PhotonView photonView;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    //Private Text;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        //XRRig rig = FindObjectOfType<XRRig>();
        XROrigin rig = FindObjectOfType<XROrigin>();

        headRig = rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");

        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
        }
    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}

*/




/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Unity.XR.CoreUtils;
using TMPro;
using ExitGames.Client.Photon; // Import the correct Hashtable class

public class NetworkPlayer : MonoBehaviourPun
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    public TextMeshPro firstNameTextMesh; // Reference to the 3D text prefab for first name

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    // Start is called before the first frame update
    void Start()
    {
        //XRRig rig = FindObjectOfType<XRRig>();
        XROrigin rig = FindObjectOfType<XROrigin>();

        headRig = rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");

        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }

            // Get the value of the FirstName variable from PlayerPrefs
            string firstName = PlayerPrefs.GetString("FirstName", "");

            // Set the photonView.Owner.NickName to the firstName
            PhotonNetwork.LocalPlayer.NickName = firstName;

            // Update the custom property "FirstName" for the local player
            ExitGames.Client.Photon.Hashtable playerNameProp = new ExitGames.Client.Photon.Hashtable();
            playerNameProp.Add("FirstName", firstName);
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerNameProp);
        }
        else
        {
            // For remote players, get their custom property "FirstName" and update the 3D Text
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("FirstName", out object remoteFirstName))
            {
                firstNameTextMesh.text = (string)remoteFirstName;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
        }
    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}

*/




/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Unity.XR.CoreUtils;
using TMPro;

public class NetworkPlayer : MonoBehaviourPun
{
    public Transform head;

    // Start is called before the first frame update
    void Start()
    {
        XROrigin rig = FindObjectOfType<XROrigin>();

        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
        else
        {
            // Instantiate and set up the 3D text for the username for remote players
            GameObject usernameTextObject = new GameObject("UsernameText");
            TextMeshPro usernameTextMesh = usernameTextObject.AddComponent<TextMeshPro>();
            usernameTextMesh.text = photonView.Owner.NickName;
            usernameTextMesh.fontSize = 0.2f;
            usernameTextObject.transform.SetParent(head); // Set the parent to the player's head
            usernameTextObject.transform.localPosition = Vector3.up * 0.2f; // Adjust the position above the player's head
            usernameTextObject.transform.localRotation = Quaternion.identity; // Ensure no rotation
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(head, Camera.main.transform);
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Unity.XR.CoreUtils;
using TMPro;

public class NetworkPlayer : MonoBehaviourPun
{
    public Transform head;

    // Dictionary to store username text objects associated with photonView.Owner.NickName
    private Dictionary<string, GameObject> usernameTextObjects = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        XROrigin rig = FindObjectOfType<XROrigin>();

        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
        else
        {
            // Check if the username text object already exists for this player
            if (usernameTextObjects.ContainsKey(photonView.Owner.NickName))
            {
                // If it exists, destroy the previous object to avoid duplicates
                Destroy(usernameTextObjects[photonView.Owner.NickName]);
            }

            // Instantiate and set up the 3D text for the username for remote players
            GameObject usernameTextObject = new GameObject("UsernameText");
            TextMeshPro usernameTextMesh = usernameTextObject.AddComponent<TextMeshPro>();
            usernameTextMesh.text = photonView.Owner.NickName;
            usernameTextMesh.fontSize = 0.2f;
            usernameTextObject.transform.SetParent(head); // Set the parent to the player's head
            usernameTextObject.transform.localPosition = Vector3.up * 0.2f; // Adjust the position above the player's head
            usernameTextObject.transform.localRotation = Quaternion.identity; // Ensure no rotation

            // Add the username text object to the dictionary
            usernameTextObjects.Add(photonView.Owner.NickName, usernameTextObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(head, Camera.main.transform);
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
