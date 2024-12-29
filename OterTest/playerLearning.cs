using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLearning : MonoBehaviour
{
    public Animator animator; // Animator 컴포넌트를 연결할 변수
    public float attackInterval = 1.0f; // 공격 간격
    private float nextAttackTime = 0f; // 다음 공격 가능 시간
    private bool attackBool = false; // 공격 애니메이션 상태 변수

    // Start is called before the first frame update
    void Start()
    {
        // Animator 컴포넌트를 가져옵니다.
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 시간보다 다음 공격 가능 시간이 크면 공격 수행
        if (Time.time >= nextAttackTime)
        {
            // 공격 애니메이션 실행
            Attack();
            // 다음 공격 가능 시간 설정
            nextAttackTime = Time.time + attackInterval;
        }
    }

    void Attack()
    {
        attackBool = true; // 공격 상태를 true로 설정
        animator.SetBool("AttackBool", attackBool); // 주먹 기본 공격 애니메이션 실행

        // 공격 후 잠시 후 공격 상태를 false로 변경하여 애니메이션을 중지
        StartCoroutine(ResetAttackBool());
    }

    private IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(0.5f); // 공격 애니메이션의 지속 시간에 맞춰 대기
        attackBool = false; // 공격 상태를 false로 설정
        animator.SetBool("AttackBool", attackBool); // 애니메이션 중지
    }
}
