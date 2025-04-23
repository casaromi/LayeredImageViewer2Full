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

        // Only let local player control the slider if they're the master
        slider.interactable = PhotonNetwork.IsMasterClient;

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
