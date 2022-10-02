using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PN = Photon.Pun.PhotonNetwork;
using PV = Photon.Pun.PhotonView;

public class MapCamController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public Transform cam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PN.LocalPlayer.NickName == GameManagement.staticPlayerName || GameManagement.staticPlaymode == "soloplay")
        {
            this.transform.position = cam.position;
            this.transform.rotation = cam.rotation;
        }
    }
}
