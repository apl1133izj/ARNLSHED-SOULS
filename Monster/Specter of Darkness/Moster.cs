using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Moster : MonoBehaviour
{
    //체력
    [SerializeField]
    private float hp = 1200;

    public Material hide_Alpha;                //몬스터 알파 값을 조절할 머티리얼
    public Image[] hide_Hp;                    //몬스터 체력바 알파 값을 조절할 스프라이트
    private Color originalColor;
    private GameObject target;                 //플레이어 오브젝트
    public GameObject rotationEat;             //먹는애니메이션 실행시 회전되는 게임오브젝트
    public float maxDistance = 10f;            // 최대 거리 (이 거리 이상에서는 완전히 투명해짐)
    public float f_RediscoveringMaxDistance;   //재탐색 을 한 후 최대 사거리가 늘어남 (참조: MonsterAgents 트리거에 코르틴
    public Animator animator;
    public Animator eatAnimator;
    //플레이어 상호작용 공격 달라붙기,상태이상
    [SerializeField]
    public bool startSerch = false;                  //몬스터가 생성시 플레이어를 찾았은 후의 상태인지
    public bool stayRush;                     //점프중인경우에만 잡기 공격이 실행됨
    public bool b_wall;                              //점프도중 벽에 부딪칠경우 점프를 멈춤
    public bool b_SmellSearchComplete;        //냄새가 플레이어에게 도착 했을경우에 몬스터가 움직임
    public bool b_cling;                      //플레이어에게 달라 붙기위해 점프 를 했는지(MonsterAgents에서 사용함:플레이어가 serch에 걸렸을경우(콜라이더에 부딪친 경우))
    public bool b_catch;                      //플레이어가 달라 붙었는지
    public bool isSearching = false;          // 코루틴 실행 여부를 추적하는 변수
    public int i_escape;
    public float coolTime = 0f;
    public GameObject g_SmellSerch;
    public bool b_Freeze;
    public float f_FreezeTime;
    public GameObject g_FreezePaticle;
    [Range(-10f, 10f)]
    public float y;
    [Range(-10f, 10f)]
    public float z;
    //카메라 설정
    public Camera defaultCamera;             //몬스터가 달라 붙기 전
    public Camera clingAttackCamera;         //후
    public GameObject tip;
    PlayerUI playerUI;
    bool isCatching = false;                 // 코루틴 실행 상태를 확인하는 플래그 변수
    AttackAndCombo player;
    MonsterAgents monsterAgents;
    public QuestManager questManager;
    HP playerHp;
    public GameObject[] die_SetActive;
    float f_ResurrectionTime = 0f;
    public GameObject[] g_ClearItem;
    public AudioClip runSound;
    public AudioClip damageSound;
    public Skill skill;
    public Shild shild;
    void Start()
    {
        StartCoroutine(SmellSerchInst());
        originalColor = hide_Alpha.color;
        target = GameObject.Find("Player");
        player = target.GetComponent<AttackAndCombo>();
        playerHp = target.GetComponent<HP>();
        tip = GameObject.Find("Player UI");
        playerUI = tip.GetComponent<PlayerUI>();
        monsterAgents = GetComponentInChildren<MonsterAgents>();
        f_RediscoveringMaxDistance = 15f;
        //questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        //animator.Play("Creep_Idle1_Action");
    }

    private void FixedUpdate()
    {
        if (!b_Freeze)
        {
            if (ClingCooltime() && b_SmellSearchComplete)//쿨타임
            {
                if (Distance() >= 5)
                {
                    StartCoroutine(ClingAttack());
                    animator.SetFloat("animationspeed", 1f);
                }
                else
                {
                    animator.SetFloat("animationspeed", 3f);
                }
            }
            LostPlayer();

        }
        else
        {
            f_FreezeTime += Time.deltaTime;
            monsterAgents.agent.speed = 0f;
            g_FreezePaticle.gameObject.SetActive(true);
            //스킬이 업그레이드 되기전에는 2초 + 강화 될때마다 1초씩증가
            if (f_FreezeTime >= 2 + skill.skillEnhanceCount[3])
            {
                g_FreezePaticle.gameObject.SetActive(false);
                monsterAgents.agent.speed = 3f;
                f_FreezeTime = 0;
                b_Freeze = false;
            }
        }
        if (Die())//죽을경우
        {
            //monsterAgents.agent.isStopped = true;
            //퀘스트 몬스터 남은 횟수 감소
            eatAnimator.SetTrigger("DieTrigger");
            animator.SetTrigger("DieTrigger");
            DieMonster();
        }

        /*        //플레이어를 잡음(상호작용 공격)
                if (b_catch)
                {
                    StartCoroutine(Catch());
                }*/

    }
    void DieMonster()
    {
        die_SetActive[0].SetActive(false);
        die_SetActive[1].SetActive(false);
        die_SetActive[2].SetActive(false);
        hide_Hp[0].enabled = false;
        f_ResurrectionTime += Time.deltaTime;
        monsterAgents.agent.speed = 0;
        if (f_ResurrectionTime >= 60)
        {
            monsterAgents.agent.speed = 3;
            f_ResurrectionTime = 0;
            Resurrection();
        }
    }
    void Resurrection()
    {
        hide_Hp[0].enabled = true;
        die_SetActive[0].SetActive(true);
        die_SetActive[1].SetActive(true);
        die_SetActive[2].SetActive(true);
        hp = 1200;
        monsterAgents.agent.isStopped = false;
    }
    bool Die() //죽었는지 확인
    {
        if (hp < 0f)
        {

            if (die_SetActive[0].activeSelf || die_SetActive[1].activeSelf || die_SetActive[2].activeSelf)
            {
                questManager.i_questMosterCount[questManager.questProgress]--;
                ItemInst();
            }
            return true;
        }
        else
        {
            Hp();

            return false;
        }
    }
    public void ItemInst()
    {
        int rand = Random.Range(0, 3);
        GameObject item1 = Instantiate(g_ClearItem[rand], gameObject.transform.position + new Vector3(1, 2, 0), Quaternion.identity);
        item1.name = g_ClearItem[0].name;
        Vector3 randomForce1 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item1.GetComponent<Rigidbody>().AddForce(randomForce1);
    }
    void Hp()
    {
        hide_Hp[0].fillAmount = hp / 1200;
    }
    void LostPlayer()//몬스터가 플레이어를 일정 사거리에서 놓침
    {
        //처음은 플레이어를 무족건 찾아야 하기때문에 거리와 상관없이 탐색
        //그렇기 때문에 startSerch가 true가 되기전까지는 이코드가 실행되면 안됨
        //몬스터가 생성후 startSerch 기본 상태 false
        if (startSerch)
        {
            //플레이와의 거리가 15이상일 경우 멈춘후 플레이어 재탐색
            //하지만 재탐색후 다시 플레이어를 찾았을 경우 일시적으로 사거리가 30까지 늘어남
            if (Distance() >= f_RediscoveringMaxDistance)
            {
                if (!isSearching && !Die()) StartCoroutine(SmellSerchInst());
                animator.SetBool("SerchBool", true);

                monsterAgents.agent.isStopped = true;//몬스터 멈춤
                b_SmellSearchComplete = false;//냄새 찾기 실패
            }
            else
            {
                animator.SetBool("SerchBool", false);
                monsterAgents.agent.isStopped = false;//몬스터 이동
                b_SmellSearchComplete = true;//새 찾기 성공
            }
        }
    }
    bool ClingCooltime()//점프 쿨타임
    {
        if (isSearching) return false; // 탐색 중일 때는 쿨타임 무시

        coolTime += Time.deltaTime;

        if (coolTime >= 12f)
        {
            coolTime = 0f; // 쿨타임 초기화
            Debug.Log("ClingCooltime 완료!"); // 디버깅 로그
            return true;
        }

        //Debug.Log($"ClingCooltime 진행 중: {coolTime:F2}s"); // 진행 시간 로그
        return false;
    }
    float AlphaValueAccordingToDistance() // 거리 기반 알파 값 계산
    {
        // 거리가 멀수록 투명해지고, 가까워질수록 불투명해지도록 1에서 0으로 알파 값을 조정
        float alpha = 1 - Mathf.Clamp01(Distance() / maxDistance);
        return alpha;
    }
    float Distance()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        //Debug.Log(distance);
        return distance;
    }
    IEnumerator SmellSerchInst()//플레이어 찾기
    {
        if (isSearching) yield break; // 이미 탐색 중이면 종료
        isSearching = true;

        // 탐색 오브젝트 생성
        GameObject instSerch = Instantiate(g_SmellSerch, transform.position, Quaternion.identity);
        float serchTime = 0;

        // 탐색 루프 시작
        while (instSerch != null)
        {
            yield return new WaitForSeconds(1); // 초당 1회 체크
            serchTime += 1;

            if (serchTime >= 18)
            {
                // 18초 동안 플레이어를 찾지 못한 경우
                Debug.Log("18초 동안 플레이어를 찾지 못했습니다. 재탐색 중...");
                Destroy(instSerch);
                instSerch = null;
            }
        }

        // 탐색 종료 후 상태 초기화
        isSearching = false;
        Debug.Log("탐색 종료");
    }
    IEnumerator ClingAttack() // 플레이어 위치로 돌진
    {
        Debug.Log("ClingAttack 실행중");

        b_cling = false;
        stayRush = true;
        b_wall = false;
        // 알파값 설정
        Color colorWithUpdatedAlpha = originalColor;
        colorWithUpdatedAlpha.a = 1f;
        hide_Alpha.color = colorWithUpdatedAlpha;
        SoundManager.PlaySound(runSound);
        // 목표 위치 설정
        float rushTime = 0f;
        // jumpTime이 2초 이하일 때만 반복
        while (rushTime < 2 && !b_wall)
        {

            rushTime += Time.deltaTime;
            monsterAgents.agent.speed = 12;
            animator.SetFloat("animationspeed", 4);
            colorWithUpdatedAlpha = originalColor;
            colorWithUpdatedAlpha.a = 1f;
            hide_Alpha.color = colorWithUpdatedAlpha;

            eatAnimator.SetBool("RushBool", true);
            animator.SetBool("RushBool", true);

            yield return null;
        }
        animator.SetFloat("animationspeed", 1);
        monsterAgents.agent.speed = 3;
        // 돌진 종료 후 초기화
        eatAnimator.SetBool("RushBool", false);
        animator.SetBool("RushBool", false);
        startSerch = true;
        stayRush = false;
    }
    IEnumerator Catch()
    {
        // 이미 실행 중인 경우 중복 실행 방지
        if (isCatching) yield break;

        isCatching = true;

        if (!playerUI.b_tip)
        {
            playerUI.g_tipUI.SetActive(true);
        }

        while (i_escape <= 10)
        {
            eatAnimator.SetTrigger("EatTrigger");
            b_catch = false;
            Vector3 playerPosition = target.transform.position + new Vector3(0, y, z);
            transform.position = playerPosition;
            rotationEat.transform.rotation = Quaternion.Euler(-48f, 0, 0);
            transform.localScale = new Vector3(-1, 1, 1);
            clingAttackCamera.gameObject.SetActive(true);
            defaultCamera.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바 입력 체크
            {
                i_escape += 1;
                if (!playerUI.b_tip)
                {
                    playerUI.spaceCount.text = i_escape.ToString();
                }
                if (i_escape >= 10)
                {
                    if (!playerUI.b_tip)
                    {
                        playerUI.g_tipUI.SetActive(false);
                    }
                    Debug.Log("벗어남");
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.rotation = Quaternion.Euler(0f, 0f, 0);
                    //rb.useGravity = true;//중력 활성화
                    animator.SetTrigger("JumpTrigger");
                    StartCoroutine(Escape());//설정 초기화
                    isCatching = false; // 코루틴 종료 상태로 설정
                    yield break;
                }
                yield return new WaitForSeconds(0.1f); // 입력 후 0.1초 대기
            }

            yield return null;
        }

        playerUI.b_tip = true;
        isCatching = false; // 코루틴 종료 상태로 설정
    }
    IEnumerator Escape()
    {
        Debug.Log("Escape");
        clingAttackCamera.gameObject.SetActive(false);
        defaultCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        i_escape = 0;
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("R"))
        {
            hp -= 5;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HandAttack"))
        {
            SoundManager.PlaySound(damageSound);
            Debug.Log("Combo Attack Count: " + AttackAndCombo.comboAttakccount);
            switch (AttackAndCombo.comboAttakccount)
            {
                case 0:
                    Debug.Log("공격1");
                    hp -= player.attackDamage[0];
                    break;
                case 1:
                    Debug.Log("공격2");
                    hp -= player.attackDamage[1];
                    break;
                case 2:
                    Debug.Log("공격3");
                    hp -= player.attackDamage[2];
                    break;
                case 3:
                    Debug.Log("공격4");
                    hp -= player.attackDamage[3];
                    break;
            }
        }
        if (other.gameObject.CompareTag("ShildAttack"))
        {
            hp -= shild.damageHap;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.GetComponent<AttackAndCombo>();
            //점프중인경우에만 상호작용공격이 활성화
            if (i_escape < 10 && stayRush && !player.invincibilityBool)
            {
                b_catch = true;
            }
            b_wall = true;//벽이나 플레이어에 부딪친경우 돌진 코르틴에서 돌아가고 있는 while문이 종료가 됨
        }
        if (other.gameObject.layer == 13 && stayRush)
        {
            b_wall = true;//벽이나 플레이어에 부딪친경우 돌진 코르틴에서 돌아가고 있는 while문이 종료가 됨
        }
        if (other.gameObject.CompareTag("freeze"))
        {
            b_Freeze = true;
        }
    }
}
