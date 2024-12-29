using System.Collections;
using UnityEngine;
public class UndeadKnight : MonoBehaviour
{
    GameObject player;

    public int weponChange; //0:도끼 1:활
    public bool attackBool;

    public GameObject[] g_WeponChange;
    public bool b_ShotBool;//활관련(화살을 쐈는지)
    public Animator animator;
    public GameObject[] g_Animation_bow_Load;
    public static int randomArrow;

    public bool b_Knight_direct, b_Load, b_Idle, b_Ax, b_WeponChange;
    public bool b_Crosed; //오른쪽 왼쪽 에 있는가;
    public float f_direct, f_Ax;


    public float angle;
    Bow bow;
    public float[] attackCoolTime; // 0:도끼 1:활
    public bool isSwitching;
    private int currentWeaponType = 0;
    Vector3 playerStartPos;
    public float loadboolTime = 0;
    public bool weponchangeNot;
    UndedHouse undedHouse;
    public PlayerBoseRoomCheck check;
    public AudioClip[] attackClip;

    //플레이어에 의한 상태 이상
    public bool b_Freeze;
    public float f_FreezeTime;
    public GameObject g_FreezePaticle;
    public Skill skill;
    private void Awake()
    {
        undedHouse = GetComponentInParent<UndedHouse>();
        bow = GetComponentInChildren<Bow>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerStartPos = player.transform.position;
    }
    void Update()
    {
        if (!b_Freeze)
        {
            if (Distance() > 120)
            {
                /// check.playerCheck = false;
                undedHouse.enabled = false;
                undedHouse.Resurrection();
            }
            else
            {
                if (undedHouse.f_undedHouseHp > 0 /*&& check.playerCheck*/)
                {
                    WeponSwitch();
                    AnimatorControll();
                    Attack(weponChange);
                    undedHouse.enabled = true;
                }
            }
        }
        else
        {
            animator.enabled = false;
            f_FreezeTime += Time.deltaTime;
            g_FreezePaticle.gameObject.SetActive(true);
            //스킬이 업그레이드 되기전에는 2초 + 강화 될때마다 1초씩증가
            if (f_FreezeTime >= 2 + skill.skillEnhanceCount[3])
            {
                g_FreezePaticle.gameObject.SetActive(false);
                f_FreezeTime = 0;
                b_Freeze = false;
            }
        }
    }

    private void WeponSwitch()
    {
        if (!weponchangeNot)
        {
            if (Distance() < 26f)
            {
                weponChange = 0;
            }
            else if (Distance() > 26f)
            {
                weponChange = 1;
            }
            // 거리에 따라 무기를 교체하는 로직
            if (Distance() < 26f && currentWeaponType != 0 && !isSwitching)
            {
                // 근거리 무기로 교체
                StartCoroutine(WeaponChangeCoroutine(0));
            }
            else if (Distance() > 26f && currentWeaponType != 1 && !isSwitching)
            {
                // 원거리 무기로 교체

                StartCoroutine(WeaponChangeCoroutine(1));
            }
        }
    }
    float Distance()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }
    float Angle()
    {
        // 플레이어와 몬스터의 방향 벡터를 계산
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // 몬스터의 정면 벡터
        Vector3 monsterForward = transform.forward;

        // 몬스터의 오른쪽 벡터
        Vector3 monsterRight = transform.right;

        // 몬스터의 정면을 기준으로 각도 계산 (y축을 기준으로 회전)
        float angle = Vector3.Angle(monsterForward, directionToPlayer);

        // 각도의 부호 계산 (왼쪽이면 양수, 오른쪽이면 음수로 변경)
        float sign = Mathf.Sign(Vector3.Dot(monsterRight, directionToPlayer)) * -1;

        // 부호를 적용한 최종 각도
        float signedAngle = angle * sign;


        // 각도 출력
        //Debug.Log(signedAngle);

        return signedAngle;
    }


    void AnimatorControll()
    {
        animator.SetBool("IdleBool", b_Idle);

        animator.SetBool("DirectionBool", b_Knight_direct);
        animator.SetFloat("DirectionBlend", Angle());

        animator.SetBool("LoadBool", b_Load);
        animator.SetFloat("LoadBlend", Angle());

        animator.SetBool("AxBool", b_Ax);
        animator.SetFloat("AxFloat", f_Ax);
    }

    void Attack(int typeChange)
    {
        Vector3 localPlayerPos = transform.InverseTransformPoint(player.transform.position);
        Vector3 crose = Vector3.Cross(Vector3.forward, localPlayerPos.normalized);
        float dis = Vector3.Distance(player.transform.position, transform.position);
        float dot = Vector3.Dot(Vector3.forward, localPlayerPos);
        //Debug.Log(dis);
        attackCoolTime[typeChange] += Time.deltaTime;

        //도끼 게임오브젝트가 활성화 되있거나 쿨타임이 2보다 크고 공격 신호가 있을경우
        if (typeChange == 0 && g_WeponChange[0].gameObject.activeSelf && attackCoolTime[0] > 5f && attackBool)
        {
            if (crose.y > 0) //오른쪽 | 11.5보다 가까울경우
            {
                SoundManager.PlaySound(attackClip[0]);
                b_Ax = true;
                f_Ax = 0;
                attackCoolTime[0] = 0;
            }
            else if (crose.y < 0)
            {
                SoundManager.PlaySound(attackClip[0]);
                b_Ax = true;
                f_Ax = 2;
                attackCoolTime[0] = 0;
            }
        }
        else
        {
            b_Ax = false;
        }
        if (typeChange == 1 && g_WeponChange[2].gameObject.activeSelf && attackCoolTime[1] > 5f)
        {
            if (dot > 0f || dis > 18.5f) //앞 11.5보다 멀경우
            {
                SoundManager.PlaySound(attackClip[0]);
                attackCoolTime[1] = 0;
                b_Load = true;
            }
            else if (dot < 0f || dis > 18.5f)//뒤11.5보다 멀경우
            {
                SoundManager.PlaySound(attackClip[0]);
                attackCoolTime[1] = 0;
                b_Load = true;
            }
        }
        //활 게임오브젝트가 활성화 되있거나 쿨타임이 2 보다 크고 공격 신호가 있을경우
        if (typeChange == 1 && g_WeponChange[2].gameObject.activeSelf && attackCoolTime[1] > 3f)
        {                // 플레이어와 몬스터의 위치
                         // 애니메이터에 각도를 전달하여 블렌드 트리에서 애니메이션이 자연스럽게 전환되도록 설정
            
            attackCoolTime[1] = 0;
            b_Load = true;
        }



        if (b_Load)
        {
            loadboolTime += Time.deltaTime;
            if (loadboolTime > 4)
            {
                b_Load = false;
                loadboolTime = 0;
            }
        }
    }
    public IEnumerator WeaponChangeCoroutine(int weaponType)
    {
        Debug.Log("WeaponChange 코루틴 시작");
        isSwitching = true;
        // 무기 교체 준비 상태 초기화
        b_Knight_direct = false;
        b_ShotBool = false;
        b_Load = false;
        // 1.5초 대기
        animator.SetBool("WeponChangeBool", true);
        yield return new WaitForSeconds(0.5f);

        // 무기 타입에 따라 무기 활성화/비활성화
        switch (weponChange)
        {
            case 0: // 근거리 무기
                g_WeponChange[0].SetActive(true);  // 근거리 무기 활성화
                g_WeponChange[1].SetActive(false); // 근거리 무기 등 장착 비활성화
                g_WeponChange[2].SetActive(false); // 원거리 무기 비활성화
                g_WeponChange[4].SetActive(true); // 활 등 장착 활성화
                break;

            case 1: // 원거리 무기
                g_WeponChange[0].SetActive(false); // 근거리 무기 비활성화
                g_WeponChange[1].SetActive(true);  // 근거리 무기 등 장착 비활성화
                g_WeponChange[2].SetActive(true);  // 워거리 무기 활성화
                g_WeponChange[4].SetActive(false); // 활 등 장착 비활성화
                break;

            default:
                Debug.LogWarning("Unknown weapon type: " + weponChange);
                break;
        }
        yield return new WaitForSeconds(0.45f);
        b_Knight_direct = false;
        b_ShotBool = false;
        b_Load = false;

        // 무기 교체 애니메이션 종료
        currentWeaponType = weaponType;
        animator.SetBool("WeponChangeBool", false);
        isSwitching = false;
        Debug.Log("WeaponChange 코루틴 끝");
    }
    public void Load()//장전(애니메이션 클립)
    {
        StartCoroutine(LoadC());
    }

    IEnumerator LoadC()
    {
        randomArrow = Random.Range(0, 6);
        yield return null;
        g_Animation_bow_Load[randomArrow].SetActive(true);
    }
    public void Pull(int pull)
    {
        if (pull == 0)
        {
            b_ShotBool = false;
        }
        else
        {
            loadboolTime = 0;
            b_ShotBool = true;
        }
    }
    public void LoadFalse()
    {
        b_Load = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            attackBool = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            attackBool = false;
        }
    }
}
