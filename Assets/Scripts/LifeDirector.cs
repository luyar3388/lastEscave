using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeDirector : MonoBehaviour
{
    GameObject LifePoint;
    int count;

    // Start is called before the first frame update
    void Start()
    {
        LifePoint = GameObject.Find("LifePoint");
        count = 2;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void lifeControl()
    {
        if (count > 0)
        {
            LifePoint.transform.GetChild(count).gameObject.SetActive(false);
            count--;
        }
        else if(count == 0){
            SceneManager.LoadScene("Die");
        }
        
    }
}