using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour
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
        if (other.tag == "Player" || other.tag == "PlayerMine")
        {
            GameManagement gm = gameManagement.GetComponent<GameManagement>();
            gm.GameClear();
        }
    }
}
