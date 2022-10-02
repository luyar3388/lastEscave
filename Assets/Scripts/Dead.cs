using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour
{
    public GameObject gameManagement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.name == "OVRCameraRig")
        {
            GameManagement gm = gameManagement.GetComponent<GameManagement>();
            gm.Dead();
        }
    }
}
