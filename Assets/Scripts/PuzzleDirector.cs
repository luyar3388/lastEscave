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


    // Start is called before the first frame update
    void Start()
    {
        fireCount = 0;
        fireNumber = 10;
        door = GameObject.Find("door");

        PV = photonView;

    }

    // Update is called once per frame
    void Update()
    {
        if (fireCount == fireNumber && GameManagement.staticDoorTrigger)
        {
            if(GameManagement.staticPlaymode == "soloplay" || GameManagement.staticPlaymode == null)
            {
                Complete();
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
    void Complete()
    {
        door.transform.Translate(Vector3.up * Time.deltaTime);
        if (door.transform.position.y > 500)
        {
            door.SetActive(false);
        }
    }
}
