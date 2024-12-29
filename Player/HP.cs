using System.IO;//외부 파일 가져오기 json
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HP : MonoBehaviour
{
    public QuestManager questManager;
    public Select select;

    public Image hpBar;
    public TextMeshProUGUI mosterhp;
    public GameObject activeUI;
    public float hpMax = 1000;//상점 에서 스팩증가 스택권을 사면 최대 최력 증가 
    public float hp = 1000;
    public float hpRecovery = 0;
    public bool isMonster;

    public bool hpRegenerationBool;
    public TextMeshProUGUI[] saveHptext;
    SkillItemFusion skillItemFusion;
    public SkillTree skillTree;
    public PlayerUI playerUI;
    Animator animator;
    public Transform reStartPos;
    public GameObject player;
    DemoCharacter demoCharacter;
    CharacterController characterController;
    public bool isDie;
    public AudioClip dieSound;

    public GameObject map;
    public GameObject[] dunAndbose;

    private void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        demoCharacter = gameObject.GetComponent<DemoCharacter>();
        animator = GetComponent<Animator>();
        skillItemFusion = GetComponent<SkillItemFusion>();
        if (!isMonster)
        {
            // 데이터가 정상적으로 로드되었는지 확인
            string filePath = DataManager.instance.path + DataManager.instance.nowSlot.ToString();
            if (File.Exists(filePath))
            {
                Debug.Log("hp저장");
                hp = DataManager.instance.nowPlayer.hpData;
                hpMax = DataManager.instance.nowPlayer.hpMaxData;
            }
            else
            {
                hp = 1000;
                hpMax = 1000;
            }
        }
    }
    private void Update()
    {
        if (!isMonster)
        {
            if (hp <= 0)
            {

                GameLoad.gameLoad = true;
                animator.SetBool("DieBool", true);
                playerUI.playerDie.SetActive(true);
            }
            else
            {
                playerUI.playerDie.SetActive(false);
            }
            hpMax = 1000 + (skillItemFusion.hpHap.Take(6).Sum() + skillTree.hpStack);
            SkillHPRegeneration();
        }
        else
        {
            if (hp < 0)
            {
                activeUI.SetActive(false);
            }
            else
            {
                activeUI.SetActive(true);
            }
            hpBar.fillAmount = hp / 15000;
            mosterhp.text = hp.ToString() + "HP";
        }

        DataManager.instance.nowPlayer.hpData = hp;
        DataManager.instance.nowPlayer.hpMaxData = hpMax;


        hpBar.fillAmount = hp / hpMax;

        HPRegeneration(hpRegenerationBool);
        if (!isMonster)
        {
            curse();
        }

    }
    void curse()
    {
        if (playerUI.f_CurseProgression <= 0)
        {
            hp -= Time.deltaTime * 10;
        }
    }
    public void DieSound()
    {
        SoundManager.PlaySound(dieSound);
    }
    public void InvokeReStart()
    {
        map.SetActive(true);
        dunAndbose[0].SetActive(false);
        dunAndbose[1].SetActive(false);
        dunAndbose[2].SetActive(false);
        animator.SetBool("DieBool", false);
        DemoCharacter.noneSpeed = false;
        demoCharacter.enabled = true;
        characterController.enabled = false;
        player.transform.position = reStartPos.position;
        playerUI.f_CurseProgression = 5000f;
        playerUI.f_Maxf_CurseProgression = 5000f;
        transform.localScale = Vector3.one;
        hp = hpMax;
        SceneManager.LoadScene(1); // 게임씬으로 이동  
        isDie = false;

    }

    void SkillHPRegeneration()
    {
        if (hp < hpMax)//플레이어 스테미너가 maximumSteminerRecovery 보다 클경우 아래 코드 실행
        {
            if (skillItemFusion.healthRegenerationBool)
            {
                hp += (skillItemFusion.healthRegenerationHap.Take(6).Sum() + skillTree.hpRecoveryStack) * Time.deltaTime;
            }

        }
    }
    void HPRegeneration(bool _hpRegenerationBool)
    {
        if (_hpRegenerationBool)
        {
            if (hp <= hpMax)//플레이어 스테미너가 maximumSteminerRecovery 보다 클경우 아래 코드 실행
            {
                hp += Time.deltaTime * 10;
            }
        }
    }
}
