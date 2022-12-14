using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PuzzleDirector : MonoBehaviourPunCallbacks
{
    int fireCount;
    public int fireNumber;
    GameObject door;
    public PhotonView PV;
    public bool doorSound = false;
    public bool doorState;
    GameObject DoorTrigger;


    // Start is called before the first frame update
    void Start()
    {
        fireCount = 0;
        fireNumber = 10;
        //door = GameObject.Find("door");

        PV = photonView;
        doorState = false;
        DoorTrigger = GameObject.Find("door");
    }

    // Update is called once per frame
    void Update()
    {
        if (fireCount == fireNumber)
        {
            if(GameManagement.staticPlaymode == "soloplay" || GameManagement.staticPlaymode == null)
            {
                DoorTrigger.GetComponent<DoorTrigger>().ComTrigger();
            }
            else if(GameManagement.staticPlaymode == "multiplay")
            {
                PV.RPC("Complete", RpcTarget.All);
            }
        }
    }

    public void Increase()
    {
        fireCount++;
        Debug.Log(fireCount);
    }

    public void Decrease()
    {
        fireCount--;
        Debug.Log(fireCount);
    }

    //[PunRPC]
    //void Complete()
    //{

    //    door.transform.Translate(Vector3.up * Time.deltaTime);
        
    //    if(doorSound == false)
    //    {
    //        door.GetComponent<AudioSource>().Play();
    //        doorSound = true;
    //    }
    //    if (door.transform.position.y > 500)
    //    {
    //        door.SetActive(false);
    //    }
    //}

    //void OnTriggerStay(Collider other)
    //{
    //    if ((other.tag == "Player" || other.tag == "PlayerMine"))
    //    {
    //        doorState = true;
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if ((other.tag == "Player" || other.tag == "PlayerMine"))
    //    {
    //        doorState = false;
    //    }
    //}
}
