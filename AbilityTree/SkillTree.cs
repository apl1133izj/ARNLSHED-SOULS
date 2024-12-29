using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public GameObject player;
    public int[] skillLevel;
    public int[] skillMaxLevel;
    public bool[] upgradeComplete;//스킬 업그레이드 완료여부
    public Image[] enableSkill;//스킬 포인트를 이용해 스킬 업그레이드 했을때 색깔을 하얀색으로
    public Image[] skillConnection;
    public TextMeshProUGUI[] skillLevelTextMeshPro;

    //스킬 강화
    public HP hPS;
    public Stamina staminaS;
    Potion potion;
    AttackAndCombo attackAndCombo;
    DemoCharacter demoCharacter;
    PlayerUI playerUI;
    PutitemBag putitemBag;


    public TextMeshProUGUI[] skillText;//0:남은 스킬 포인트 1:사용한 스킬 포인트
    public int skillPoint; //사용 가능한 스킬 포인트
    public int usedSkillPoint;//사용한 스킬 포인트
    public bool[] skillUpGradeBool;
    int skillnum;

    public float upgradeReduceCoolTime;
    public Button[] skillpointActivationLevelButton;


    public int hpStack;
    public int staminaStack;
    public float hpRecoveryStack;
    public float staminaRecoveryStack;
    public float walkSpeedStack;
    public float runspeedStack;
    public float skillattackSpeed;
    public float staminaReductionRate;
    public float attackRange;
    public ToolTip[] toolTip;
    private void Start()
    {
        playerUI = GetComponent<PlayerUI>();
        player = GameObject.Find("Player");
        if (player != null)
        {
            potion = player.GetComponent<Potion>();
            attackAndCombo = player.GetComponent<AttackAndCombo>();
            putitemBag = player.GetComponent<PutitemBag>();
            demoCharacter = player.GetComponent<DemoCharacter>();
        }
        string filePath = DataManager.instance.path + DataManager.instance.nowSlot.ToString();
        if (File.Exists(filePath))
        {
            skillPoint = DataManager.instance.nowPlayer.skillPointData;
            usedSkillPoint = DataManager.instance.nowPlayer.useSkillPointData;
            for (int i = 0; i < skillLevel.Length; i++)
            {

                skillLevel[i] = DataManager.instance.nowPlayer.skillLevelsData[i];
                //Debug.Log($"Initial skillLevel[{i}] = {skillLevel[i]}");
                skillLevelTextMeshPro[i].text = skillLevel[i].ToString();
                if (skillLevel[i] > 0)
                {
                    skillConnection[i].fillAmount = 1;
                    enableSkill[i].color = Color.white;
                }
            }
        }
        // DataManager에서 불러온 데이터를 초기화

    }
    private void Update()
    {
        SKillTreeUI();
        UpdateSkillTreeUI();
    }

    void SKillTreeUI()
    {
        if (DataManager.instance != null)
        {
            DataManager.instance.nowPlayer.skillPointData = skillPoint;
            DataManager.instance.nowPlayer.useSkillPointData = usedSkillPoint;
            DataManager.instance.nowPlayer.skillTree = skillnum;
        }
        if (usedSkillPoint >= 15)
        {
            skillpointActivationLevelButton[2].interactable = true;
            skillpointActivationLevelButton[3].interactable = true;
        }
        if (usedSkillPoint >= 55)
        {
            skillConnection[14].fillAmount = 1;
            skillpointActivationLevelButton[4].interactable = true;
        }
        if (usedSkillPoint >= 70)
        {
            skillConnection[15].fillAmount = 1;
            skillConnection[16].fillAmount = 1;
            skillpointActivationLevelButton[5].interactable = true;
            skillpointActivationLevelButton[6].interactable = true;
        }
        if (usedSkillPoint >= 100)
        {
            skillpointActivationLevelButton[7].interactable = true;
            skillpointActivationLevelButton[8].interactable = true;
        }
        if (usedSkillPoint >= 130)
        {
            skillConnection[17].fillAmount = 1;
            skillpointActivationLevelButton[9].interactable = true;
            skillpointActivationLevelButton[10].interactable = true;
            skillpointActivationLevelButton[11].interactable = true;
        }
        if (usedSkillPoint >= 170)
        {
            skillConnection[18].fillAmount = 1;
            skillpointActivationLevelButton[12].interactable = true;
            skillpointActivationLevelButton[13].interactable = true;
        }
    }
    void UpdateSkillTreeUI()
    {
        if (DataManager.instance != null)
        {
            DataManager.instance.nowPlayer.skillPointData = skillPoint;
            DataManager.instance.nowPlayer.useSkillPointData = usedSkillPoint;
            for (int i = 0; i < skillLevel.Length; i++)
            {
                DataManager.instance.nowPlayer.skillLevelsData[i] = skillLevel[i];
            }
        }


        skillText[0].text = skillPoint.ToString();
        skillText[1].text = usedSkillPoint.ToString();
    }
    public void Skill(int skill)
    {
        //스킬 레벨은 필요한 스킬 포인트와 같음
        skillnum = skill;


        if (!upgradeComplete[skill])//업그레이드 가 완료된 상태가 아닌경우
        {

            //스킬 레벨이 MAX일 경우
            if (skillLevel[skill] == skillMaxLevel[skill])
            {
                if (skillPoint >= skillLevel[skill])
                {
                    skillLevelTextMeshPro[skill].text = skillLevel[skill].ToString();
                    skillPoint -= skillLevel[skill];
                    usedSkillPoint += skillLevel[skill];
                    MaxSkillCommend(skill);
                    upgradeComplete[skill] = true;
                }
            }
            else//아닌경우
            {
                if (skillPoint >= skillLevel[skill])
                {
                    skillConnection[skill].fillAmount = 1;//스킬이 활성화 되면 연결선 을 0에서 1로 채운다

                    enableSkill[skill].color = Color.white;//비활성화시 색상(회색) 활성화시 색상(하얀색)
                    skillLevelTextMeshPro[skill].text = skillLevel[skill].ToString();//스킬 레벨을 보여줌
                    skillPoint -= skillLevel[skill];//스킬 포인트를 레벨 만큼 감소 시킴
                    usedSkillPoint += skillLevel[skill]; //사용한 스킬 포인트를 스킬레벨 만큼 증가
                    skillLevel[skill] += 1; //레벨증가
                    SkillCommend(skill);

                }
            }
        }
    }
    /// <summary>
    ///  skillcommend : 스킬 종류 
    ///  스킬 레벨이 증가 하게 될경우 기본 증가 량 * 스킬레벨로 스탯이 증가 
    /// </summary>
    /// <param name="skillcommend"></param>
    private void SkillCommend(int skillcommend)
    {
        switch (skillcommend)
        {
            case 0://체력증가
                hpStack += 100 * skillLevel[skillcommend];
                break;
            case 1://스테미나 증가
                staminaStack += 100 * skillLevel[skillcommend];
                break;
            case 2://체력 재생 효과 증가
                hpRecoveryStack = 1 * skillLevel[skillcommend];
                break;
            case 3://재새용 대기시간 감소
                upgradeReduceCoolTime = 0.2f * skillLevel[skillcommend];
                break;
            case 4://포션 효과 증가
                potion.stackPostionRecovery = 50 * skillLevel[skillcommend];
                break;
            case 5://저주 효과 감소
                playerUI.f_Curse -= 0.025f * skillLevel[skillcommend];
                break;
            case 6://기력 재생 효과 증가
                staminaRecoveryStack = 1 * skillLevel[skillcommend];
                break;
            case 7://공격력 증가
                attackAndCombo.stackAttackDamage[0] += (1 * skillLevel[skillcommend]);
                attackAndCombo.stackAttackDamage[1] += (1 * skillLevel[skillcommend]);
                attackAndCombo.stackAttackDamage[2] += (1 * skillLevel[skillcommend]);
                attackAndCombo.stackAttackDamage[3] += (1 * skillLevel[skillcommend]);
                break;
            case 8://피해량 감소
                attackAndCombo.defend += 2 * skillLevel[skillcommend];
                break;
            case 9://이동 속도 증가
                walkSpeedStack += 0.15f * skillLevel[skillcommend];
                runspeedStack += 0.15f * skillLevel[skillcommend];
                break;
            case 10://공격 속도 증가
                skillattackSpeed += 0.03f * skillLevel[skillcommend];
                break;
            case 11://기력 사용시 비용 감소
                staminaReductionRate += 5f * skillLevel[skillcommend];
                break;
            case 12://공격 범위 증가
                attackRange += 0.025f * skillLevel[skillcommend];
                break;
            case 13: //궁극기 생성
                attackAndCombo.ultimateSkillunlock = true;
                attackAndCombo.ultimateSkillCoolTimeMax -= 1 * skillLevel[skillcommend];
                break;
        }
    }
    private void MaxSkillCommend(int skillcommend)
    {
        switch (skillcommend)
        {
            case 0:
                hPS.hpRegenerationBool = true;
                hPS.hpMax = 200 * skillLevel[skillcommend];
                hPS.hp = hPS.hpMax;
                break;
            case 1:
                staminaS.staminaRegenerationBool = true;
                staminaS.staminaMax = 300 * skillLevel[skillcommend];
                staminaS.stamina = staminaS.staminaMax;
                break;
            case 2:
                hPS.hpRecovery = 7;
                break;
            case 3:
                upgradeReduceCoolTime = 1.5f;
                break;
            case 4:
                potion.stackPostionRecovery = 700;
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                attackAndCombo.stackdefend = 15;
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
        }
    }
}
