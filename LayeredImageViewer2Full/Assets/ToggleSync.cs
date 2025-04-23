using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSync : MonoBehaviourPun
{
    public Toggle toggle;

    void Start()
    {
        if (toggle == null)
            toggle = GetComponent<Toggle>();

        // Always interactable
        toggle.interactable = true;

        // Add listener to detect state changes
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void OnToggleValueChanged(bool isOn)
    {
        // Broadcast to others
        photonView.RPC("SyncToggleState", RpcTarget.OthersBuffered, isOn);
    }

    [PunRPC]
    void SyncToggleState(bool isOn)
    {
        // Update toggle state without triggering OnValueChanged again
        toggle.SetIsOnWithoutNotify(isOn);
    }
}
