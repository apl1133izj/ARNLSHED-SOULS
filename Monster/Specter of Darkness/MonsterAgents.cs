using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class MonsterAgents : MonoBehaviour
{
    public enum aiType { moster, serch }
    public aiType type;
    public Transform t_StartPos;
    public enum MonsterType { nul, Creep2, Creep3, Creep4, Demon_damaged };
    public MonsterType monsterType;
    public NavMeshAgent agent;
    public float resurrectionTime = 0;
    //처음 시작
    public bool startserchs = true;

    // 목적지 설정
    GameObject player;
    public Transform target;
    public GameObject p_serchPaticle;//파티클
    public GameObject g_smell;
    public Moster monster;
    RangedMonster rangedMonster;
    public GameObject demo;
    bool b_attackCoroutine = false;
    bool b_Arrival; //도착 여부
    bool b_RediscoveringMaxDistanceCoroutine;
    //부활 죽음 관련
    private bool isDead = false; // 몬스터가 죽었는지 여부
    public bool isResurrecting = false; // 부활 중인지 여부
    public float f_ResurrectingFloat = 0;
    public float f_MaxResurrectingFloat = 60;
    public GameObject[] g_ClearItem;

    void Start()
    {
        // NavMeshAgent가 할당되지 않은 경우 자동으로 할당
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent is not attached to this GameObject.");
            }
        }
        // target이 null일 경우, Player 오브젝트를 찾아 자동으로 할당
        // Ensure target is initialized
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player"); // Find the player by its tag
            if (player != null)
            {
                target = player.transform; // Assign the player's transform to target
                Debug.Log("Target assigned automatically to player.");
            }
        }
        if (type == aiType.moster && monsterType != MonsterType.Demon_damaged)
        {
            monster = GetComponent<Moster>();
        }
        else
        {
            rangedMonster = GetComponentInChildren<RangedMonster>();
        }

    }
    void Update()
    {

        if (type == aiType.moster)
        {
            if (monsterType != MonsterType.Demon_damaged)
            {
                if (!monster.b_Freeze)
                {
                    if (monster != null && monster.b_SmellSearchComplete)
                    {
                        MoveAI();
                    }

                }
            }
            else
            {
                if (monsterType == MonsterType.Demon_damaged)
                {
                    if (!rangedMonster.b_Freeze)
                    {
                        if (!rangedMonster.ballAttack)
                        {
                            MoveAI();
                        }
                    }
                }
            }
        }
        else
        {
            Destroy(gameObject, 15);
        }
        if (b_RediscoveringMaxDistanceCoroutine)
        {
            b_RediscoveringMaxDistanceCoroutine = false; // 코루틴이 중복 실행되지 않도록 바로 false로 설정
            StartCoroutine(RediscoveringMaxDistance());
        }
        if (monsterType == MonsterType.Demon_damaged)
        {
            Resurrecting();
        }

    }

    void Resurrecting()
    {
        if (rangedMonster.f_Hp > 0)
        {
            demo.gameObject.SetActive(true);
            if (isResurrecting)
            {
                StartCoroutine(ResurrectingCoroutin());
            }
        }
        else
        {
            f_ResurrectingFloat += Time.deltaTime;
            if (!isDead)
            {
                StartCoroutine(Die());
            }
            if (f_ResurrectingFloat >= f_MaxResurrectingFloat)
            {
                isDead = false;
                rangedMonster.f_Hp = 700;
                f_ResurrectingFloat = 0;
            }
        }
    }
    IEnumerator Die()
    {
        isDead = true;
        rangedMonster.animator.SetTrigger("DieTrigger");
        isResurrecting = true;
        yield return new WaitForSeconds(3);
        ItemInst();
        demo.gameObject.SetActive(false);
    }
    public void ItemInst()
    {
        int rand = Random.Range(0, 3);
        GameObject item1 = Instantiate(g_ClearItem[rand], gameObject.transform.position + new Vector3(1, 2, 0), Quaternion.identity);
        item1.name = g_ClearItem[0].name;
        Vector3 randomForce1 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item1.GetComponent<Rigidbody>().AddForce(randomForce1);
    }
    IEnumerator ResurrectingCoroutin()
    {
        isResurrecting = false;
        agent.enabled = true; // NavMeshAgent 재활성화
        agent.Warp(transform.position); // 현재 위치로 이동 초기화
        rangedMonster.ui.gameObject.SetActive(true); // UI 활성화
        rangedMonster.animator.SetLayerWeight(3, 1f); // 레이어 초기화
        rangedMonster.ballAttack = false;
        rangedMonster.attackCoroutin = false;
        rangedMonster.launchAttackCoroutin = false;
        rangedMonster.b_isAttack = false;
        //rangedMonster.animator.SetTrigger("isResurrectingTrigger");
        yield return new WaitForSeconds(3);
        rangedMonster.animator.SetLayerWeight(0, 1f); // 레이어 초기화
        rangedMonster.animator.Rebind(); // Animator 상태 초기화
    }
    float Distence()
    {
        // target이 null인지 확인
        if (target == null)
        {
            return float.MaxValue; // 기본값 반환
        }
        return Vector3.Distance(transform.position, target.position);
    }
    public IEnumerator SerchSmell()//플레이어 냄새 검색(몬스터가 플레이어의 냄새를 찾습니다)
    {
        float serchTime = 0;// 탐색시간
        float paticleTime = 0;
        bool searchComplete = false;//탐색 시간이 다 되었는지
        GameObject paticle = null;
        //g_smell.SetActive(true);

        Debug.Log(type + "가 이동중");
        if (gameObject != null)
        {
            while (Distence() > 1 && !searchComplete)//플레이어와의 거리가 0이 될때까지 플레이어를 찾고 플레이어를 찾을때까지 반복
            {

                serchTime += Time.deltaTime;
                paticleTime += Time.deltaTime;
                if (paticle != null)//파티클이 있는 경우 삭제
                {
                    Destroy(paticle, 1.25f);
                }
                if (paticleTime >= 1f)//0.25초에 한번씩 파티클 생성
                {
                    paticle = Instantiate(p_serchPaticle, transform.position, Quaternion.identity);

                    paticleTime = 0;
                }
                if (serchTime > 13)
                {
                    searchComplete = true;//탐색시간 종료
                }
                MoveAI();//ai이동
                yield return null;
            }

        }
        else
        {
            yield break;
        }
        yield return new WaitForSeconds(4);
        //g_smell.SetActive(false);
        // transform.position = monster.transform.position;//냄새가 다시 몬스터에게 이동
        /*if(monster.b_SmellSearchComplete) */
        StartCoroutine(SerchSmell());//플레이어가 냄새에 도망쳤거나 찾지 못했을 경우에만 재탐색
    }
    //몬스터 이동
    public void MoveAI()
    {
        if (agent == null)
        {
            return; // Prevent any further operations if agent is not assigned
        }
        if (monsterType == MonsterType.Demon_damaged)
        {
            if (Distence() <= 35)
            {
                RangedMonster.lookat = true;
                agent.stoppingDistance = 25;
                if (target != null && !b_Arrival)
                {
                    // 시작 시 목적지 설정
                    agent.SetDestination(target.position);
                    if (!b_attackCoroutine && monsterType != MonsterType.Demon_damaged)
                    {
                        if (monster.b_SmellSearchComplete)
                        {
                            monster.animator.SetBool("SerchBool", false);
                            monster.animator.SetBool("WalkBool", true);
                        }
                        else
                        {
                            monster.animator.SetBool("WalkBool", false);
                            monster.animator.SetBool("SerchBool", true);
                        }
                    }
                    else
                    {
                        if (agent.isOnOffMeshLink)
                        {
                            rangedMonster.animator.SetBool("Run Bool", false);
                        }
                        else
                        {
                            rangedMonster.animator.SetBool("Run Bool", true);
                        }

                    }
                }

            }
            else
            {
                RangedMonster.lookat = false;
                agent.stoppingDistance = 0;
                agent.SetDestination(t_StartPos.position);
            }
        }
        else
        {
            if (target != null && !b_Arrival)
            {
                // 시작 시 목적지 설정
                agent.SetDestination(target.position);
                if (type == aiType.moster)
                {
                    if (!b_attackCoroutine && monsterType != MonsterType.Demon_damaged)
                    {
                        if (monster.b_SmellSearchComplete)
                        {
                            monster.animator.SetBool("SerchBool", false);
                            monster.animator.SetBool("WalkBool", true);
                        }
                        else
                        {
                            monster.animator.SetBool("WalkBool", false);
                            monster.animator.SetBool("SerchBool", true);
                        }
                    }
                    else
                    {
                        if (agent.isOnOffMeshLink)
                        {
                            rangedMonster.animator.SetBool("Run Bool", false);
                        }
                        else
                        {
                            rangedMonster.animator.SetBool("Run Bool", true);
                        }

                    }
                }
            }
        }

        // 목적지까지의 거리를 체크하여 도착했을 때 행동 설정
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !agent.isOnOffMeshLink)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                // 목적지에 도착한 경우의 로직
                if (monsterType != MonsterType.Demon_damaged)
                {
                    OnDestinationReached();
                }
                else//MonsterType.Demon_damaged 인경우
                {
                    if (!rangedMonster.b_isAttack && Distence() <= 35) rangedMonster.RandomAttack();
                    rangedMonster.animator.SetBool("Run Bool", false);
                }

            }
        }
    }
    private void OnDestinationReached()
    {
        // 목적지 도착 시 호출되는 메서드
        if (type == aiType.moster)
        {

            if (!b_attackCoroutine) StartCoroutine(Attack(4f));
        }
    }
    IEnumerator Attack(float attackTime)
    {
        b_attackCoroutine = true;
        b_Arrival = true;
        monster.animator.SetBool("WalkBool", false);
        monster.animator.SetTrigger("AttackTrriger");
        monster.eatAnimator.SetTrigger("AttackTrriger");
        yield return new WaitForSeconds(attackTime);
        b_Arrival = false;
        b_attackCoroutine = false;
    }
    // 새 목적지를 설정하는 메서드
    public void SetNewDestination(Vector3 newDestination)
    {
        agent.ResetPath();
        agent.SetDestination(newDestination);
    }
    IEnumerator RediscoveringMaxDistance()
    {
        monster.f_RediscoveringMaxDistance = 30;
        Debug.Log("거리 30으로 변경");
        yield return new WaitForSeconds(2);
        monster.f_RediscoveringMaxDistance = 15;
        Debug.Log("거리 15으로 초기화");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (type == aiType.serch)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                target = other.transform; // 충돌 시 target 설정

                Debug.Log($"Collision detected with {other.gameObject.name}");
                Destroy(gameObject);
                if (monster != null)
                {
                    monster.isSearching = false;
                    monster.startSerch = true;
                    monster.b_SmellSearchComplete = true;
                }
            }
        }

        if (other.gameObject.CompareTag("Monster") || other.gameObject.name == "Creep")
        {
            Debug.Log("서치가 몬스터와 부딪침");
            startserchs = false;
            monster = other.GetComponent<Moster>();
            if (monster == null)
            {
                Debug.LogError($"Monster component not found on {other.gameObject.name}");
                return;
            }

            if (type == aiType.serch && !monster.b_SmellSearchComplete)
            {
                Destroy(gameObject, 15);
                StartCoroutine(SerchSmell());
            }
        }
    }
}
