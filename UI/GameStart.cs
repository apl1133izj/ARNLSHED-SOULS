using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class GameStart : MonoBehaviour
{
    public GameObject menu;
    public Text[] selectMenul; //0:게임 로드 및 새로 시작 1: 정보 2 : 게임 설정 3: 게임 종료
    public Text pRESSANYBUTTONText;
    public bool load; //로드할 게임이 있는가?
    public GameObject mainTitle;
    public Transform menuPos;
    public bool start;

    public DemoCharacter demoCharacter;
    public AudioSource[] gameSound;
    public Animator startAnimator;
    public Image[] fillStartImage;
    public Image fadeImage;
    public GameObject lodingGameObject;
    public bool startGameAnimator;
    public Camera camera;
    public GameObject loadData;
    public Canvas menucanvas;
    public bool loding;
    public GameObject playerState;
    public AudioClip[] menuMoveSound; // 이동 효과음
    public GameLoad gameLoad;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        CheckForSavedGames();
        demoCharacter.enabled = false;
        gameSound[0].enabled = false;
        gameSound[1].enabled = false;
        camera.enabled = false;
    }

    void CheckForSavedGames()
    {
        load = false; // 기본값은 저장된 게임이 없는 것으로 설정

        for (int i = 0; i < 3; i++)
        {
            string filePath = DataManager.instance.path + $"{i}";
            if (File.Exists(filePath))
            {
                load = true; // 저장된 게임이 하나라도 있으면 true로 설정
                Debug.Log($"슬롯 {i}: 저장된 게임이 있습니다.");
                return; // 하나라도 발견하면 추가 확인은 불필요하므로 종료
            }
        }

        Debug.Log("저장된 게임이 없습니다.");
    }

    // Update is called once per frame
    void Update()
    {
        if (!start)
        {
            if (Input.anyKeyDown)//아무 버튼 이나 누르면 메뉴로 이동
            {
                SoundManager.PlaySound(menuMoveSound[0]);
                pRESSANYBUTTONText.gameObject.SetActive(false);
                //start = true;
            }
        }
        if (!pRESSANYBUTTONText.gameObject.activeSelf)//pRESSANYBUTTONText가 비활성화 체크 비활성화라면 메뉴 활성화
        {
            if (!loding)
            {
                menu.gameObject.SetActive(true);

                Menu();
                MenuScroll();
                Invoke("InvokeStart", 1);
            }

        }
        else
        {
            menu.gameObject.SetActive(false);
        }
    }
    void InvokeStart()
    {
        start = true;
    }
    void Menu()
    {
        if (load)
        {
            selectMenul[0].text = "LOAD GAME";
        }
        else
        {
            selectMenul[0].text = "NEW GAME";
        }


        for (int i = 0; i < selectMenul.Length; i++)
        {
            if ((selectMenul[i].text == "LOAD GAME" || selectMenul[i].text == "NEW GAME") && Input.GetKeyDown(KeyCode.Space) && start)
            {

                if (selectMenul[i].text == "NEW GAME")
                {
                    SoundManager.PlaySound(menuMoveSound[0]);
                    startAnimator.enabled = true;
                }
                else
                {
                    SoundManager.PlaySound(menuMoveSound[0]);
                    //mainTitle.gameObject.SetActive(false);
                    camera.enabled = true;
                    loadData.gameObject.SetActive(true);
                    startAnimator.enabled = true;
                    demoCharacter.enabled = true;
                    gameSound[0].enabled = true;
                    gameSound[1].enabled = true;
                }
            }
            else if (selectMenul[i].text == "INFORMATION" && Input.GetKeyDown(KeyCode.Space))
            {
                SoundManager.PlaySound(menuMoveSound[0]);
            }
            else if (selectMenul[i].text == "SYSTEM" && Input.GetKeyDown(KeyCode.Space))
            {
                SoundManager.PlaySound(menuMoveSound[0]);
            }
            else if (selectMenul[i].text == "QUIT GAME" && Input.GetKeyDown(KeyCode.Space))
            {
                SoundManager.PlaySound(menuMoveSound[0]);
                Application.Quit();
            }
        }
    }
    public void EnableAnimation()
    {
        startAnimator.enabled = false;

        StartCoroutine(FillStartGauge());
    }
    public void PlayerState()
    {
        playerState.gameObject.SetActive(true);
    }
    IEnumerator FillStartGauge()
    {
        lodingGameObject.gameObject.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            loding = true;
            int random = Random.Range(3, 6);
            while (fillStartImage[0].fillAmount < 1)
            {
                fillStartImage[0].fillAmount += Time.deltaTime / random;
                fillStartImage[1].fillAmount += Time.deltaTime / random;
                yield return null;
            }
            fillStartImage[1].fillAmount = 0;
            fillStartImage[0].fillAmount = 0;
        }
        yield return new WaitForSeconds(1);
        lodingGameObject.gameObject.SetActive(false);
        fillStartImage[1].fillAmount = 1;
        fillStartImage[0].fillAmount = 1;
        startAnimator.SetLayerWeight(1, 1f);
        startAnimator.enabled = true;
        menucanvas.enabled = false;
        yield return new WaitForSeconds(2.4f);
        mainTitle.gameObject.SetActive(false);

        demoCharacter.enabled = true;
        gameSound[0].enabled = true;
        gameSound[1].enabled = true;
        startAnimator.enabled = false;
        // 현재 색상을 가져옵니다.
        Color tempColor = fadeImage.color;
        gameObject.SetActive(false);
        // 알파 값을 변경합니다 (0.0f = 완전 투명, 1.0f = 완전 불투명).
        tempColor.a = 0f;
        camera.enabled = true;
        // 변경된 색상을 다시 할당합니다.
        fadeImage.color = tempColor;
        Screen.SetResolution(1920, 1080, true);
        GameLoad.gameLoad = true;
        yield break;
    }
    void MenuScroll()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && menuPos.transform.localPosition.y > -43)
        {
            menuPos.transform.localPosition += Vector3.down * 43f;
            SoundManager.PlaySound(menuMoveSound[1]);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && menuPos.transform.localPosition.y < 86)
        {
            menuPos.transform.localPosition += Vector3.up * 43f;
            SoundManager.PlaySound(menuMoveSound[2]);
        }
    }
}
