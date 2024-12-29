using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BoneDragon : MonoBehaviour
{
    [Range(0f, 20000f)]
    public float boneDragonHp = 200000;
    public TextMeshProUGUI boneDragonHpUI;
    public GameObject activeUI;
    public Image hpbarImage;
    GameObject player;
    AttackAndCombo andCombo;
    public QuestManager questManager;

    public DemoCharacter demoCharacter;
    public Animator charAnim;
    public bool isAir = false;
    public bool isDying = false;
    public bool isDyingEnd = false;
    bool fireBreathBool;
    //public Vector3 airPos;
    public Vector3 groundPos;

    public GameObject[] breathParticles;
    public GameObject[] breathLights;

    public Material[] matLimbs;
    public Material[] matSpine;
    public SkinnedMeshRenderer mesh;
    public int colliderCount;
    public string animationName;
    public GameObject g_attackHitBox;
    public GameObject playerLooak;

    bool flyBool;//날고 있는가?(이 bool값이 false이 되도 애니메이션은 바뀌지 안는다)
    [Range(0f, 10f)]
    public float flyHight;//드래곤이 최종 위치까지 도착 하는 시간
    [Range(-20f, 20f)]
    public float flySpeed_X;//드레곤이 날때 이동하는 속도
    [Range(-20f, 20f)]
    public float flySpeed_Y;//드래곤이 날아서 위로 이동 하는 속도
    [Range(-20f, 20f)]
    public float flySpeed_Z;//드래곤이 날아서 이동 하는 속도
    public GameObject[] skillPrefab;//0:일반 메테오 1:추적 메테오  2:장판기 3:브레스
    public GameObject[] attackPos;
    public float skillSpeed = 2f;
    bool clearLookAt;  //플레이어 바라봐야 하는 상태인지 아닌지
    public bool exhaustion;// 탈진 상태
    public float exhaustionTime = 0f; //탈진 시간
    int attackLoopMax = 3; //돌진 공격 횟수
    float attackTime = 0;
    public GameObject g_Desh;

    [SerializeField]
    bool skill_tested; //스킬 시전중(탈진상태가 아닌 경우)
    bool showingSkills;
    [SerializeField]
    private bool testSkill;
    bool pattern;//패턴 중인때는 colliderCount의 영향을 안받음
    bool[] animationBool = { false, false };// 애니메이션이 실행중일때 다른 애니메이션 실행 방지
    int conut = 0;
    CircleR circleR;
    public GameObject ui;
    public GameObject boseClearPotal;
    public GameObject[] g_ClearItem;

    public AudioClip damageAudioClip;
    public AudioClip attackClip;
    public AudioClip stepSound;

    public Shild shild;
    void Start()
    {
        boneDragonHpUI.text = "200000HP";
        player = GameObject.Find("Player");
        andCombo = player.GetComponent<AttackAndCombo>();
        circleR = player.GetComponent<CircleR>();
        //드레곤 기본 위치 저장
        groundPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        charAnim = GetComponent<Animator>();
    }
    /*
        public void UpdateGroundLocomotion(float newValue) {
            charAnim.SetFloat("groundLocomotion", newValue);
        }
    */
    public void CallTrigger(string newValue)
    {
        charAnim.SetTrigger(newValue);
        /*        if (newValue == "goAir")
                    GoAir();
                if (newValue == "goGround")
                    GoGround();
                if (newValue == "flyDie")
                    isDying = true;
                else if (isDying)
                {
                    isDying = false;
                    isDyingEnd = false;
                    GoAir();
                }*/
        if (newValue == "fireBreath" || newValue == "flyFireBreath")
        {
            Invoke("StartBreath", 0.3f);
            Invoke("StopBreath", 2.5f);
        }
    }
    public void StartDeath()
    {
        isDying = true;
    }
    //플레이어 바라보기
    void LookAtPlayer(GameObject playerRotation)
    {
        Vector3 direction = playerRotation.transform.position - transform.position;
        direction.y = 90;
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        }
    }
    void Update()
    {

        if (testSkill)
        {
            //CallTrigger("flyFireBreath");
            //StartCoroutine(Skill2());
            //StartCoroutine(Skill3());
            //StartCoroutine(Skill5());
        }
        ThisHp();
        if (!exhaustion)
        {
            if (!clearLookAt)
            {
                LookAtPlayer(playerLooak);
            }

            if (colliderCount == 1 && !pattern)
            {
                StartCoroutine(Fly_Y_Corotin());
            }

            if (flyBool)
            {
                gameObject.transform.position = new Vector3(transform.position.x + Time.deltaTime * flySpeed_X,
                                                            transform.position.y + Time.deltaTime * flySpeed_Y,
                                                            transform.position.z + Time.deltaTime * flySpeed_Z);
            }
            if (!skill_tested)
            {
                Vector3 playerPosition = playerLooak.transform.position;
                float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
                //거리에 비래 해서 드레곤의 이동속도가 빨라짐(플레이이와 의 거리가 5.5이상일경우) * 0.3의 증가 률로 기본 속도 1로 시작해서 최대속도2까지 속도가 증가
                float speedFactor = Mathf.Clamp((distanceToPlayer - 5.5f) * 0.5f, 1f, 1.5f); // 최소 속도 1, 최대 속도 1.5
                // 플레이어와의 거리가 3.49f보다 가까우면 후퇴 애니메이션을 실행
                if (distanceToPlayer > 200)
                {
                    ui.gameObject.SetActive(false);
                    charAnim.SetFloat("groundLocomotion", 0);
                }
                else
                {
                    ui.gameObject.SetActive(true);
                }
                if (distanceToPlayer < 5f)
                {
                    DistanceToPlayerAttack(distanceToPlayer, 1);
                    float backwardSpeed = Mathf.Lerp(-1, -5, (3.5f - distanceToPlayer) / 3.5f);
                    charAnim.SetFloat("groundLocomotion", backwardSpeed);
                    attackTime += Time.deltaTime;
                }
                // 플레이어와의 거리가 3.5f ~ 5.5f 사이이면 멈추는 애니메이션을 실행
                else if (distanceToPlayer >= 5f && distanceToPlayer < 6f)
                {
                    clearLookAt = false;
                    DistanceToPlayerAttack(distanceToPlayer, 2);
                    charAnim.SetFloat("groundLocomotion", 0);
                    gameObject.layer = 0;
                    attackTime += Time.deltaTime;
                    Attack(attackTime, 1);
                }
                // 플레이어와의 거리가 5.5f보다 멀어지면 전진 애니메이션을 실행
                else if (distanceToPlayer >= 6f)
                {
                    attackTime += Time.deltaTime;
                    Attack(attackTime, 0);
                    charAnim.SetFloat("groundLocomotion", speedFactor);
                }
            }
        }
        else
        {
            charAnim.SetFloat("exhaustionFloat", 1.0f);

            exhaustionTime += Time.deltaTime;
            if (exhaustionTime >= 10)//탈진 시간 10초
            {
                exhaustion = false;
                charAnim.SetFloat("exhaustionFloat", -1.0f);
            }
        }
        /* if (isAir && isDying && !isDyingEnd && transform.position.y < 110f)
             DeathEnd();

         if (isDyingEnd)
         {
             float posY = transform.position.y;
             posY -= Time.deltaTime * 17.58f;
             transform.position = new Vector3(transform.position.x, posY, transform.position.z);
             if (transform.position.y < groundPos.y)
             {
                 transform.position = new Vector3(transform.position.x, groundPos.y, transform.position.z);
                 isDyingEnd	 = false;
                 isDying		= false;
             } 
         }*/
    }
    void ThisHp()
    {
        boneDragonHpUI.text = boneDragonHp.ToString() + "HP";
        hpbarImage.fillAmount = boneDragonHp / 20000f;
        Die();
    }
    void Die()
    {
        if (boneDragonHp < 0 && gameObject.activeSelf)
        {
            charAnim.SetTrigger("DieTrigger");
            activeUI.SetActive(false);
            boseClearPotal.SetActive(true);
            questManager.i_questMosterCount[5] = 0;
        }
    }
    public void DieClip()
    {
        gameObject.SetActive(false);
        GameObject item1 = Instantiate(g_ClearItem[0], gameObject.transform.position + new Vector3(1, 2, 0), Quaternion.identity);
        item1.name = g_ClearItem[0].name;
        GameObject item2 = Instantiate(g_ClearItem[1], gameObject.transform.position + new Vector3(-1, 2, 0), Quaternion.identity);
        item2.name = g_ClearItem[1].name;
        GameObject item3 = Instantiate(g_ClearItem[2], gameObject.transform.position + new Vector3(3, 2, 0), Quaternion.identity);
        item1.name = g_ClearItem[2].name;
        GameObject item4 = Instantiate(g_ClearItem[3], gameObject.transform.position + new Vector3(0, 2, 1), Quaternion.identity);
        item2.name = g_ClearItem[3].name;
        GameObject item5 = Instantiate(g_ClearItem[4], gameObject.transform.position + new Vector3(0, 2, -1), Quaternion.identity);
        item2.name = g_ClearItem[4].name;
        GameObject item6 = Instantiate(g_ClearItem[5], gameObject.transform.position + new Vector3(2, 2, -1), Quaternion.identity);
        item2.name = g_ClearItem[5].name;

        Vector3 randomForce1 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item1.GetComponent<Rigidbody>().AddForce(randomForce1);
        Vector3 randomForce2 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item2.GetComponent<Rigidbody>().AddForce(randomForce2);
        Vector3 randomForce3 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item3.GetComponent<Rigidbody>().AddForce(randomForce3);
        Vector3 randomForce4 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item4.GetComponent<Rigidbody>().AddForce(randomForce4);
        Vector3 randomForce5 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item5.GetComponent<Rigidbody>().AddForce(randomForce5);
        Vector3 randomForce6 = new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), Random.Range(-1f, 1f)) * 300f;
        item6.GetComponent<Rigidbody>().AddForce(randomForce6);
    }
    public void ActiveGameObject()
    {
        gameObject.SetActive(false);
    }
    void Attack(float at, int type)
    {
        if (attackTime >= 3f && type == 1)
        {
            charAnim.SetTrigger("attack1"); gameObject.layer = 0;
            SoundManager.PlaySound(attackClip);
            attackTime = 0;
        }
        else if (attackTime >= 3f && type == 2)
        {
            charAnim.SetTrigger("attack2"); gameObject.layer = 10;
            attackTime = 0;
        }
    }
    void DistanceToPlayerAttack(float dis, int disT)
    {
        if (disT == 1)
        {
            //animationBool[0~1]0번이 참이될경우 공격2번 애니메이션이 실행중이기 때문에 공격1번 애니메이션은 실행불가 1번이 참인경우에도 같은 방식 사용
            if (dis > 0f && dis < 3.5f && !animationBool[1])//플레이와의 거리가 0f~2f인경우 초근접(발차기 공격)
            {
                Attack(attackTime, 2);
                animationBool[0] = true;
                clearLookAt = true;
            }
            else if (dis < 3.5f && !animationBool[0])
            {
                Attack(attackTime, 1);  //플레이어의 거리가 2f~3.5f인 경우 근접(깨물기 공격)
                animationBool[1] = true;
            }
        }
        else if (disT == 2)
        {
            if (dis >= 3.5f && dis < 4.5f && !animationBool[1])//플레이와의 거리가 0f~2f인경우 초근접(발차기 공격)
            {
                Attack(attackTime, 2);
                animationBool[0] = true;
                clearLookAt = true;
            }
            else if (dis < 4.5f && !animationBool[0])
            {
                Attack(attackTime, 1);  //플레이어의 거리가 2f~3.5f인 경우 근접(깨물기 공격)
                animationBool[1] = true;
            }
        }
    }
    //드레곤 Y위치 변경(날기)
    IEnumerator Fly_Y_Corotin()
    {

        pattern = true; //패턴 중인때는 colliderCount의 영향을 안받음
        skill_tested = true;
        flySpeed_Y = 10f;
        flyHight = 3f;
        flyBool = true;
        colliderCount = 2;
        charAnim.SetBool("goAir", true);
        charAnim.SetFloat("Air Locomotion", 0);
        yield return new WaitForSeconds(flyHight);
        charAnim.SetFloat("groundLocomotion", 0);
        flyBool = false;
        StartCoroutine(Fly_Skill_Meteo());
    }
    //나는 도중 원거리 스킬(메태오)
    IEnumerator Fly_Skill_Meteo()
    {
        skillSpeed = 2;

        for (int attackCount = 0; attackCount <= 20; attackCount++)
        {
            switch (attackCount)
            {
                case 1:
                    GameObject meteo1 = Instantiate(skillPrefab[3], attackPos[1].transform.position, Quaternion.identity);
                    break;
                case 4:
                    GameObject meteo2 = Instantiate(skillPrefab[3], attackPos[2].transform.position, Quaternion.identity);
                    break;
                case 8:
                    GameObject meteo3 = Instantiate(skillPrefab[4], attackPos[3].transform.position, Quaternion.identity);
                    break;
                case 12:
                    GameObject meteo4 = Instantiate(skillPrefab[5], attackPos[4].transform.position, Quaternion.identity);
                    break;
                case 16:
                    GameObject meteo5 = Instantiate(skillPrefab[6], attackPos[5].transform.position, Quaternion.identity);
                    break;
                case 19:
                    GameObject meteo6 = Instantiate(skillPrefab[7], attackPos[5].transform.position, Quaternion.identity);
                    break;
            }
            if (attackCount <= 19)
            {
                GameObject meteo0 = Instantiate(skillPrefab[0], attackPos[0].transform.position, Quaternion.identity);
                skillSpeed -= 0.1f;
                yield return new WaitForSeconds(skillSpeed);
            }
            else if (attackCount == 20)
            {
                StartCoroutine(DescendingAttack());
            }
        }
    }
    //flyFireBreath
    //드레곤 돌진 공격
    IEnumerator DescendingAttack()
    {
        clearLookAt = true;
        charAnim.SetFloat("Air Locomotion", 1);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(60, 0, 0));
        flyBool = true;
        flySpeed_Y -= 40;
        flyHight = 0.9947f;
        yield return new WaitForSeconds(flyHight);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        flyBool = false;
        clearLookAt = false;
        yield return null;
        //attackLoopMax: 돌진 을 몇번 할것인가
        for (int attackLoop = 0; attackLoop <= attackLoopMax; attackLoop++)
        {
            g_Desh.gameObject.SetActive(true);
            clearLookAt = true;
            // 플레이어를 향해 이동 (이동하면서 플레이어와 충돌할 가능성이 있음)
            gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerLooak.transform.position.x, playerLooak.transform.position.y, playerLooak.transform.position.z), 30 * Time.deltaTime);

            // 공격 애니메이션 실행
            CallTrigger("flyFireBreath");
            yield return new WaitForSeconds(4);

            // 꼬리 공격 애니메이션 실행
            charAnim.SetTrigger("tailWhip1");
            yield return new WaitForSeconds(2);
            charAnim.SetFloat("groundLocomotion", -1);
            clearLookAt = false;
            yield return new WaitForSeconds(1);

        }
        g_Desh.gameObject.SetActive(false);
        clearLookAt = false;
        exhaustion = true;
        skill_tested = false;
    }
    //범위 스킬(장판기1:얼음 가시가 올라옴) 
    IEnumerator Skill2()
    {
        player.transform.position += Vector3.forward * 50;
        demoCharacter.slowBool = true;
        Debug.Log("skill2");
        exhaustionTime = 0;
        exhaustion = false;
        int skill2Count = 0;
        yield return null;
        testSkill = false;
        for (skill2Count = 0; skill2Count < 12; skill2Count++)
        {
            yield return new WaitForSeconds(2);
            GameObject skill2 = Instantiate(skillPrefab[2]);
            Destroy(skill2, 10);
        }
        demoCharacter.slowBool = false;
        StartCoroutine(Skill3());
    }
    //범위 스킬(장판기2:얼음 칼이 떨어짐) 
    IEnumerator Skill3()
    {
        int skill3Count = 0;
        yield return null;
        testSkill = false;
        for (skill3Count = 0; skill3Count < 5; skill3Count++)
        {
            yield return new WaitForSeconds(1);
            GameObject skill3 = Instantiate(skillPrefab[8]);
            yield return new WaitForSeconds(4);
            Destroy(skill3, 10);
        }
        StartCoroutine(Skill4());
    }
    //브레스
    IEnumerator Skill4()
    {
        StartBreath();
        yield return new WaitForSeconds(5);
        StopBreath();
        clearLookAt = false;
        pattern = false;
    }
    IEnumerator Skill5()
    {
        for (int i = 0; i < 4; i++)
        {
            testSkill = false;//테스트 끝나면 지워도됨
            circleR.b_circleR = true;
            circleR.f_randonDeg = Random.Range(180, 360);
            yield return new WaitForSeconds(5);

            Debug.Log("돌진");
            clearLookAt = true;
            // 플레이어를 향해 이동 (이동하면서 플레이어와 충돌할 가능성이 있음)
            gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerLooak.transform.position.x, playerLooak.transform.position.y, playerLooak.transform.position.z), 100 * Time.deltaTime);
            clearLookAt = false;
            yield return new WaitForSeconds(1);
        }
    }
    /*public void SetAir(){
        isAir		= true;
        isDying		= false;
        isDyingEnd	= false;
    }

    public void SetDying(){
        isDying		= true;
    }

    public void GoAir(){

        transform.position	= groundPos + new Vector3(0f,15f,0f);
        isAir		= true;
        isDying		= false;
        isDyingEnd	= false;
    }

    public void GoGround(){
        transform.position	= groundPos;
        isAir		= false;
        isDying		= false;
        isDyingEnd	= false;
    }

    public void DeathEnd(){
        charAnim.SetTrigger("flyDieEnd");
        isDyingEnd	= true;
    }
    */
    //브레스 시작
    public void StartBreath()
    {
        for (int l = 0; l < breathLights.Length; l++)
        {
            breathLights[l].SetActive(true);
        }
        for (int p = 0; p < breathParticles.Length; p++)
        {
            breathParticles[p].GetComponent<ParticleSystem>().Play();
        }
    }
    //브레스 멈춤
    public void StopBreath()
    {
        for (int l = 0; l < breathLights.Length; l++)
        {
            breathLights[l].SetActive(false);
        }
        for (int p = 0; p < breathParticles.Length; p++)
        {
            breathParticles[p].GetComponent<ParticleSystem>().Stop();
        }
    }

    public void SetMaterials(int id)
    {
        Material[] materials = mesh.materials;
        materials[0] = matLimbs[id];
        materials[1] = matSpine[id];
        mesh.materials = materials;
    }
    public void AnimationBool(int aniInt)
    {
        animationBool[aniInt] = false;
    }
    public void AttackBool(int attint)
    {
        if (attint == 0)
        {
            g_attackHitBox.SetActive(true);
        }
        else
        {
            g_attackHitBox.SetActive(false);
        }
    }
    public void ClearLookAt()
    {
        clearLookAt = false;
    }
    public void AnimatorSkill2()
    {
        Invoke("InvokeSkill2", 10);
    }
    void InvokeSkill2()
    {
        StartCoroutine(Skill2());
    }
    public void StepSound()
    {
        SoundManager.PlaySound(stepSound);
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("R"))
        {
            boneDragonHp -= 5;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HandAttack"))
        {
            SoundManager.PlaySound(damageAudioClip);
            conut++;
            if (conut >= 5) { colliderCount = 1; conut = 0; }

            boneDragonHpUI.text = "" + boneDragonHp + "HP";
            //AttackAndCombo andCombo = other.GetComponent<AttackAndCombo>();
            switch (AttackAndCombo.comboAttakccount)
            {
                case 0:
                    boneDragonHp -= andCombo.attackDamage[0];
                    break;
                case 1:
                    boneDragonHp -= andCombo.attackDamage[1];
                    break;
                case 2:
                    boneDragonHp -= andCombo.attackDamage[2];
                    break;
                case 3:
                    boneDragonHp -= andCombo.attackDamage[3];
                    break;
            }
        }
        if (other.gameObject.CompareTag("ShildAttack"))
        {
            boneDragonHp -= shild.damageHap;
        }
    }
}

