using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool doorState;
    public bool doorSound;
    public bool comState;

    // Start is called before the first frame update
    void Start()
    {
        doorState = false;
        doorSound = false;
        comState = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (doorState && comState)
        {
            Complete();
        }
    }

    public void Complete()
    {
            this.transform.Translate(Vector3.up * Time.deltaTime);
            
            if (doorSound == false)
            {
                this.GetComponent<AudioSource>().Play();
                doorSound = true;
            }
            if (this.transform.position.y > 500)
            {
                this.gameObject.SetActive(false);
            }      
    }

    public void ComTrigger()
    {
        comState = true;
    }
    void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "PlayerMine"))
        {
            doorState = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "PlayerMine"))
        {
            doorState = false;
        }
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if ((other.tag == "Player" || other.tag == "PlayerMine") && Input.GetButtonDown("Interaction"))
    //    {
    //        GameManagement.staticDoorTrigger = true;
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player" || other.tag == "PlayerMine")
    //    {
    //        GameManagement.staticDoorTrigger = false;
    //    }
    //}
}
