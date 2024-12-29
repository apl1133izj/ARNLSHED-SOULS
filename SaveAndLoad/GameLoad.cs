using UnityEngine;

public class GameLoad : MonoBehaviour
{
    public static bool gameLoad;
    public bool game;
    public GameObject StartMenu;
    public GameObject playerState;
    public GameObject tutorials;

    public HP GetHP;
    public Stamina GetStamina;
    public AttackAndCombo GetAttackAndCombo;
    public PutitemBag GetPutitemBag;
    public Potion GetPotion;
    public QuestManager GetQuestManager;
    public KeyTip GetKeyTip;
    public Skill GetSkill;
    private void Start()
    {
        Debug.Log("gameLoad" + gameLoad);

        if (gameLoad)
        {

            StartMenu.gameObject.SetActive(false);
            tutorials.gameObject.SetActive(false);
            playerState.gameObject.SetActive(true);
        }
        // 데이터 로드


        // 데이터 적용
        if (DataManager.instance.nowPlayer != null)
        {
            DataManager.instance.LoadData();
            ApplyPlayerData();
        }
    }
    private void Update()
    {
        game = gameLoad;
        if (gameLoad && GetHP != null)
        {

            StartMenu.gameObject.SetActive(false);
            tutorials.gameObject.SetActive(false);
            playerState.gameObject.SetActive(true);
        }

    }
    // 로드된 데이터를 기반으로 플레이어 상태를 적용
    private void ApplyPlayerData()
    {
        // 예시: 플레이어 위치, 체력 적용
        GetHP.hp = DataManager.instance.nowPlayer.hpData;
        GetHP.hpMax = DataManager.instance.nowPlayer.hpMaxData;
        GetStamina.stamina = DataManager.instance.nowPlayer.staminaData;
        GetStamina.staminaMax = DataManager.instance.nowPlayer.staminaMaxData;
        GetAttackAndCombo.savePos = DataManager.instance.nowPlayer.plyerPosition;
        GetPutitemBag.itemNameString = DataManager.instance.nowPlayer.itemnameData;
        GetPutitemBag.itemCount = DataManager.instance.nowPlayer.itemCountData;
        GetQuestManager.questProgress = DataManager.instance.nowPlayer.questProgressData;
        GetKeyTip.firstMove = DataManager.instance.nowPlayer.movetip;
        GetSkill.skillEnhanceCount = DataManager.instance.nowPlayer.skillEnhanceCount;
        // 추가로 아이템이나 퀘스트 상태 등도 적용 가능
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
