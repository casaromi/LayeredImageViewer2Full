using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SliderSync : MonoBehaviourPunCallbacks, IPunObservable
{
    public Slider slider;
    private float syncedValue;

    void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        Debug.Log("Am I Master? " + PhotonNetwork.IsMasterClient);

        // Only let local player control the slider if they're the master
        // slider.interactable = PhotonNetwork.IsMasterClient;

        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            syncedValue = value;
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            slider.value = syncedValue; // Keep synced for remote players
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(syncedValue);
        }
        else
        {
            syncedValue = (float)stream.ReceiveNext();
        }
    }
}



//Allow anyone to change slider value
/*
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SliderSync : MonoBehaviourPun
{
    public Slider slider;

    void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        // Make slider always interactable for all clients
        slider.interactable = true;

        // Add listener for when the slider value is changed
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        // Send the updated slider value to all other clients
        photonView.RPC("SyncSliderValue", RpcTarget.OthersBuffered, value);
    }

    [PunRPC]
    void SyncSliderValue(float value)
    {
        // Update the slider value when an RPC is received
        slider.SetValueWithoutNotify(value);
    }
}
*/