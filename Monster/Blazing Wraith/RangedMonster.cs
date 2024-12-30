using System.Collections;
using Unity.AI.Navigation.Samples;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class RangedMonster : MonoBehaviour
{
    public Image hpSprite;
    public float f_Hp = 500;
    private NavMeshAgent agent;
    public Animator animator;
    AgentLinkMover linkMover;
    GameObject player;
    public AttackAndCombo attack;
    public GameObject g_ball;
    public GameObject g_attackFire;
    public GameObject g_oil;
    public GameObject g_Launch;
    public bool ballAttack;
    public bool attackCoroutin;
    public bool launchAttackCoroutin;
    //Demon_damaged의 램덤 공격
    public int randomAttack = 2;
    AgentLinkMover agentLink;
    public bool b_isAttack;
    public Canvas ui;
    public static bool lookat;
    MonsterAgents agents;
    public QuestManager questManager;
    public AudioClip damageSound;
    public AudioClip[] attackSound;

    //플레이어에 의한 상태 이상
    public bool b_Freeze;
    public float f_FreezeTime;
    public GameObject g_FreezePaticle;
    public Skill skill;
    public Shild shild;
    private void Start()
    {
        lookat = true;
        agents = GetComponentInParent<MonsterAgents>();
        animator.SetLayerWeight(3, 0f);
        agentLink = GetComponent<AgentLinkMover>();

    }
    private void Awake()
    {
        player = GameObject.Find("Player");
        attack = player.GetComponent<AttackAndCombo>();
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
        linkMover = GetComponentInParent<AgentLinkMover>();
    }


    void Update()
    {
        if (!b_Freeze)
        {
            Jump();
            LookAtPlayer(player);
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
            f_FreezeTime += Time.deltaTime;
            agents.agent.speed = 0f;
            g_FreezePaticle.gameObject.SetActive(true);
            //스킬이 업그레이드 되기전에는 2초 + 강화 될때마다 1초씩증가
            if (f_FreezeTime >= 2 + skill.skillEnhanceCount[3])
            {
                g_FreezePaticle.gameObject.SetActive(false);
                agents.agent.speed = 3f;
                f_FreezeTime = 0;
                b_Freeze = false;
            }
        }

        HP();

        g_ball.transform.LookAt(transform.position);
    }
    void HP()
    {
        hpSprite.fillAmount = f_Hp / 700;

    }

    public void QuestCount()
    {
        questManager.i_questMosterCount[2]--;
    }
    /*******************************************점프*******************************************/
    void Jump()
    {
        if (agent.isOnOffMeshLink && !b_isAttack)
        {
            JumpLooAt();
            animator.SetBool("JumpBool", true);
            animator.SetLayerWeight(1, 1f);
        }
        else
        {
            Invoke("InvokeJump", 0.2f);
        }
    }
    public void JumpAnimatorClip()
    {
        StartCoroutine(linkMover.Curve(agent, 0.5f));
    }
    void JumpLooAt()
    {
        OffMeshLinkData linkData = agent.currentOffMeshLinkData;
        Vector3 startPos = linkData.startPos;
        Vector3 endPos = linkData.endPos;

        // 이동할 방향 계산
        Vector3 direction = (endPos - startPos).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }
    void InvokeJump()
    {
        animator.SetBool("JumpBool", false);
        animator.SetLayerWeight(1, 0f);
    }
    /*******************************************점프*******************************************/
    void LookAtPlayer(GameObject playerRotation)
    {
        Vector3 direction = playerRotation.transform.position - transform.position;
        direction.y = 90;
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        }
    }


    public void RandomAttack()
    {
        if (randomAttack == 1)
        {
            if (!attackCoroutin)
            {
                StartCoroutine(Attack());
                agent.stoppingDistance = 15;
            }
        }
        else if (randomAttack == 2)
        {
            if (!launchAttackCoroutin)
            {
                StartCoroutine(LaunchAttack());
                agent.stoppingDistance = 25;
            }
        }
    }
    public void Generate()
    {
        animator.SetLayerWeight(3, 0f);
    }
    public IEnumerator Attack()
    {
        b_isAttack = true;
        animator.SetLayerWeight(2, 1f);
        animator.SetLayerWeight(0, 0f);
        attackCoroutin = true;
        agent.isStopped = true;
        g_oil.gameObject.SetActive(true);
        animator.SetTrigger("AttackBallTrigger");
        yield return new WaitForSeconds(1);
        g_oil.gameObject.SetActive(false);
        yield return new WaitForSeconds(10);
        g_attackFire.SetActive(false);
        animator.SetTrigger("CatchBallTrigger");
        g_oil.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        agent.isStopped = false;
        g_oil.gameObject.SetActive(false);
        animator.SetLayerWeight(2, 0f);
        animator.SetLayerWeight(0, 1f);
        yield return new WaitForSeconds(3);
        attackCoroutin = false;
        b_isAttack = false;
        randomAttack = Random.Range(1, 3);
    }
    public IEnumerator LaunchAttack()
    {
        b_isAttack = true;
        animator.SetLayerWeight(2, 1f);
        animator.SetLayerWeight(0, 0f);
        agent.isStopped = true;
        launchAttackCoroutin = true;
        animator.SetBool("LaunchBool", true);
        yield return new WaitForSeconds(4.84f);
        animator.SetBool("LaunchBool", false);
        agent.isStopped = false;
        animator.SetLayerWeight(2, 0f);
        animator.SetLayerWeight(0, 1f);
        yield return new WaitForSeconds(4f);
        launchAttackCoroutin = false;
        b_isAttack = false;
        launchBulletCount = 0;
        randomAttack = Random.Range(1, 3);
    }
    public void Fire(int tf)
    {
        if (tf == 1)
        {
            g_attackFire.SetActive(true);
        }
    }
    int launchBulletCount = 0;
    public void Launch(int launchBulletCounts)
    {
        launchBulletCount += 1;
        launchBulletCounts = launchBulletCount;
        Debug.Log(launchBulletCount);
        if (launchBulletCount <= 2)
        {
            if (launchBulletCounts == 1)
            {
                SoundManager.PlaySound(attackSound[0]);
                GameObject launchBullet = Instantiate(g_Launch, g_ball.transform.position + new Vector3(0, -0.4f, 0), Quaternion.identity);
                Destroy(launchBullet, 6);
                if (player.transform.position.y >= 21)
                {
                    Vector3 direction = player.transform.position - launchBullet.transform.position;
                    direction.y = 90;
                    {
                        Quaternion rotation = Quaternion.LookRotation(direction);
                        launchBullet.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                    }
                }
                else
                {
                    launchBullet.transform.LookAt(player.transform);
                }
            }
            else if (launchBulletCounts == 2)
            {
                SoundManager.PlaySound(attackSound[0]);
                GameObject launchBullet = Instantiate(g_Launch, g_ball.transform.position + new Vector3(0, -0.4f, 0), Quaternion.identity);
                Destroy(launchBullet, 10);
                if (player.transform.position.y >= 21)
                {
                    Vector3 direction = player.transform.position - launchBullet.transform.position;
                    direction.y = 90;
                    {
                        Quaternion rotation = Quaternion.LookRotation(direction);
                        launchBullet.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                    }
                }
                else
                {
                    launchBullet.transform.LookAt(player.transform);
                }
            }

        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("R"))
        {
            f_Hp -= 5;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("HandAttack") || other.gameObject.CompareTag("Desh"))
        {
            Debug.Log("Combo Attack Count: " + AttackAndCombo.comboAttakccount);
            SoundManager.PlaySound(damageSound);
            switch (AttackAndCombo.comboAttakccount)
            {
                case 0:
                    Debug.Log("공격1");
                    f_Hp -= attack.attackDamage[0];
                    break;
                case 1:
                    Debug.Log("공격2");
                    f_Hp -= attack.attackDamage[1];
                    break;
                case 2:
                    Debug.Log("공격3");
                    f_Hp -= attack.attackDamage[2];
                    break;
                case 3:
                    Debug.Log("공격4");
                    f_Hp -= attack.attackDamage[3];
                    break;
            }
        }
        if(other.gameObject.CompareTag("ShildAttack"))
        {
            f_Hp -= shild.damageHap;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("freeze"))
        {
            b_Freeze = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("freeze"))
        {
            b_Freeze = true;
        }
    }
}
