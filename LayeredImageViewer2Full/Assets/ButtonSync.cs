using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSync : MonoBehaviourPun
{
    public Button button;

    void Start()
    {
        if (button == null)
            button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        Debug.Log("Button clicked locally");
        photonView.RPC("ExecuteButtonAction", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ExecuteButtonAction()
    {
        Debug.Log("Executing button action on: " + PhotonNetwork.NickName);

        // Call your actual effect here
        // e.g., turn on light, play sound, show UI panel, etc.
        DoTheThing();
    }

    void DoTheThing()
    {
        // Example: just logging for now
        Debug.Log("Button action executed!");
    }
}
