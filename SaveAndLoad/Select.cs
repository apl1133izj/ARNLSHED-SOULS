using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Select : MonoBehaviour
{
    public GameObject playerPos;
    public HP hP;
    public PlayerUI playerUI;
    public GameObject creat;	// 플레이어 닉네임 입력UI
    public TextMeshProUGUI[] slotText;		// 슬롯버튼 아래에 존재하는 Text들
    public TextMeshProUGUI[] saveTimeText;
    public TextMeshProUGUI newPlayerName;  // 새로 입력된 플레이어의 닉네임

    public TMP_InputField playerNameInput;
    public string[] saveSlotName;
    public string[] saveTimeLoad;
    public bool[] savefile = new bool[3];	// 세이브파일 존재유무 저장
    public static int slaotArr;
    public DataManager dataManager;
    public PutitemBag putitemBag;
    public GameObject saveGameObject;
    //스크린샷
    public float screenShotTime;
    public Canvas saveCanvas;//저장시 화면캡쳐 Canvas 비활성화 시키기
    public Image[] screenShotImage;
    public Sprite[] screenShotSprite;
    public Image writingImage;
    public GameObject playerState;
    public GameObject startMenu;

    void Start()
    {
        // 슬롯별로 저장된 데이터가 존재하는지 판단.
        UpdateFile();
    }

    void UpdateFile()
    {

        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.instance.path + $"{i}"))	// 데이터가 있는 경우
            {
                Debug.Log(DataManager.instance.path + i);
                savefile[i] = true; //파일이 존재 한다 = true
                DataManager.instance.nowSlot = i;
                DataManager.instance.LoadData();    // 해당 슬롯 데이터 불러옴
                string filePath = $"{Application.persistentDataPath}/save{DataManager.instance.nowSlot}.png";
                screenShotImage[i].gameObject.SetActive(true);
                slotText[i].text = DataManager.instance.nowPlayer.name;
                saveTimeText[i].text = DataManager.instance.nowPlayer.saveTime;
                // Texture2D를 파일 시스템에서 로드(게임플 플레이하는 플레이어의 컴퓨터 경로에 맞는 경로 설정
                byte[] fileData = File.ReadAllBytes(filePath);
                playerPos.transform.position += DataManager.instance.nowPlayer.plyerPosition;
                Texture2D texture = new Texture2D(2, 2);

                if (texture.LoadImage(fileData))
                {
                    screenShotSprite[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    screenShotImage[i].sprite = screenShotSprite[i];
                }
                else
                {
                    Debug.LogError($"Failed to load texture from file data at '{filePath}'.");
                }
            }
            else	// 데이터가 없는 경우
            {
                Debug.Log(DataManager.instance.path);
                slotText[i].text = "빈 기록지";
                saveTimeText[i].text = "";
            }
        }

        // 불러온 데이터를 초기화시킴.(버튼에 닉네임을 표현하기위함이었기 때문)
        DataManager.instance.DataClear();
    }
    public void DestoryFile(int sloat)
    {
        Debug.Log("삭제 버튼 누름");
        string fullPath = Application.persistentDataPath + "/save" + sloat.ToString();
        string relativePath = Application.persistentDataPath + "/save" + sloat + ".png";
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            File.Delete(relativePath);
            DataManager.instance.nowPlayer = new PlayerData();
            screenShotImage[sloat].sprite = null;
            screenShotImage[sloat].gameObject.SetActive(false);
            playerUI.hptext[sloat].text = "0";
            playerUI.staminatext[sloat].text = "0";
            playerUI.skillPointtext[sloat].text = "0";
            playerUI.killedbosstext[sloat].text = "0";
            playerUI.positionHptext[sloat].text = "0";
            playerUI.positionStaminatext[sloat].text = "0";
            playerUI.savePointText[sloat].text = "0";
            saveTimeText[sloat].text = "";
            savefile[sloat] = false;
            slotText[sloat].text = "저장된 기억이 없음";
            Debug.Log("파일 삭제: " + fullPath);
            Debug.Log("파일 삭제: " + relativePath);
        }
        else
        {
            Debug.Log("삭제할 데이터가 없음: " + fullPath);
        }

    }
    public void Slot(int slaot)	// 슬롯의 기능 구현
    {
        if (playerUI._savePoint[0] >= 1 || playerUI._savePoint[1] >= 1 ||
            playerUI._savePoint[2] >= 1)
        {
            DataManager.instance.nowSlot = slaot;   // 슬롯의 번호를 슬롯번호로 입력함.
            slaotArr = slaot;
            if (savefile[slaot])    // bool 배열에서 현재 슬롯번호가 true라면 = 데이터 존재한다는 뜻
            {
                Debug.Log("저장");
                startMenu.gameObject.SetActive(false);
                playerState.gameObject.SetActive(true);
                DataManager.instance.nowSlot = slaot;
                DataManager.instance.LoadData();    // 데이터를 로드하고

                GoGame();   // 게임씬으로 이동
            }
            else    // bool 배열에서 현재 슬롯번호가 false라면 데이터가 없다는 뜻
            {
                Creat();    // 슬롯 이름 입력 UI 활성화
            }
        }
    }

    public void Creat()	// 플레이어 닉네임 입력 UI를 활성화하는 메소드
    {
        creat.gameObject.SetActive(true);
    }

    public void DeCreat()	// 플레이어 닉네임 입력 UI를 활성화하는 메소드
    {
        saveGameObject.gameObject.SetActive(false);
    }

    public void GoGame()	// 게임씬으로 이동
    {
        if (!savefile[DataManager.instance.nowSlot])	// 현재 슬롯번호의 데이터가 없다면
        {
            DataManager.instance.nowPlayer.itemnameData = putitemBag.itemNameString;
            DataManager.instance.nowPlayer.itemCountData = putitemBag.itemCount;
            DataManager.instance.nowPlayer.name = newPlayerName.text; // 입력한 이름을 복사해옴
            DataManager.instance.nowPlayer.plyerPosition = playerPos.transform.position; //플레이어 위치 저장  
            DateTime now = DateTime.Now;
            saveTimeLoad[slaotArr] = now.ToString("yyyy-MM-dd HH:mm:ss");                //시간 저장
            DataManager.instance.nowPlayer.saveTime = saveTimeLoad[slaotArr]; // 입력한 이름을 복사해옴
            playerUI._savePoint[0] -= 1;
            playerUI._savePoint[1] -= 1;
            playerUI._savePoint[2] -= 1;
            DataManager.instance.nowPlayer.savePoint--;
            DataManager.instance.SaveData(); // 현재 정보를 저장함.
            StartCoroutine(Screenshot());
            GameLoad.gameLoad = true;
        }
        else
        {
            playerState.gameObject.SetActive(true);
            GameLoad.gameLoad = true;
            startMenu.gameObject.SetActive(false);
            SceneManager.LoadScene(1); // 게임씬으로 이동  
        }
    }

    IEnumerator Screenshot()
    {
        saveCanvas.enabled = false;
        //string relativePath = $"Assets/Resources/Sceenshot/save{DataManager.instance.nowSlot}.png";
        string relativePath = $"{Application.persistentDataPath}/save{DataManager.instance.nowSlot}.png";
        ScreenCapture.CaptureScreenshot(relativePath);

        yield return new WaitForSeconds(2f);
        saveCanvas.enabled = true;
        writingImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(7f);
        writingImage.gameObject.SetActive(false);
        SceneManager.LoadScene(1);

    }
}
