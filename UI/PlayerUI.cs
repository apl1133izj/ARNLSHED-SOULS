using System.Collections;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public GameObject[] postionTypeImage;//포션이미지 바꾸기
    public Image[] potionImageA;//포션 이미지
    public Image[] previewBar;

    public HP hPS;
    public Stamina staminaS;
    public Potion potionS;
    public SkillTree skillTreeS;
    public PutitemBag putitemBagS;

    public Text textTypingSpeedText;//타이핑 속도 증가 속도 안내 즉 스페이스바를 눌렀는지 아닌지를 UI로 표시
    public Text announcementText;//대화중 안내문자
    public float announcementTime;//일정시간이 지나면 안내문자가 바뀜니다

    //마우스 관련
    [Header("마우스")]
    public Texture2D customCursor;
    public Texture2D clikCursor;
    public Vector2 hotSpot = Vector2.zero; // 커서의 핫스팟 위치
    public bool uiOpen; //마우스 커서를 사용할수있게 됩니다
    public CursorMode cursorMode = CursorMode.Auto;


    int menuCount;
    public int bagCount;
    public GameObject menuGameObject;
    public GameObject bagGameObject;
    public GameObject dataUIGameObject;
    //저장 정보
    public int[] _savePoint;
    public TextMeshProUGUI[] savePointText;//시간 마다 저장을 할수있는 포인트를 생성
    public TextMeshProUGUI[] savePointTimeText;//시간 마다 저장을 할수있는 포인트를 시간을 보여주는 UI
    public bool[] savePointSaveFile;
    public bool[] savePointTimeSaveFile;
    public TextMeshProUGUI[] staminatext;
    public TextMeshProUGUI[] hptext;
    public TextMeshProUGUI[] skillPointtext;
    public TextMeshProUGUI[] killedbosstext;
    public TextMeshProUGUI[] positionHptext;
    public TextMeshProUGUI[] positionStaminatext;
    //볼륨,효과음
    public AudioSource[] backgroundAudio;
    public Slider backgroundSlider;
    public AudioSource templeMapAudio;
    public AudioSource playerAudio;
    public Slider playerSlider;
    public AudioClip buttonClip;
    public GameObject startMenu;
    //시야 관련(프레임)
    public Camera camera;
    public Slider playerVisualrangeSlider;
    public TextMeshProUGUI performance;
    //저주
    public SkillItemFusion skillItemFusion;
    public GameObject curseGameObject;//저주가 해제 됬을시 저주 게임오브젝트 를 비활성화 하기 위한 게임오브젝트
    public Image Image_CurseProgression;//저주
    public TextMeshProUGUI[] curseText;
    public float f_CurseProgression;
    public float f_Maxf_CurseProgression = 5000;
    public float f_Curse = 1;
    public bool curseRemoval = false;
    //보스 처치
    public int boseKillNum;

    //어둠의 망령 tip
    //전투팁
    public bool b_tip;//처음 몬스터에(어둠의 망령) 제압 당했을시 제압에서 풀리는 방법을 알려줌
    public GameObject g_tipUI;
    public TextMeshProUGUI spaceCount;

    public GameObject g_cheat;
    private bool isCheatEnabled = false; // 치트 모드 상태를 저장하는 변수

    //가방
    public GameObject[] potionSlot;
    public AudioClip bagSound;

    public GameObject playerDie;
    private void Start()
    {
        Application.targetFrameRate = 90; // 60 FPS 제한
        QualitySettings.vSyncCount = 1;  // VSync로 프레임 속도 제어
        performance.text = "권장";
        /*        for (int r = 0; r < 3; r++)
                {
                    if (File.Exists(DataManager.instance.path + $"{r}"))	// 데이터가 있는 경우
                    {
                        Debug.Log("O");
                        DataManager.instance.nowSlot = r; // 현재 슬롯 설정
                        DataManager.instance.LoadData(); // 데이터를 해당 슬롯에서 불러옴
                        //f_CurseProgression = DataManager.instance.nowPlayer.curseProgressionData;
                    }
                    else
                    {
                        f_CurseProgression = 5000;
                    }
                }*/
        Cursor.SetCursor(customCursor, hotSpot, cursorMode);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        LoadGameData();
    }
    //아이템 칸에 포션을 장착했을경우
    void PotionBagIn()
    {
        //HP
        if (potionSlot[0].transform.childCount > 1)
        {
            potionImageA[0].gameObject.SetActive(true);
        }
        else
        {
            potionImageA[0].gameObject.SetActive(false);
        }
        //Stamina
        if (potionSlot[1].transform.childCount > 1)
        {
            potionImageA[1].gameObject.SetActive(true);
        }
        else
        {
            potionImageA[1].gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (DataManager.instance != null)
        {
            DataManager.instance.nowPlayer.curseProgressionData = f_CurseProgression;
        }
        if (!startMenu.activeSelf)
        {
            templeMapAudio.enabled = true;
        }
        if (!hPS.isDie)
        {
            Posion();
            PotionBagIn();
            MenuOpen();
            BagOpen();
            ConversationStatus();
            Mouse();
            Curse();
        }


        g_cheat.SetActive(CheateOpen());
    }
    void Curse()
    {
        if (!curseRemoval)//f_Maxf_CurseProgression가 0이 되면 저주가 해제됨f_CurseProgression0이 되면 사망함
        {
            if (f_CurseProgression >= 0)
            {
                float curseReductionRate = 0.5f + f_Curse + skillItemFusion.curseHap.Take(6).Sum();
                f_CurseProgression -= curseReductionRate * Time.fixedDeltaTime;
            }

            int curseProgress = Mathf.CeilToInt(f_CurseProgression);
            int curseMaxProgress = Mathf.CeilToInt(f_Maxf_CurseProgression);
            curseText[0].text = curseProgress.ToString();
            curseText[1].text = curseMaxProgress.ToString();
            Image_CurseProgression.fillAmount = f_CurseProgression / f_Maxf_CurseProgression;
            curseGameObject.SetActive(true);
        }
        else
        {
            curseGameObject.SetActive(false);
        }

        if (f_CurseProgression <= 0)
        {
            hPS.isDie = true;
        }
        else
        {
            hPS.isDie = false;
        }
        if (f_Maxf_CurseProgression > 0)
        {
            curseRemoval = false;
        }
        else
        {
            curseRemoval = true;
        }
    }
    void Mouse()
    {
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(clikCursor, hotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(customCursor, hotSpot, cursorMode);
        }
    }

    public void LoadGameData()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.instance.path + $"{i}"))
            {
                savePointSaveFile[i] = true;
                savePointTimeSaveFile[i] = true;
                _savePoint[i] = DataManager.instance.nowPlayer.savePoint;
                savePointText[i].text = DataManager.instance.nowPlayer.savePoint.ToString();
                DataManager.instance.nowPlayer.backgroundVolume = backgroundSlider.value;
                DataManager.instance.nowPlayer.playerVolume = playerSlider.value;
                staminatext[i].text = staminaS.stamina.ToString();
                hptext[i].text = hPS.hp.ToString();
                skillPointtext[i].text = skillTreeS.skillPoint.ToString();
                killedbosstext[i].text = boseKillNum.ToString();
                positionHptext[i].text = potionS.hpPotion.ToString();
                positionStaminatext[i].text = potionS.staminaPotion.ToString();
            }
            else
            {
                savePointText[i].text = "1";
            }
        }
    }
    public void Posion()
    {
        if (putitemBagS.itemCount[0] != 0)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                postionTypeImage[0].SetActive(true);
                postionTypeImage[1].SetActive(false);
                StartCoroutine(PreviewBar("Hp"));
            }
        }
        if (putitemBagS.itemCount[1] != 0)
        {
            if (Input.GetKey(KeyCode.Alpha2))
            {
                postionTypeImage[0].SetActive(false);
                postionTypeImage[1].SetActive(true);
                StartCoroutine(PreviewBar("Stamina"));
            }
        }
    }

    //포션 사용시 차는 게이지 미리보기
    IEnumerator PreviewBar(string previewBarName)
    {
        if (previewBarName == "Hp")
        {
            putitemBagS.itemCount[0] -= 1;
            previewBar[0].fillAmount += 0.2f;
            //previewBar[0].fillAmount = (hPS.hp + potionS.hpPostionRecovery) / hPS.hpMax;
        }
        else if (previewBarName == "Stamina")
        {
            putitemBagS.itemCount[1] -= 1;
            //previewBar[1].fillAmount = (staminaS.stamina + potionS.staminaPostionRecovery) / staminaS.staminaMax;
            previewBar[1].fillAmount += 0.2f;
        }
        yield return new WaitForSeconds(3.8f);
        if (previewBarName == "Hp")
        {
            previewBar[0].fillAmount = hPS.hp / hPS.hpMax;
        }
        else if (previewBarName == "Stamina")
        {
            previewBar[1].fillAmount = staminaS.stamina / staminaS.staminaMax;
        }
    }
    bool CheateOpen()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return isCheatEnabled = true;

        }
        else if (Input.GetKey(KeyCode.Alpha9))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return isCheatEnabled = false;
        }
        return isCheatEnabled;
    }
    public void MenuOpen()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuCount += 1;
        }

        if (menuCount == 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
            menuGameObject.SetActive(true);
            uiOpen = true;
        }
        else if (menuCount == 2)
        {
            menuGameObject.SetActive(false);
            uiOpen = false;
            menuCount = 0;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }

    public void BagOpen()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SoundManager.PlaySound(bagSound);
            bagCount += 1;
        }

        if (bagCount == 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
            bagGameObject.transform.localScale = Vector3.one;
            uiOpen = true;
        }
        else if (bagCount == 2)
        {

            bagGameObject.transform.localScale = Vector3.zero;
            uiOpen = false;
            bagCount = 0;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }
    void ConversationStatus()
    {
        if (uiOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    public void Audio(int _type)
    {
        if (_type == 1)
        {
            for (int i = 0; i <= 5; i++)
                backgroundAudio[i].volume = backgroundSlider.value;
        }

        if (_type == 2) playerAudio.volume = playerSlider.value;
    }
    public void VisualRange()
    {

        camera.farClipPlane = playerVisualrangeSlider.value * 800;

        if (camera.farClipPlane >= 800)
        {
            performance.text = "매우 높음";
        }
        else if (camera.farClipPlane >= 400)
        {
            performance.text = "높음";
        }
        else if (camera.farClipPlane >= 200)
        {
            performance.text = "권장";
        }
        else if ((camera.farClipPlane >= 100))
        {
            performance.text = "낮음";
        }
    }

    public void DataUI()
    {
        dataUIGameObject.SetActive(true);
        SoundManager.PlaySound(buttonClip);
    }
    public void DeDataUI()
    {
        SoundManager.PlaySound(buttonClip);
        menuCount = 2;
    }


}
