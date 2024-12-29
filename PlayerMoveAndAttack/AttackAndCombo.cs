using System.Collections;
using System.IO;//외부 파일 가져오기 json
using System.Linq;
using UnityEngine;
public class AttackAndCombo : MonoBehaviour
{
    private Animator animator;
    public QuestManager questManager;
    private HP playerHp;//플레이어 체력 가지고 오기
    public PlayerUI playerUI;
    private PutitemBag putitemBag;
    Skill skill;

    public Vector3 savePos;
    public Vector3 v_DefultPos;
    //주먹,발차기 공격 및 공격 콤보 , 최종 스탯을 찍었을시 궁극기 스킬생성
    private bool attackBool;
    static public int comboAttakccount = 0;
    public int[] attackDamage = { 50, 70, 60, 80 };
    public int[] stackAttackDamage = { 0, 0, 0, 0 };
    public float[] attackSpeed = { 0, 0, 0, 0 };
    private bool kickBool;
    static public int comboKickCount = 0;
    public GameObject attackCollider;//공격 범위
    public SphereCollider attackSphereCollider;
    public float attackRange;
    public bool ultimateSkillunlock;
    public bool ultimateSkill;
    public float ultimateSkillCoolTime = 0;
    public float ultimateSkillCoolTimeMax = 20;
    public bool ultimateSkillCoroutin = true;
    //타격감
    private float feelinghitting;
    //공격 속도 증가,기력 소비감소
    public float skillattackSpeed = 0;
    public float staminaReductionRate = 0;
    //회피 밎 막기,반격
    public float defend = 1;
    public float stackdefend = 1;
    private bool rollingBool;
    private float rollingCoolTime = 0f;
    private float JumpTime = 0f;
    public bool invincibilityBool;//무적 인지 아닌지
    public bool block;
    public float blockCoolTime = 0;
    public bool counterattack;
    public GameObject counterAttackGameObject;//counterAttackGameObject에 맞는 Prefab넣기
    //점프 밎 점프 공격
    Rigidbody rb;
    //private float junpAttackCoolTime = 0f;
    private bool jumpBool;
    // private bool junpAttackBool = false;
    public bool hitFlyBackBool;
    public bool cc; //상태이상
    //좌우,뒤 이동 애니메이션 여부 확인
    private bool backWalkadnRunbool;
    private bool rightWalkandRun;
    private bool leftWalkandRun;


    public CharacterController charControl;
    private Vector3 velocity;

    //땅인지 여부
    bool isGround;

    //적 보스 찾기 
    GameObject darkKnightGameObject;
    Enemy enemy;
    GameObject undedHouse;
    //기본 컨트롤
    DemoCharacter demoCharacter;

    //체력 관리
    Stamina stamina;
    public bool potionDrinkBool;//포션 먹는 애니메이션
    Potion potion;
    bool[] postionType = { false, false };

    //스킬 업그레이드 
    public SkillTree skillTree;
    //무적 시간
    public bool InvincibleTime;

    SkillItemFusion skillItemFusion;

    public float hpPostionCoolTime;
    public float staminaPostionCoolTime;
    public GameObject startMenu;

    UndeadKnight undeadKnight;

    //퀘스트(원흉과의 거리)
    public GameObject g_CurseSource;
    public BoxCollider box_CurseSourcePotal;
    private void Awake()
    {
        skillItemFusion = GetComponent<SkillItemFusion>();
        rb = GetComponent<Rigidbody>();
        putitemBag = GetComponent<PutitemBag>();
        skill = GetComponent<Skill>();
    }

    void Start()
    {
        Debug.Log("저장된 아이템이 있는지 확인");
        gameObject.transform.position = savePos;
        string filePath = DataManager.instance.path + DataManager.instance.nowSlot.ToString();
        if (File.Exists(filePath))
        {
            // 데이터가 있으면 데이터 로드 및 코루틴 실행
            StartCoroutine(SavePosition());
        }
        else
        {
            gameObject.transform.position = v_DefultPos;
        }
        playerHp = GetComponent<HP>();
        charControl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        demoCharacter = GetComponent<DemoCharacter>();
        stamina = GetComponent<Stamina>();
        potion = GetComponent<Potion>();
    }

    void Update()
    {
        if (DataManager.instance.nowPlayer != null)
        {
            savePos = gameObject.transform.localPosition;
            DataManager.instance.nowPlayer.plyerPosition = savePos;
        }
        if (!startMenu.activeSelf)
        {
            if (!questManager.conversationStatus)
            {
                if (!playerUI.uiOpen)
                {
                    IsGround();
                    KeyControll();
                    AnimatorControll();
                    StartCoroutine(Feelinghitting());
                    darkKnightGameObject = GameObject.Find("DarkKnight");
                }
                if (darkKnightGameObject != null)
                {
                    enemy = darkKnightGameObject.GetComponent<Enemy>();
                }
            }
            AttackDamageItem();

            AttackSpeed(skillTree.skillattackSpeed + skill.skillAttackSpeed);


            AttackRange(skillTree.attackRange);
        }
        if (questManager.questBool[3])
        {
            CurseSourceDistence();
        }
        else if (questManager.questBool[4])
        {
            box_CurseSourcePotal.enabled = true;
        }

    }
    IEnumerator SavePosition()
    {
        Debug.Log("플레이어 저장위치");
        charControl.enabled = false;
        yield return null;
        gameObject.transform.localPosition = DataManager.instance.nowPlayer.plyerPosition;
        yield return null;
        charControl.enabled = true;
    }
    void CurseSourceDistence()
    {
        float dis = Vector3.Distance(transform.position, g_CurseSource.transform.position);
        if (dis <= 20)
        {
            questManager.i_questMosterCount[3] = 0;
        }
    }

    void AttackDamageItem()
    {
        attackDamage[0] = 50 + skillItemFusion.attackDamageHap.Take(6).Sum() + stackAttackDamage[0];
        attackDamage[1] = 70 + skillItemFusion.attackDamageHap.Take(6).Sum() + stackAttackDamage[1];
        attackDamage[2] = 60 + skillItemFusion.attackDamageHap.Take(6).Sum() + stackAttackDamage[2];
        attackDamage[3] = 80 + skillItemFusion.attackDamageHap.Take(6).Sum() + stackAttackDamage[3];
    }

    public void AttackSpeed(float attspeed)
    {
        
        attackSpeed[0] = skillItemFusion.attackSpeedHap.Take(6).Sum();
        attackSpeed[1] = skillItemFusion.attackSpeedHap.Take(6).Sum();
        attackSpeed[2] = skillItemFusion.attackSpeedHap.Take(6).Sum();
        attackSpeed[3] = skillItemFusion.attackSpeedHap.Take(6).Sum();

        animator.SetFloat("AttackSpeed1", 1f + ((attspeed + attackSpeed[0]) / 100));
        animator.SetFloat("AttackSpeed2", 1f + ((attspeed + attackSpeed[1]) / 100));
        animator.SetFloat("AttackSpeed3", 1f + ((attspeed + attackSpeed[2]) / 100));
        animator.SetFloat("AttackSpeed4", 1f + ((attspeed + attackSpeed[3]) / 100));
    }
    void AttackRange(float range)
    {
        attackSphereCollider.radius = 0.65f + range;
    }
    void KeyControll()
    {
        //주먹 공격
        if (Input.GetMouseButtonDown(0) && isGround && !demoCharacter.isInWater) attackBool = true;
        else attackBool = false;


        /*        //발차기 공격
                if (Input.GetMouseButtonDown(1) && isGround) kickBool = true;
                else kickBool = false;*/

        //구르기
        if (Input.GetKeyDown(KeyCode.E) && rollingCoolTime > 3f - skillTree.upgradeReduceCoolTime && stamina.stamina >= 100 && !demoCharacter.isInWater)
        {
            rollingBool = true;
            rollingCoolTime = 0;
            invincibilityBool = true;
            Debug.Log("기력 소모:" + (100f - skillTree.staminaReductionRate));
            stamina.stamina -= 100f - skillTree.staminaReductionRate;
        }
        else
        {
            if (rollingCoolTime <= 3) rollingCoolTime += Time.deltaTime;
        }

        //점프
        if (Input.GetKey(KeyCode.Space) && JumpTime > 2f && isGround && !questManager.conversationStatus)
        {
            jumpBool = true;
            JumpTime = 0;
            rb.AddForce(Vector3.up * 3, ForceMode.Impulse);
        }
        else
        {
            jumpBool = false;
            if (JumpTime <= 2) JumpTime += Time.deltaTime;
        }

        //뒤로 이동
        if (Input.GetKey(KeyCode.S))
        {
            backWalkadnRunbool = true;
        }
        else { backWalkadnRunbool = false; }
        //오른쪽으로 이동
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            rightWalkandRun = true;
        }
        else { rightWalkandRun = false; }
        //왼쪽으로 이동
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) { leftWalkandRun = true; }
        else { leftWalkandRun = false; }

        /*        //막기
                if (Input.GetKey(KeyCode.Q) && blockCoolTime > 4 - skillTree.upgradeReduceCoolTime && stamina.stamina >= 60)
                {
                    blockCoolTime = 0;
                    DemoCharacter.noneSpeed = true;
                    attackCollider.SetActive(true);
                    attackCollider.gameObject.tag = "Counter";
                    block = true;
                    invincibilityBool = true;
                    stamina.stamina -= 120f - skillTree.staminaReductionRate;
                }
                else
                {
                    attackCollider.gameObject.tag = "HandAttack";
                    if (blockCoolTime <= 4) blockCoolTime += Time.deltaTime;

                }*/
        //포션 먹기
        if (Input.GetKey(KeyCode.Alpha1) && potion.hpPotion >= 0 &&
            playerHp.hp < playerHp.hpMax && hpPostionCoolTime >= 3) //hp
        {
            if (putitemBag.itemCount[81] > 0)
            {
                ItemType itemType = putitemBag.ItemSlot[81].gameObject.GetComponentInChildren<ItemType>();
                playerHp.hp += 200 + potion.staminaPotion;
                itemType.get_itemcount -= 1;
            }
            hpPostionCoolTime = 0;
            postionType[0] = true;
            potionDrinkBool = true;
        }
        else
        {
            if (hpPostionCoolTime <= 3.1f) hpPostionCoolTime += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Alpha2) && potion.staminaPotion >= 0 &&
            stamina.stamina < stamina.staminaMax && staminaPostionCoolTime >= 3)//stamina
        {
            if (putitemBag.itemCount[82] > 0)
            {
                ItemType itemType = putitemBag.ItemSlot[82].gameObject.GetComponentInChildren<ItemType>();
                itemType.get_itemcount -= 1;
            }
            staminaPostionCoolTime = 0;
            postionType[1] = true;
            potionDrinkBool = true;
        }
        else
        {
            if (staminaPostionCoolTime <= 3.1f) staminaPostionCoolTime += Time.deltaTime;
        }
        //궁극기
        if (Input.GetKey(KeyCode.R) && ultimateSkillunlock && ultimateSkillCoolTime >= ultimateSkillCoolTimeMax - skillTree.upgradeReduceCoolTime)
        {
            if (ultimateSkillCoroutin)
            {
                ultimateSkillCoolTime = 0;
                DemoCharacter.noneSpeed = true;
                stamina.stamina -= 300f - skillTree.staminaReductionRate;
                StartCoroutine(UltimateSkill());
            }
        }
        else
        {
            if (ultimateSkillCoolTime <= ultimateSkillCoolTimeMax) ultimateSkillCoolTime += Time.deltaTime;
        }
    }

    IEnumerator UltimateSkill()
    {

        invincibilityBool = true;
        ultimateSkillCoroutin = false;
        counterAttackGameObject.gameObject.SetActive(true);
        animator.SetLayerWeight(3, 1f);
        animator.SetTrigger("isR");
        yield return new WaitForSeconds(4);
        DemoCharacter.noneSpeed = false;
        animator.SetLayerWeight(3, 0f);
        counterAttackGameObject.gameObject.SetActive(false);
        ultimateSkillCoroutin = true;
        invincibilityBool = false;
    }
    void IsGround()
    {
        if (demoCharacter.isInWater)
        {
            isGround = true;
        }
        else
        {
            if (charControl.isGrounded)
            {
                isGround = true;
            }
            else { isGround = false; }
        }

    }
    public void invincibilityFalse()
    {
        invincibilityBool = false;
        rollingBool = false;
    }
    void AnimatorControll()
    {
        animator.SetBool("AttackBool", attackBool);                 //주먹기본 공격
        animator.SetInteger("AttackInt", comboAttakccount);         //주먹콤보 공격
        animator.SetBool("KickBool", kickBool);                     //다리 공격
        animator.SetInteger("kinkInt", comboKickCount);             //다리콤보 공격
        animator.SetBool("JumpBool", jumpBool);
        // animator.SetBool("JumpAttack", junpAttackBool);          //점프 공격;
        animator.SetBool("RollingBool", rollingBool);               //구르기
        animator.SetBool("BlockBool", block);
        animator.SetBool("PlayerCounterBreathBool", counterattack); //카운터 어택
        animator.SetBool("PotionDrinkBool", potionDrinkBool);       //포션 먹기

        animator.SetBool("backRun Bool", backWalkadnRunbool);       //뒤로 걷기
        animator.SetBool("rightRun Bool", rightWalkandRun);         //오른쪽으로 걷기
        animator.SetBool("leftRun Bool", leftWalkandRun);           //왼쪽으로 걷기

        animator.SetBool("HitFlyBack Bool", hitFlyBackBool);        //특정 공격을 맞아을때 뒤로 날아가는 히트 애니메이션
    }
    IEnumerator Feelinghitting()
    {
        if (counterattack)
        {
            Time.timeScale = 0.4f;
            yield return new WaitForSecondsRealtime(1);
            Time.timeScale = 1;
        }
        if (rollingBool)
        {
            gameObject.layer = 0;
            invincibilityBool = true;
            yield return new WaitForSecondsRealtime(1);
            invincibilityBool = false;
            gameObject.layer = 8;
        }
    }
    public void AttackCollider()
    {
        attackCollider.gameObject.SetActive(true);
    }
    public void CountAttackFire()
    {
        counterAttackGameObject.gameObject.SetActive(true);
    }

    public void DeAttackCollider()
    {
        attackCollider.gameObject.SetActive(false);
    }
    public void Deblock()
    {
        DemoCharacter.noneSpeed = false;
        block = false;
        invincibilityBool = false;
        attackCollider.SetActive(false);
        attackCollider.gameObject.tag = "HandAttack";
    }

    public void Postion()
    {
        if (postionType[0])
        {
            potion.HpPotion();
        }
        else if (postionType[1])
        {
            potion.StaminaPotion();
        }
    }

    public void DeCountAttackFire()
    {
        counterAttackGameObject.gameObject.SetActive(false);

        block = false;
        counterattack = false;
        DemoCharacter.noneSpeed = false;
    }
    public void DePotionDrinkBool()
    {
        potionDrinkBool = false;

        if (postionType[0] && potion.hpPotion != 0)
        {
            potion.hpPotion -= 1;
            postionType[0] = false;
        }
        else if (postionType[1] && potion.staminaPotion != 0)
        {
            potion.staminaPotion -= 1;
            postionType[1] = false;
        }

    }
    public void HitFlyBackBool(int moveInt)
    {

        if (moveInt == 1) cc = true;
        else if (moveInt == 2) { Invoke("DemoCharacterenabled", 0.1f); }
    }
    void DemoCharacterenabled()
    {
        hitFlyBackBool = false;

        Debug.Log("demoCharacter");
        cc = false;
    }



    IEnumerator knoback(int i, GameObject undedHouse) // i: 0 -> 앞으로 밀쳐짐, 1 -> 뒤로 밀쳐짐
    {
        // Debug.Log("넉백:" + knoback(i));
        hitFlyBackBool = true;
        demoCharacter.enabled = false; // 캐릭터 컨트롤 비활성화
        charControl.enabled = false;

        Vector3 startPosition = transform.position; // 시작 위치
        Vector3 targetPosition;

        // 적과 플레이어 사이의 방향
        Vector3 directionToPlayer = (undedHouse.transform.position - transform.position).normalized; // 플레이어와 적 사이의 방향 벡터

        // 넉백 방향 설정
        Vector3 knockbackDirection;

        // i 값에 따라 넉백 방향 설정 (0: 앞으로, 1: 뒤로)
        if (i == 0) // 앞발차기
        {
            knockbackDirection = directionToPlayer; // 플레이어를 향해 밀쳐짐 (앞으로)
        }
        else if (i == 1) // 뒷발차기
        {
            knockbackDirection = -directionToPlayer; // 플레이어 반대 방향으로 밀쳐짐 (뒤로)
        }
        else
        {
            yield break; // 잘못된 인덱스인 경우 코루틴 종료
        }

        // 목표 위치 계산 (거리를 3으로 나눠 1/3 만큼 밀쳐짐)
        float knockbackDistance = 60 / 3f;
        targetPosition = startPosition + knockbackDirection * knockbackDistance;

        float elapsedTime = 0f; // 경과 시간 초기화
        float duration = 0.35f; // 이동 시간 설정

        // Lerp를 사용하여 0.35초 동안 부드럽게 이동
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // 경과 시간 증가
            yield return null; // 다음 프레임까지 대기
        }

        transform.position = targetPosition; // 최종 위치 설정
        demoCharacter.enabled = true; // 캐릭터 컨트롤 활성화
        charControl.enabled = true;
        hitFlyBackBool = false; // 넉백 상태 해제
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Flame"))
        {
            playerHp.hp -= (20 - (defend + stackdefend));
        }
        else if (other.gameObject.CompareTag("Flame2"))
        {
            playerHp.hp -= (1 - (defend + stackdefend));
        }
        else if (other.gameObject.CompareTag("Ice"))
        {
            playerHp.hp -= 200;
        }
        if (!InvincibleTime)
        {
            if (other.gameObject.CompareTag("Luncher") && !invincibilityBool)
            {
                playerHp.hp -= (200 - (defend + stackdefend));
                Destroy(other.gameObject);
            }
            for (int i = 21; i < 27; i++)
            {
                if (other.gameObject.layer == i && !invincibilityBool)
                {
                    //죽음의 기사 모르타누스 의 스킬은 고정 데미지(데미지 감소 효과를 받지 못함)
                    switch (other.gameObject.layer)
                    {
                        case 21:
                            Debug.Log("skill1");
                            playerHp.hp -= 10;
                            break;
                        case 22:
                            Debug.Log("skill2");
                            playerHp.hp -= 150;
                            break;
                        case 23:
                            Debug.Log("skill3");
                            playerHp.hp -= 150;
                            break;
                        case 24:
                            Debug.Log("skill4");
                            playerHp.hp -= 100;
                            break;
                        case 25:
                            Debug.Log("skill5");
                            playerHp.hp -= 50;
                            break;
                        case 26:
                            Debug.Log("skill6");
                            playerHp.hp -= 100;
                            break;
                    }
                }
            }

        }
    }
    float DefendItem()
    {
        return defend + skillItemFusion.defenseHap.Take(6).Sum();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!playerHp.isDie)
        {
            if (other.gameObject.CompareTag("Enemy") && !invincibilityBool)
            {
                Debug.Log("적에게 맞음");
                if (enemy.attackcout == 5 || enemy.attackcout == 9)
                {
                    playerHp.hp -= enemy.attackDamage - (DefendItem() + stackdefend);
                    hitFlyBackBool = true;
                    DemoCharacter.noneSpeed = true;
                }
                else
                {
                    playerHp.hp -= enemy.attackDamage - (DefendItem() + stackdefend);
                }
            }
            if (!invincibilityBool)
            {
                if (other.gameObject.CompareTag("Enemy") && invincibilityBool)
                {
                    demoCharacter.attackSound.PlayOneShot(demoCharacter.attackClip[11], 1.0f);
                    counterattack = true;
                    playerHp.hp -= 150 - (DefendItem() + stackdefend);
                }

                if (other.gameObject.layer == 10 && !invincibilityBool)
                {
                    Debug.Log("넉백");
                    hitFlyBackBool = true;
                    other.gameObject.layer = 0;
                    playerHp.hp -= 150 - (DefendItem() + stackdefend);
                    //gameObject.transform.position += Vector3.forward * 50;
                }

                if (other.gameObject.layer == 11 && !invincibilityBool)
                {
                    playerHp.hp -= 250 - (DefendItem() + stackdefend);
                    StartCoroutine(knoback(0, other.gameObject));
                }
                if (other.gameObject.layer == 12 && !invincibilityBool)
                {
                    playerHp.hp -= 100 - (DefendItem() + stackdefend);
                    StartCoroutine((knoback(1, other.gameObject)));
                }
                //얼음용
                if (other.gameObject.CompareTag("BoneDragon") && !invincibilityBool)
                {
                    playerHp.hp -= 50 - (DefendItem() + stackdefend); 
                    StartCoroutine(Feelinghitting());
                }
                if (other.gameObject.CompareTag("Desh"))
                {
                    hitFlyBackBool = true;
                    StartCoroutine((knoback(1, other.gameObject)));
                    playerHp.hp -= 200 - (DefendItem() + stackdefend);
                }

                //죽음의 기사
                if (other.gameObject.CompareTag("Arrow") && !invincibilityBool)
                {
                    Debug.Log("화살 맞음");
                    GameObject undead = GameObject.Find("Undead Horse");
                    UndedHouse undedHouse = undead.GetComponent<UndedHouse>();
                    undedHouse.f_undedHouseHp += 500;
                    undedHouse.f_MaxundedHouseHp = undedHouse.f_undedHouseHp;
                    playerHp.hp -= 100 - (DefendItem() + stackdefend);
                    Destroy(other.gameObject);
                }
                if (other.gameObject.CompareTag("Ax") && !invincibilityBool)
                {
                    GameObject undead = GameObject.Find("Undead Horse");
                    UndedHouse undedHouse = undead.GetComponent<UndedHouse>();
                    undedHouse.f_undedHouseHp += 500;
                    undedHouse.f_MaxundedHouseHp = undedHouse.f_undedHouseHp;
                    playerHp.hp -= 100 - (DefendItem() + stackdefend);
                    StartCoroutine((knoback(1, undead)));
                }

                if (other.gameObject.layer==30 && !invincibilityBool)
                {

                    GameObject undead = GameObject.Find("Undead Horse");
                    UndedHouse undedHouse = undead.GetComponent<UndedHouse>();
                    undedHouse.f_undedHouseHp += 500;
                    undedHouse.f_MaxundedHouseHp = undedHouse.f_undedHouseHp;
                    StartCoroutine((knoback(1, undead)));
                    playerHp.hp -= 40 - (DefendItem() + stackdefend);
                }
                //어둠의 망령
                if (other.gameObject.CompareTag("MonsterAttack") && !invincibilityBool)
                {
                    playerHp.hp -= 10 - (DefendItem() + stackdefend);
                }
            }
        }
    }
}
