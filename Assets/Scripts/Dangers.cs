using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dangers : MonoBehaviour
{
    [SerializeField] EnemyAI m_enemy = null; //위험지역으로 달려 갈 적 변수

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_enemy.SetTarget(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_enemy.RemoveTarget();
        }
    }


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
