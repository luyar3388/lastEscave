using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PN = Photon.Pun.PhotonNetwork;
using PV = Photon.Pun.PhotonView;

public class CamController : MonoBehaviourPunCallbacks
{
    public Transform cam;
    public GameObject player;
    string playername;
    // Start is called before the first frame update

    void Start()
    {

        cam = gameObject.transform;
        player = gameObject.transform.parent.gameObject;
        //멀티플레이일때만 적용
        //자기 카메라 아니면 다 부숨
        if(GameManagement.staticPlaymode == "multiplay")
        {
            playername = player.GetComponent<MultiPlayer>().photonView.Owner.NickName;
            if (!player.GetComponent<MultiPlayer>().photonView.IsMine)
            {
                Destroy(gameObject);
            }
        }

    }

    // Update is called once per frame
    
    void Update()
    {
        //카메라가 플레이어 움직임 따라 transform이 변함
        if (playername == GameManagement.staticPlayerName || GameManagement.staticPlaymode == "soloplay")
        {
            cam.position = new Vector3(player.transform.position.x, player.transform.position.y+3.5f, player.transform.position.z);
            cam.transform.localEulerAngles = new Vector3(player.GetComponent<MultiPlayer>().currentCameraRotationX, player.transform.rotation.y, 0);
        }
    }
}
