using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiAnim : MonoBehaviourPunCallbacks
{
    public Animator anim;
    public PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = photonView;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagement.staticPlaymode == "multiplay" && PV.IsMine)
        {
            PV.RPC("AnimMove", RpcTarget.All);
        }
    }

    [PunRPC]
    public void AnimMove()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
    }
}
