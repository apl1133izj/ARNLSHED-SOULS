using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNOAIMonster : MonoBehaviour
{
    public Transform[] points; // 이동 포인트 배열
    public float speed = 3.0f; // 이동 속도
    private int currentPointIndex = 0;

    void Update()
    {
        // 포인트가 없으면 실행하지 않음
        if (points.Length == 0)
            return;

        // 현재 위치와 다음 포인트 사이의 거리 계산
        Transform targetPoint = points[currentPointIndex];
        Vector3 targetPosition = targetPoint.position;
        float step = speed * Time.deltaTime;

        // MoveTowards를 사용하여 캐릭터 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // 목표 지점에 거의 도달했는지 확인
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // 다음 포인트로 이동
            currentPointIndex = (currentPointIndex + 1) % points.Length;
        }
    }
}
