using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;

public class SyncObject : MonoBehaviour, IPunObservable
{
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // If you are the master client, send the data to others.
            stream.SendNext(transform.position);
            // Send other necessary data.
        }
        else
        {
            // If you are a non-master client, receive the data.
            transform.position = (Vector3)stream.ReceiveNext();
            // Receive other necessary data.
        }
    }
}
