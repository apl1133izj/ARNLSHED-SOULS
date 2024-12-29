using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAIMonster : MonoBehaviour
{
    public Transform[] points; // 이동 포인트 배열
    private NavMeshAgent agent;
    private int destPoint = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 자동으로 회전하지 않도록 설정
        agent.autoBraking = false;

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // 포인트가 없으면 반환
        if (points.Length == 0)
            return;

        // 다음 위치 설정
        agent.destination = points[destPoint].position;

        // 다음 포인트로 인덱스 갱신
        destPoint = (destPoint + 1) % points.Length;
    }

    void Update()
    {
        // 에이전트가 목적지에 거의 도달했는지 확인
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }
    
}
