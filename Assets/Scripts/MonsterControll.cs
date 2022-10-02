using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterControll : MonoBehaviour
{
    public enum CurrentState { Idle, Walk, Run, attack, Death};
    public CurrentState curState = CurrentState.Idle;
    

    private Transform _transform;
    public GameObject player;
    private NavMeshAgent nvAgent;
    Animator anim;
    int HP = 100;
    
    
    

    public float traceDist = 15.0f; //추적을 시작하는 거리
    public float attackDist = 3.2f; // 공격하는 거리
    private bool isDead = false;

    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>(); //몬스터의 위치


        nvAgent = gameObject.GetComponent<NavMeshAgent>(); //몬스터의 navNMeshAgent 컴포넌트
        
        

        anim = GetComponent<Animator>(); //몬스터의 Animator 컴포넌트

        

        StartCoroutine(this.CheckState()); //코루틴함수
        StartCoroutine(this.CheckStateForAction()); //코루틴함수

       

    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(player.transform.position, _transform.position); //몬스터와 플레이어의 거리

            if(dist <= attackDist)
            {
                curState = CurrentState.attack;
                anim.SetFloat("isAttack", 1);
                SceneManager.LoadScene("Die");
                Destroy(GameObject.Find("SoundManager"));
            }
            else if(dist <= traceDist)
            {
                curState = CurrentState.Walk;
                anim.Play("Run");
            }
            else
            {
                curState = CurrentState.Idle;
                anim.Play("idle");
            }
        }
    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (curState)
            {
                case CurrentState.Idle:
                    nvAgent.Stop();
                    break;
                case CurrentState.Walk:
                    nvAgent.destination = player.transform.position;
                    nvAgent.Resume();
                    break;
                case CurrentState.attack:
                    break;
            }

            yield return null; //코루틴 함수 반환값
        }
    }


    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); //플레이어의 위치
        }

    }
}
