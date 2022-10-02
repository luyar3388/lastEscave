using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.forward;
        Vector3 direction = Quaternion.AngleAxis(-30, Vector3.up) * dir;
        Debug.Log(direction);
    }
}
