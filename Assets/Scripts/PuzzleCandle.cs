//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;

//public class PuzzleCandle : MonoBehaviourPunCallbacks
//{
//    GameObject fire;
//    GameObject pairObject;
//    GameObject puzzleDirector;
//    public bool fire_state;

//    public PhotonView PV;
//    GameObject candleTutorial;

//    // Start is called before the first frame update
//    void Start()
//    {
//        fire = transform.GetChild(6).gameObject;
//        pairObject = transform.GetChild(7).gameObject;
//        puzzleDirector = GameObject.Find("PuzzleDirector");

//        PV = photonView;
//        candleTutorial = GameObject.Find("CandleTutorial");
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Puzzle1();
//    }

//    public void Puzzle1()
//    {
//        if (Input.GetButtonDown("Interaction") && GameManagement.staticGetLighter == true && fire_state && (GameManagement.staticPlaymode == "soloplay" || GameManagement.staticPlaymode == null))
//        {
//            if(candleTutorial.activeSelf == true)
//            {
//                candleTutorial.SetActive(false);
//            }
//            PuzzleControl();
//        }
//        else if (Input.GetButtonDown("Interaction") && GameManagement.staticGetLighter == true && fire_state && GameManagement.staticPlaymode == "multiplay")
//        {
//            if (candleTutorial.activeSelf == true)
//            {
//                candleTutorial.SetActive(false);
//            }
//            PV.RPC("PuzzleControl", RpcTarget.All);
//        }
//    }

//    [PunRPC]
//    public void PuzzleControl()
//    {
//        if (fire.activeSelf == true)
//        {
//            fire.SetActive(false);
//            pairObject.GetComponent<Light>().enabled = false;
//            puzzleDirector.GetComponent<PuzzleDirector>().Decrease();
//        }
//        else if (fire.activeSelf == false)
//        {
//            fire.SetActive(true);
//            pairObject.GetComponent<Light>().enabled = true;
//            puzzleDirector.GetComponent<PuzzleDirector>().Increase();
//        }
//    }


//    void OnTriggerStay(Collider other)
//    {
//        if (other.tag == "Player" || other.tag == "PlayerMine")
//        {
//            fire_state = true;
//        }
//    }

//    void OnTriggerExit(Collider other)
//    {
//        if (other.tag == "Player" || other.tag == "PlayerMine")
//        {
//            fire_state = false;
//        }
//    }
//}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PuzzleCandle : MonoBehaviourPunCallbacks
{
    GameObject fire;
    GameObject pairObject;
    GameObject puzzleDirector;
    public bool fire_state;

    public PhotonView PV;
    GameObject candleTutorial;

    // Start is called before the first frame update
    void Start()
    {
        fire = transform.GetChild(6).gameObject;
        pairObject = transform.GetChild(7).gameObject;
        puzzleDirector = GameObject.Find("PuzzleDirector");

        PV = photonView;
        candleTutorial = GameObject.Find("CandleTutorial");
    }

    // Update is called once per frame
    void Update()
    {
        Puzzle1();
    }

    public void Puzzle1()
    {
        if (Input.GetButtonDown("Interaction") && GameManagement.staticGetLighter && fire_state && GameManagement.staticTurnOnLighter && (GameManagement.staticPlaymode == "soloplay" || GameManagement.staticPlaymode == null))
        {
            Destroy(candleTutorial);
            PuzzleControl();
        }
        else if (Input.GetButtonDown("Interaction") && GameManagement.staticGetLighter && fire_state && GameManagement.staticTurnOnLighter && GameManagement.staticPlaymode == "multiplay")
        {
            Destroy(candleTutorial);
            PV.RPC("MultiPuzzleControl", RpcTarget.All, this.gameObject.transform.position);
        }
    }

    public void PuzzleControl()
    {
        if (fire.GetComponent<ParticleSystem>().isPlaying == true)
        {
            fire.GetComponent<ParticleSystem>().Stop();
            pairObject.GetComponent<Light>().enabled = false;
            puzzleDirector.GetComponent<PuzzleDirector>().Decrease();
        }
        if (fire.GetComponent<ParticleSystem>().isPlaying == false)
        {
            fire.GetComponent<ParticleSystem>().Play();
            pairObject.GetComponent<Light>().enabled = true;
            puzzleDirector.GetComponent<PuzzleDirector>().Increase();
        }
    }

    [PunRPC]
    public void MultiPuzzleControl(Vector3 pos)
    {
        PhotonNetwork.Instantiate("FireParticle", pos, Quaternion.identity);
        pairObject.GetComponent<Light>().enabled = true;
        puzzleDirector.GetComponent<PuzzleDirector>().Increase();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PlayerMine")
        {
            fire_state = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PlayerMine")
        {
            fire_state = false;
        }
    }
}










//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;

//public class PuzzleCandle : MonoBehaviourPunCallbacks
//{
//    GameObject fire;
//    GameObject pairObject;
//    GameObject puzzleDirector;
//    public bool fire_state;

//    // Start is called before the first frame update
//    void Start()
//    {
//        fire = transform.GetChild(6).gameObject;
//        pairObject = transform.GetChild(7).gameObject;
//        puzzleDirector = GameObject.Find("PuzzleDirector");
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Puzzle1();
//    }

//    public void Puzzle1()
//    {
//        if (Input.GetButtonDown("Interaction") && GameManagement.staticGetLighter == true)
//        {
//            PuzzleControl();
//        }
//    }

//    public void PuzzleControl()
//    {
//        if (fire.GetComponent<ParticleSystem>().isPlaying == true && fire_state)
//        {
//            fire.GetComponent<ParticleSystem>().Stop();
//            pairObject.GetComponent<Light>().enabled = false;
//            puzzleDirector.GetComponent<PuzzleDirector>().Decrease();
//        }
//        else if (fire.GetComponent<ParticleSystem>().isPlaying == false && fire_state)
//        {
//            fire.GetComponent<ParticleSystem>().Play();
//            pairObject.GetComponent<Light>().enabled = true;
//            puzzleDirector.GetComponent<PuzzleDirector>().Increase();
//        }
//    }

//    void OnTriggerStay(Collider other)
//    {
//        if (other.tag == "Player")
//        {
//            fire_state = true;
//        }
//    }

//    void OnTriggerExit(Collider other)
//    {
//        if (other.tag == "Player")
//        {
//            fire_state = false;
//        }
//    }
//}