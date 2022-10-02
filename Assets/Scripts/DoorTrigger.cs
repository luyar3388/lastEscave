using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "PlayerMine") && Input.GetButtonDown("Interaction"))
        {
            GameManagement.staticDoorTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PlayerMine")
        {
            GameManagement.staticDoorTrigger = false;
        }
    }
}
