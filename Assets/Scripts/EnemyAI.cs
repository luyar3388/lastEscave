using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float m_angle = 0f; //AI 시야각
    [SerializeField] float m_distance = 0f; //AI 시야 거리
    [SerializeField] LayerMask m_layerMask = 0; // 플레이어 레이어 마스크 (타켓의 레이어만 검출할 수 있는 것)
    public enum CurrentState { Idle, Walk, Run, attack, Death };
    public CurrentState curState = CurrentState.Walk;
    
    
    
    Animator anim;
    NavMeshAgent m_enemy = null;

    [SerializeField] Transform[] m_tfWayPoints = null;
    int m_count = 0;

    Transform m_target = null;

    public VideoPlayer video;
    public GameObject attackedImage;
    //GameObject attackedPlayer = GameObject.FindGameObjectWithTag("Player");

    public GameObject LifeDirector;
  
    
    public void SetTarget(Transform p_target) //위험지역 들어오면 순찰을 취소하고 타겟을 향해 쫓아감
    {
        curState = CurrentState.Walk;
        anim.Play("Walk");
        CancelInvoke(); //순찰을 취소
        m_target = p_target;
    }
    public void RemoveTarget()
    {
        m_target = null;
        curState = CurrentState.Idle;
        anim.Play("Idle");
        InvokeRepeating("MoveToNextWayPoint", 0f, 0.1f); // 위험지역을 나가면 다시 순찰을 재개
    }

    void MoveToNextWayPoint() //순찰함수
    {
        if (m_target == null) //타겟이 없으면
        {
            curState = CurrentState.Walk;
            anim.Play("Walk");
            if (m_enemy.velocity == Vector3.zero)
            {
                m_enemy.SetDestination(m_tfWayPoints[m_count++].position);

                if (m_count >= m_tfWayPoints.Length)
                    m_count = 0;
            }
        }
    }



    void Start()
    {
        anim = GetComponent<Animator>(); //몬스터의 Animator 컴포넌트
        m_enemy = GetComponent<NavMeshAgent>();
        InvokeRepeating("MoveToNextWayPoint", 0f, 0.1f); //3초마다 반복
        LifeDirector = GameObject.Find("LifeDirector");
    }


    void Update()
    {
        //if (attackedImage.activeSelf)
        //{
        //    timer += Time.deltaTime;
        //    if (timer > waitingTime)
        //    {
        //        video.Stop();
        //        attackedImage.SetActive(false);
        //    }
        //    return;
        //}

        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_distance, m_layerMask); // 주변에 있는 플레이어 컬라이더를 검출

        if (t_cols.Length > 0) //검출 되었다면
        {
            Transform t_tfPlayer = t_cols[0].transform; //플레이어의 트랜스폼 정보를 받아 옴 (플레이어는 1명뿐, 검출될 컬라이더도 한개므로 인덱스 0은 무조건 플레이어)

            Vector3 t_direction = (t_tfPlayer.position - transform.position).normalized; // 플레이어가 어느 방향에 있는지 구함
            float t_angle = Vector3.Angle(t_direction, transform.forward); // 플레이어 방향의 각도 차이
            if (t_angle < m_angle * 0.5f) // 몬스터의 방향과 플레이어 방향의 각도 차이가 시야각보다 작으면
            {
                if (Physics.Raycast(transform.position, t_direction, out RaycastHit t_hit, m_distance)) //시야각 안에 있다면 Ray를 플레이어에 쏨 (장애물이 없는지 체크)
                {
                    if (t_hit.transform.CompareTag("Player")) //Ray에 닿은 객체가 Player라면 둘 사이에 장애물이 없는 걸로 간주
                    {
                        if (GameManagement.staticTurnOnLighter)
                        {
                            CancelInvoke();
                            m_target = t_tfPlayer;
                            curState = CurrentState.Run;
                            anim.Play("Run");
                            transform.position = Vector3.Lerp(transform.position, t_hit.transform.position, 0.001f); //플레이어를 향해 다가감
                        }
                    }
                   
                }
                
            }
            else
            {
                m_target = null;
                InvokeRepeating("MoveToNextWayPoint", 0f, 0.1f); // 다시 순찰을 재개
            }

        }

        if (m_target != null)
        {
            m_enemy.SetDestination(m_target.position);
        }

        

    }

    void Sight() //시야 체크 함수
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_distance, m_layerMask); // 주변에 있는 플레이어 컬라이더를 검출

        if (t_cols.Length > 0) //검출 되었다면
        {
            Transform t_tfPlayer = t_cols[0].transform; //플레이어의 트랜스폼 정보를 받아 옴 (플레이어는 1명뿐, 검출될 컬라이더도 한개므로 인덱스 0은 무조건 플레이어)

            Vector3 t_direction = (t_tfPlayer.position - transform.position).normalized; // 플레이어가 어느 방향에 있는지 구함
            float t_angle = Vector3.Angle(t_direction, transform.forward); // 플레이어 방향의 각도 차이
            if (t_angle < m_angle * 0.5f) // 몬스터의 방향과 플레이어 방향의 각도 차이가 시야각보다 작으면
            {
                if (Physics.Raycast(transform.position, t_direction, out RaycastHit t_hit, m_distance)) //시야각 안에 있다면 Ray를 플레이어에 쏨 (장애물이 없는지 체크)
                {
                    if (t_hit.transform.CompareTag("Player")) //Ray에 닿은 객체가 Player라면 둘 사이에 장애물이 없는 걸로 간주
                    {
                        if (GameManagement.staticTurnOnLighter)
                        {
                            CancelInvoke();
                            m_target = t_tfPlayer;
                            curState = CurrentState.Run;
                            anim.Play("Run");
                            transform.position = Vector3.Lerp(transform.position, t_hit.transform.position, 0.001f); //플레이어를 향해 다가감

                        }

                    }
                    else
                    {
                        m_target = null;
                    }
                }
            }
        }
    }

    /* private void OnTriggerStay(Collider other)
     {
         if (m_target == null)
         {
             if (other.CompareTag("WayPoint"))
             {
                 curState = CurrentState.Idle;
                 anim.Play("Idle");
             }
         }

     }



     private void OnTriggerExit(Collider other)
     {
         if (m_target == null)
         {
             if (other.CompareTag("WayPoint"))
             {
                 curState = CurrentState.Walk;
                 anim.Play("Walk");
             }
         }

     }*/


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameManagement.staticTurnOnLighter)
        {
            //timer = 0.0f;
            //attackedImage.SetActive(true);
            //video.Stop();
            //video.Play();
            LifeDirector.GetComponent<LifeDirector>().lifeControl();
            other.GetComponent<AudioSource>().Play();
            other.gameObject.transform.parent.gameObject.transform.position = new Vector3(-40, -12, 0);
            other.gameObject.transform.parent.gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }
}
