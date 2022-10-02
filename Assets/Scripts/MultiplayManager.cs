using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayManager : MonoBehaviourPunCallbacks
{
    public GameObject multiplayManager;
    public GameObject playerPrefab;

    [PunRPC]
    public static Vector3 Player1Position;
    [PunRPC]
    public static Quaternion Player1Rotation;
    [PunRPC]
    public static Vector3 Player2Position;
    [PunRPC]
    public static Quaternion Player2Rotation;


    private void Awake()
    {
        Player1Position = Vector3.zero;
        Player1Rotation = Quaternion.identity;
        Player2Position = Vector3.zero;
        Player2Rotation = Quaternion.identity;
    }
    void Start()
    {
        //PN.IsMessageQueueRunning = true;

        if (GameManagement.staticPlaymode == "soloplay" || GameManagement.staticPlaymode == null)
        {
            GameObject go;
            go = Instantiate(playerPrefab) as GameObject;
            go.transform.position = GameObject.Find("OVRCameraRig").transform.position + (Vector3.down * 7f); //+ Vector3.back*50f;
            go.transform.parent = GameObject.Find("OVRCameraRig").transform;
            go.tag = "Player";
            go.transform.parent.gameObject.AddComponent<MultiPlayer>();
            go.layer = 10;
            Destroy(multiplayManager);
        }
        else if(GameManagement.staticPlaymode == "multiplay")
        {
            
            CreateMultiPlayer();
        }
    }
    void CreateMultiPlayer()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        GameObject go = PhotonNetwork.Instantiate("Originalprefab", points[idx].position, Quaternion.identity);
        go.tag = "Player";
        go.AddComponent<OtherPlayer>();
        go.GetComponent<PhotonView>().ObservedComponents.Add(go.GetComponent<OtherPlayer>());
        if (go.GetComponent<PhotonView>().IsMine)
        {
            go.transform.position = GameObject.Find("OVRCameraRig").transform.position + (Vector3.down * 7f);
            go.transform.parent = GameObject.Find("OVRCameraRig").transform;
            go.transform.parent.gameObject.AddComponent<MultiPlayer>();
            go.layer = 10;
        }
    }

    //void CreateMultiPlayer()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        GameObject go = PhotonNetwork.Instantiate("Originalprefab", new Vector3(0, 0, 0), Quaternion.identity);
    //        go.tag = "Player";
    //        go.AddComponent<OtherPlayer>();
    //        go.GetComponent<PhotonView>().ObservedComponents.Add(go.GetComponent<OtherPlayer>());
    //        if (go.GetComponent<PhotonView>().IsMine)
    //        {
    //            go.transform.position = GameObject.Find("OVRCameraRig").transform.position + (Vector3.down * 7f);
    //            go.transform.parent = GameObject.Find("OVRCameraRig").transform;
    //            go.transform.parent.gameObject.AddComponent<MultiPlayer>();
    //            go.layer = 10;
    //        }
    //    }
    //    else
    //    {
    //        GameObject go = PhotonNetwork.Instantiate("Originalprefab", new Vector3(0, 0, 0), Quaternion.identity);
    //        go.tag = "Player";
    //        go.AddComponent<OtherPlayer>();
    //        go.GetComponent<PhotonView>().ObservedComponents.Add(go.GetComponent<OtherPlayer>());
    //        if (go.GetComponent<PhotonView>().IsMine)
    //        {
    //            go.transform.position = GameObject.Find("OVRCameraRig").transform.position + (Vector3.down * 7f);
    //            go.transform.parent = GameObject.Find("OVRCameraRig").transform;
    //            go.transform.parent.gameObject.AddComponent<MultiPlayer>();
    //            go.layer = 10;
    //        }
    //    }
    //}
}
