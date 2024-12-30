using System.Collections;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public enum boseType { SkullWarrior, SkeletonDregon, Witch, Zombi };
    public boseType bose;
    public ZombiSpaw zombiSpaw;
    HP hpEnemy;

    public GameObject playerPos;//플레이어 위치 가지고 오기
    public GameObject playerAt3pos;

    Rigidbody body;
    Animator animator;
    bool runBool;
    bool discoveryBool = false;
    bool spawAnimator;
    bool backWalkBool;
    bool stabBool;

    public float[] speed;
    int speedType;
    [SerializeField]
    bool donMove = false; //0인경우 findPlayer실행 1일경우 실행불가

    bool hit;  //일반 공격일경우
    public int attackDamage;
    bool attackTimeBool = true;

    public GameObject spawPos;//DarkKnightSpawPos
    float[] attackLen = { 3f, 0.5f, 1.4f, 2.5f };
    int attLenType;
    GameObject player;
    Vector3 spawVector;
    float findPlayer;
    float findSpaw;

    //맞을경우
    public int hitType; //0일경우 주먹,1일경우 발차기 로만 공격가능2인경우는 모두 가능;
    public GameObject[] hitTypeGameObject;//공격 타입을 알려주는 게임오브젝트를 확성화하거나 비활성화
    int hitTypeMaxTime;

    [SerializeField]
    bool findPlayerBool;
    [SerializeField]
    float attackTime = 0;
    float[] attackTimeType = { 5f, 3f, 3.5f };
    public int attackcout;

    public GameObject attackSize;
    public GameObject shildGameObject;//공격 모션3 찌르기 대기 자세 중 쉴드
    [SerializeField]
    bool attackBool;
    float[] warringTime = { 5.3f, 3.5f };

    public AudioSource attackSound;
    public AudioSource warkeSound;
    public AudioClip[] attackClip;
    public AudioClip[] warkeClip;
    int warkTypeAudioCount;
    [SerializeField]
    bool boseSpaw;  //보스가 스폰 했을시 거리 상관 없이 플레이어 추적

    public HP hP;
    public GameObject hpBar;

    public GameObject g_ClearPotal;//보스가 처지 될경우 포탈방 으로 복귀 시키는 포탈
    public GameObject[] g_ClearItem;
    public GameObject attackParticle;

    //플레이어에 의한 상태 이상
    public bool b_Freeze;
    public float f_FreezeTime;
    public GameObject g_FreezePaticle;
    public Skill skill;
    public Shild shild;
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        attackSound = GetComponent<AudioSource>();
        hpEnemy = GetComponent<HP>();

        BasicSettings();
        if (bose != boseType.Zombi)
        {
            StartCoroutine(AttackType());
        }
    }

    void Update()
    {
        if (!b_Freeze)
        {
            EnemyAnimator(bose);
            spawEnemy();
            FindPlayer();
            warkeSoundAudiotrueFalse();
            Attack(bose);
            AttackDamage();
            SpawZombi();
        }
        else
        {
            animator.enabled = false;
            f_FreezeTime += Time.deltaTime;
            speed[2] = 0;
            g_FreezePaticle.gameObject.SetActive(true);
            //스킬이 업그레이드 되기전에는 2초 + 강화 될때마다 1초씩증가
            if (f_FreezeTime >= 2 + skill.skillEnhanceCount[3])
            {
                g_FreezePaticle.gameObject.SetActive(false);
                animator.enabled = true;
                speed[2] = 4;
                f_FreezeTime = 0;
                b_Freeze = false;
            }
        }
        if (bose != boseType.Zombi) Die();
        else { gameObject.SetActive(false); }
    }
    void BasicSettings()
    {

        if (bose == boseType.SkullWarrior)
        {
            attLenType = 0;
            speedType = 0;
        }
        else if (bose == boseType.Zombi)
        {
            hitType = Random.Range(0, 2);
            HitTypeGameObjectTF(hitType, true);
            attLenType = 2;
            speedType = 2;
        }
    }

    void Die()
    {
        if (hP.hp < 0)
        {
            animator.SetTrigger("DieTrigger");
            donMove = true;
        }
    }
    public void DieClip()
    {
        g_ClearPotal.SetActive(true);
        gameObject.SetActive(false);
        GameObject item1 = Instantiate(g_ClearItem[0], gameObject.transform.position + new Vector3(1, 2, 0), Quaternion.identity);
        item1.name = g_ClearItem[0].name;
        GameObject item2 = Instantiate(g_ClearItem[1], gameObject.transform.position + new Vector3(-1, 2, 0), Quaternion.identity);
        item2.name = g_ClearItem[1].name;
        GameObject item3 = Instantiate(g_ClearItem[2], gameObject.transform.position + new Vector3(3, 2, 0), Quaternion.identity);
        item1.name = g_ClearItem[2].name;
        GameObject item4 = Instantiate(g_ClearItem[3], gameObject.transform.position + new Vector3(-3, 2, 0), Quaternion.identity);
        item2.name = g_ClearItem[3].name;
        Vector3 randomForce1 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item1.GetComponent<Rigidbody>().AddForce(randomForce1);
        Vector3 randomForce2 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item2.GetComponent<Rigidbody>().AddForce(randomForce2);
        Vector3 randomForce3 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item3.GetComponent<Rigidbody>().AddForce(randomForce3);
        Vector3 randomForce4 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item4.GetComponent<Rigidbody>().AddForce(randomForce4);
    }
    //좀비 스폰 공격(좀비스폰 스크립트 비활성화 상태에서 200초 이상지나거나 피가 500이하 499이상일경우 활성화)
    void SpawZombi()
    {
        float spawTime = 0;
        spawTime += Time.deltaTime;
        if (spawTime > 200 || hpEnemy.hp < 500)
        {
            zombiSpaw.enabled = true;
            spawTime = 0;
        }

        if (zombiSpaw.spawEnd)
        {
            zombiSpaw.enabled = false;
        }

    }
    void HitTypeGameObjectTF(int _type, bool _tf)
    {
        hitTypeGameObject[_type].SetActive(_tf);
    }

    IEnumerator AttackType()
    {
        while (true)
        {
            hitType = Random.Range(0, 3);
            HitTypeGameObjectTF(hitType, true);
            hitTypeMaxTime = Random.Range(10, 30);
            yield return new WaitForSeconds(hitTypeMaxTime);
            for (int count = 0; count < 3; count++)
            {
                HitTypeGameObjectTF(count, false);
            }
        }
    }
    public void AttackPaticle(int i)
    {
        if (i == 0) attackParticle.gameObject.SetActive(false);
        if (i == 1) attackParticle.gameObject.SetActive(true);

    }
    void EnemyAnimator(boseType boseType)
    {
        if (boseType == boseType.SkullWarrior)
        {
            animator.SetBool("hitBool", hit);
            animator.SetBool("runBool", runBool);
            animator.SetBool("discovery", discoveryBool);
            animator.SetInteger("Attack", attackcout);
            animator.SetBool("AttackBool", attackBool);

            animator.SetBool("DarkKnightSummonsBool", spawAnimator);
            animator.SetBool("BackWalkBool", backWalkBool);
            animator.SetBool("Stab Bool", stabBool);
        }
        else if (boseType == boseType.Zombi)
        {
            animator.SetBool("RunBool", runBool);
            animator.SetBool("AttackBool", attackBool);
        }
    }
    void warkeSoundAudiotrueFalse()//걸음을 멈추면 warkeSound endabel false 걸으면 true;
    {
        if (runBool != true)
        {
            warkeSound.enabled = false;
        }
        else
        {
            warkeSound.enabled = true;
        }
    }

    void LookAtPlayer(GameObject playerRotation)
    {
        Vector3 direction = playerRotation.transform.position - transform.position;
        direction.y = 0;
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        }
    }
    void FindPlayer()
    {
        if (!donMove)
        {
            if (!spawAnimator)
            {
                if (!boseSpaw)//보스가 소환한 적이 아닐경우 스폰위치 저장
                {
                    spawVector = spawPos.transform.position;
                }
                player = GameObject.Find("Player");
                float findPlayer = Vector3.Distance(player.transform.position, transform.position);//플레이어가 있는 현재 거리 구하기
                float findSpaw = Vector3.Distance(spawVector, transform.position);//플레이어가 찾을 수 없는 거리일 경우 적이 스폰된 위치로 되돌갈때 필요한 거리 구하기 
                if (findPlayer > attackLen[attLenType])//플레이어와의 거리가 attackLen(3)보다 클경우 아래코드 실행   
                {
                    hpBar.gameObject.SetActive(true);
                    discoveryBool = false;
                    findPlayerBool = false;
                    if (findPlayer < 20f)//플레이어와의 거리가 15보다 작을경우 플레이어를 추적
                    {
                        runBool = true;
                        LookAtPlayer(player);//플레이어를 바라보도록함
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed[speedType] * Time.deltaTime);
                    }
                    else//그렇지 않을경우 스폰 위치로 되돌아감
                    {
                        runBool = true;
                        hpBar.gameObject.SetActive(false);
                        if (!boseSpaw && !stabBool)//좀비가 아닐경우에만 스폰위치로 돌아감
                        {
                            transform.position = Vector3.MoveTowards(transform.position, spawVector, speed[speedType] * Time.deltaTime);
                            LookAtPlayer(spawPos);
                        }
                        else//좀비일경우 한번 추적을 시작하면 계속 플레이어를 추적함
                        {
                            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed[speedType] * Time.deltaTime);
                            LookAtPlayer(player);//플레이어를 바라보도록함
                        }
                    }
                }
                else//플레이어와의 거리가 attackLen(3)보다 작을 경우 플레이어가 공격 범위 안에 들어온 상태가 되기때문에 공격 대기 애니메이션 이 활성화
                {
                    runBool = false;
                    findPlayerBool = true;
                    if (findPlayer < 2.5f)
                    {
                        backWalkBool = true;
                        // 현재 위치의 Y값을 고정하여 이동
                        Vector3 targetPosition = new Vector3(
                            transform.position.x, // 현재 X좌표
                            transform.position.y, // Y좌표 고정
                            transform.position.z
                        );

                        Vector3 direction = (transform.position - player.transform.position).normalized;
                        targetPosition += new Vector3(direction.x, 0, direction.z) * speed[4] * Time.deltaTime;

                        transform.position = targetPosition;
                    }
                    else if (findPlayer > 2.5f)
                    {
                        backWalkBool = false;
                        discoveryBool = true;
                    }
                    if (attackcout != 3) LookAtPlayer(player);//플레이어를 바라보도록함

                }
                if (findSpaw < attackLen[1])//스폰과 의 거리가 0일경우 달리는 애니메이션이 false됨
                {
                    runBool = false;
                }
            }
        }
    }
    public void AttackDamage()
    {
        if (bose == boseType.SkullWarrior)
        {
            switch (attackcout)
            {
                case 1:
                    attackDamage = 60;
                    break;
                case 2:
                    attackDamage = 50;
                    break;
                case 3:
                    attackDamage = 120;
                    break;
                case 4:
                    attackDamage = 60;
                    break;
                case 5:
                    attackDamage = 80;
                    break;
                case 6:
                    attackDamage = 50;
                    break;
                case 7:
                    attackDamage = 120;
                    break;
                case 8:
                    attackDamage = 120;
                    break;
                case 9:
                    attackDamage = 70;
                    break;
                case 10:
                    attackDamage = 120;
                    break;

            }
        }
    }
    void spawEnemy()
    {
        if (bose != boseType.Zombi)
        {
            if (zombiSpaw.spawBool)
            {
                speedType = 3;
                discoveryBool = false;
                spawAnimator = true;
                runBool = false;
                attackBool = false;
            }
            else
            {
                spawAnimator = false;
                attLenType = 0;
                speedType = 0;
            }
        }
    }
    private void Attack(boseType boseType)
    {

        if (boseType == boseType.SkullWarrior)
        {
            if (!spawAnimator)
            {
                attackTime += Time.deltaTime;
            }
            if (findPlayerBool)
            {
                if (attackcout >= 10)
                {
                    attackcout = 0;
                }
                if (attackcout == 3)
                {
                    //attackcout = 4; 
                    StartCoroutine(Attack3());
                }
                if (attackTime >= attackTimeType[0] && attackTimeBool)
                {
                    attackcout += 1;
                    attackBool = true;
                    attackTime = 0;
                }
            }
        }
        if (boseType == boseType.Zombi && findPlayerBool)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= attackTimeType[1])
            {
                attackBool = true;
                attackTime = 0;
            }
        }
    }
    IEnumerator Attack3()
    {
        attackTimeBool = false;
        shildGameObject.SetActive(true);
        stabBool = true;
        donMove = true;
        yield return new WaitForSeconds(3f);
        shildGameObject.SetActive(false);
        stabBool = false;
        animator.SetBool("AttackBool", true);
        yield return null;
        if (!attackTimeBool)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerAt3pos.transform.position, 20 * Time.deltaTime);
        }
        donMove = false;
        animator.SetBool("AttackBool", false);
        yield return new WaitForSeconds(1.5f);
        attackcout = 4;
        attackTimeBool = true;
        yield return null;
    }
    public void DontMove(int speed)
    {
        speedType = speed;
    }
    public void AttackAudio(int attackTypeAudio)
    {
        if (!spawAnimator)
        {
            attackSound.PlayOneShot(attackClip[0], 1.0f);
        }
        if (attackTypeAudio != 8)
        {
            attackSound.PlayOneShot(attackClip[attackTypeAudio], 1.0f);
        }
    }
    public void AttackAudio2(int attackTypeAudio)
    {
        attackSound.PlayOneShot(attackClip[attackTypeAudio], 1.0f);
    }
    public void WarkeAudio(int warkTypeAudio)
    {
        warkeSound.PlayOneShot(warkeClip[warkTypeAudio], 1.0f);
    }
    public void HitFalse()
    {
        hit = false;
    }
    public void AttackBool()
    {
        attackSize.SetActive(false);
        attackBool = false;
    }

    public void AttackGameObject()
    {
        attackSize.SetActive(true);
    }
    public void DeCounterHit()
    {
        animator.SetBool("CounterHit Bool", false);
    }
    public void WarkeAudio()
    {
        //걷는 효과음 
        warkTypeAudioCount += 1;
        if (warkTypeAudioCount == 20)
        {
            warkTypeAudioCount = 0;
        }
        warkeSound.PlayOneShot(warkeClip[warkTypeAudioCount], 1.0f);
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("R"))
        {
            hpEnemy.hp -= 5;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (attackcout != 3)
        {
            if (other.gameObject.CompareTag("HandAttack") && hitType == 0)
            {
                hit = true;
                hpEnemy.hp -= 100;
                attackSound.PlayOneShot(attackClip[5], 1.0f);
                if (AttackAndCombo.comboAttakccount == 3) { body.AddForce(Vector3.forward * 10, ForceMode.Impulse); }
                if (AttackAndCombo.comboAttakccount == 2) { body.AddForce(Vector3.up * 9, ForceMode.Impulse); }
            }
            else if (other.gameObject.CompareTag("FootAttack") && hitType == 1)
            {
                hit = true;
                hpEnemy.hp -= 100;
                attackSound.PlayOneShot(attackClip[5], 1.0f);
                if (AttackAndCombo.comboAttakccount == 3) { body.AddForce(Vector3.forward * 10, ForceMode.Impulse); }
                if (AttackAndCombo.comboAttakccount == 2) { body.AddForce(Vector3.up * 9, ForceMode.Impulse); }
            }
            else if (other.gameObject.CompareTag("HandAttack") || other.gameObject.CompareTag("FootAttack") && hitType == 2)
            {
                hit = true;
                hpEnemy.hp -= 100;
                attackSound.PlayOneShot(attackClip[5], 1.0f);
                if (AttackAndCombo.comboAttakccount == 3) { body.AddForce(Vector3.forward * 10, ForceMode.Impulse); }
                if (AttackAndCombo.comboAttakccount == 2) { body.AddForce(Vector3.up * 9, ForceMode.Impulse); }
            }
        }
        if (other.gameObject.CompareTag("ShildAttack"))
        {
            hpEnemy.hp -= shild.damageHap;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("freeze"))
        {
            b_Freeze = true;
        }
    }
}
