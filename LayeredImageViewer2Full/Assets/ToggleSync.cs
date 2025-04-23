using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSync : MonoBehaviourPun
{
    public Toggle toggle;

    void Start()
    {
        if (toggle == null)
        {
            toggle = GetComponent<Toggle>();
        }

        toggle.interactable = true;

        // Listen for toggle changes locally
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void OnToggleValueChanged(bool isOn)
    {
        Debug.Log("Local Toggle changed to: " + isOn);

        // Send toggle state to all other clients
        photonView.RPC("SyncToggleState", RpcTarget.OthersBuffered, isOn);
    }

    [PunRPC]
    void SyncToggleState(bool isOn)
    {
        Debug.Log("Received toggle sync RPC: " + isOn);

        // Update toggle state without triggering another OnValueChanged
        toggle.SetIsOnWithoutNotify(isOn);
    }
}
