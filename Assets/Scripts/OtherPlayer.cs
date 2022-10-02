using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OtherPlayer : MonoBehaviourPunCallbacks, IPunObservable
{

    // Update is called once per frame
    void Update()
    {
        RPCUpdate();
    }

    public void RPCUpdate()
    {

        if (!gameObject.GetComponent<PhotonView>().IsMine)
        {
            if (gameObject.GetComponent<PhotonView>().ViewID == 1001)
            {
                gameObject.transform.position = MultiplayManager.Player1Position;
                gameObject.transform.rotation = MultiplayManager.Player1Rotation;
            }
            else
            {
                gameObject.transform.position = MultiplayManager.Player2Position;
                gameObject.transform.rotation = MultiplayManager.Player2Rotation;
            }
        }
        else
        {
            if (gameObject.GetComponent<PhotonView>().ViewID == 1001)
            {
                MultiplayManager.Player1Position = gameObject.transform.parent.transform.position;
                MultiplayManager.Player1Rotation = gameObject.transform.rotation;
            }
            else
            {
                MultiplayManager.Player2Position = gameObject.transform.parent.transform.position;
                MultiplayManager.Player2Rotation = gameObject.transform.rotation;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(MultiplayManager.Player1Position);
            stream.SendNext(MultiplayManager.Player1Rotation);
            stream.SendNext(MultiplayManager.Player2Position);
            stream.SendNext(MultiplayManager.Player2Rotation);
        }
        else
        {
            // Network player, receive data
            MultiplayManager.Player1Position = (Vector3)stream.ReceiveNext();
            MultiplayManager.Player1Rotation = (Quaternion)stream.ReceiveNext();
            MultiplayManager.Player2Position = (Vector3)stream.ReceiveNext();
            MultiplayManager.Player2Rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
