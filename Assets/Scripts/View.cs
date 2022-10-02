using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class View : MonoBehaviour
{
    [SerializeField] float m_angle = 0f; //AI 시야각
    [SerializeField] float m_distance = 0f; //AI 시야 거리
    [SerializeField] LayerMask m_layerMask = 0; // 플레이어 레이어 마스크 (타켓의 레이어만 검출할 수 있는 것)
    public enum CurrentState { Idle, Walk, Run, attack, Death };
    public CurrentState curState = CurrentState.Idle;

   
    Animator anim;
    NavMeshAgent m_enemy = null;

   

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
                    if (t_hit.transform.name == "Player") //Ray에 닿은 객체가 Player라면 둘 사이에 장애물이 없는 걸로 간주
                    {
                        curState = CurrentState.Run;
                        anim.Play("Run");
                        transform.position = Vector3.Lerp(transform.position, t_hit.transform.position, 0.001f); //플레이어를 향해 다가감
                    }
                }
            }
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>(); //몬스터의 Animator 컴포넌트
        m_enemy = GetComponent<NavMeshAgent>();


    }

    void Update()
    {
        Sight();
    }
}