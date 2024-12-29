
using UnityEngine;

public class Shild : MonoBehaviour
{
    public float damageHap;
    public float f_shildTime;
    CapsuleCollider capsuleCollider;
    public GameObject hitPaticle;
    bool b_attack;
    Skill skill;
    public int hitCount = 0;
    private void Awake()
    {

        skill = GetComponentInParent<Skill>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    private void Update()
    {
        //쉴드 후 공격 대기 시간까지 4초
        f_shildTime += Time.deltaTime;
        if (f_shildTime >= 4)
        {
            if (Input.GetMouseButtonDown(1))//버튼을 누르면 쉴드에 저장되었던 받은 공격을 그대로 돌려줌
            {
                b_attack = true;

            }
            if (f_shildTime >= 8)//버튼을 누르지 안거나 8초가 지나면 시간 초기화
            {
                f_shildTime = 0f;
            }
        }
        if (b_attack)//공격중 트리거 상태로 변하고 크기가 커짐
        {
            gameObject.transform.localScale += Vector3.one * Time.deltaTime * 3;
            capsuleCollider.isTrigger = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //몬스터 에게 받은 데미지 저장
        if (collision.gameObject.CompareTag("Enemy"))
        {
            damageHap += 150;
        }

        if (collision.gameObject.layer == 10)
        {
            damageHap += 150;
        }

        if (collision.gameObject.layer == 11)
        {
            damageHap += 250;

        }
        if (collision.gameObject.layer == 12)
        {
            damageHap += 100;
        }
        //얼음용
        if (collision.gameObject.CompareTag("BoneDragon"))
        {
            damageHap += 50;
        }
        if (collision.gameObject.CompareTag("Desh"))
        {
            damageHap += 200;
        }
        //죽음의 기사
        if (collision.gameObject.CompareTag("Arrow"))
        {
            damageHap += 100;
        }
        if (collision.gameObject.CompareTag("Ax"))
        {
            damageHap += 100;
        }

        if (collision.gameObject.layer == 30)
        {
            damageHap += 40;
        }
        //어둠의 망령
        if (collision.gameObject.CompareTag("MonsterAttack"))
        {
            damageHap += 10;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            hitCount += 1;//공격을 돌려준 몬스터 수
            if (hitCount == skill.skillEnhanceCount[4])//강화 수 만큼 추가 공격 가능
            {
                //맞았으면 기본 상태로 초기화
                f_shildTime = 0;
                b_attack = false;
                capsuleCollider.isTrigger = false;
                gameObject.transform.localScale = Vector3.one;
                GameObject hit = Instantiate(hitPaticle, other.transform);
                Destroy(hit, 1);
                gameObject.SetActive(false);
            }

        }
    }
}
