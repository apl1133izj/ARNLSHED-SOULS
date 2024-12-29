using System.Collections;
using System.IO;
using UnityEngine;

public class TextReaderExample : MonoBehaviour
{

    private TextAsset textFile;  // 텍스트 파일을 저장할 변수
    private StringReader stringReader;  // 문자열을 읽기 위한 StringReader
    private string currentLine;  // 현재 읽고 있는 줄을 저장할 변수
    public UnityEngine.UI.Text textDi;
    public float typingSpeed;
    private bool isTyping = false;
    public QuestManager questManager;
    int buttonCount = 0;
    public PlayerUI playerUI;
    public int spaceCount = 0;
    public bool b_endQuest;
    public RecoveryArear recoveryArear;
    public GameObject recoveryArearEffect;
    public AudioClip questOnSound;
    public GameObject lastBosePotal;
    public void TextRead()
    {
        Debug.Log("TextRead 실행 중");

        // questName 값 로그 출력
        Debug.Log($"현재 questName: {QuestManager.questName}");

        // 이전 StringReader 닫기
        if (stringReader != null)
        {
            stringReader.Close();
            stringReader = null;
        }

        // 텍스트 파일 로드
        textFile = Resources.Load<TextAsset>(QuestManager.questName);
        if (textFile != null)
        {
            stringReader = new StringReader(textFile.text);
            playerUI.uiOpen = true;

            // 대화 오브젝트 활성화 확인
            if (!questManager.dialogueGameObject.activeSelf)
            {
                questManager.dialogueGameObject.SetActive(true);
            }

            // 첫 줄 읽어오기
            ReadNextLine();
        }
        else
        {
            Debug.LogError("텍스트 파일을 찾을 수 없습니다.");
        }
    }
    IEnumerator co()
    {
        QuestArear.b_textReadPlay = false;
        yield return null;
        TextRead();
    }
    void Update()
    {
        if (QuestArear.b_textReadPlay)
        {
            StartCoroutine(co());
        }
        // 버튼을 누르면 다음 줄로 넘어가도록 함
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttonCount += 1;

            if (buttonCount == 3)
            {
                buttonCount = 1;
            }
            Debug.Log(buttonCount);
        }
        if (Input.GetKeyDown(KeyCode.Space) && buttonCount == 1)
        {
            playerUI.textTypingSpeedText.text = "ON";
            typingSpeed = 0.001f;
        }
        if (Input.GetKeyDown(KeyCode.Space) && buttonCount == 2)
        {
            playerUI.textTypingSpeedText.text = "OFF";
            typingSpeed = 0.06f;
        }

    }

    public void NextDialogueButton()
    {
        if (!isTyping)
        {
            ReadNextLine();
        }
    }
    public void ReadNextLine()
    {
        // 이전에 읽은 줄이 있다면 처리 후 비우기
        if (!string.IsNullOrEmpty(currentLine))
        {
            textDi.text = "";
            Debug.Log("읽은 줄: " + currentLine);
        }

        // 다음 줄 읽기
        currentLine = stringReader.ReadLine();

        // 읽은 줄이 null이면 모든 줄을 다 읽었다는 뜻
        if (currentLine == null)
        {
            StartCoroutine(TextPosEnd());
            questManager.questBool[questManager.questProgress] = true;
            questManager.questCountBool[questManager.questProgress] = true;
            questManager.questGameObject[questManager.questProgress].SetActive(true);
            b_endQuest = true;//퀘스트 이벤트 종료
            DemoCharacter.noneSpeed = false;//플레이어 이동 가능
            playerUI.uiOpen = false;//UI비화성화
            questManager.conversationStatus = false;
            questManager.dialogueGameObject.SetActive(false);//대화창 비활성화
            QuestArear.b_textReadPlay = true;
            questManager.Compensation();
            SoundManager.PlaySound(questOnSound);
            playerUI.f_CurseProgression = 5000f;
            Invoke("InvokeFlase", 3);
            Debug.Log("더 이상 읽을 줄이 없습니다.");
            if (questManager.questProgress == 1)
            {
                questManager.uiElement[0].gameObject.SetActive(false);
            }
            if (questManager.questProgress > 1)
            {
                questManager.uiElement[questManager.questProgress - 1].gameObject.SetActive(false);
            }
            if (questManager.questProgress == 3)
            {
                questManager.uiElement[questManager.questProgress - 1].gameObject.SetActive(false);
                recoveryArear.enabled = true;
                recoveryArear.smallsanctum = true;
                recoveryArearEffect.SetActive(true);
            }
            if(questManager.questProgress == 6) 
            {
                lastBosePotal.SetActive(true);
            }
            return;
        }
        if (!questManager.dialogueGameObject.activeSelf)
        {
            questManager.dialogueGameObject.SetActive(true);
        }
        StartCoroutine(TypingTextCoroutine());
    }

    IEnumerator TypingTextCoroutine()
    {
        isTyping = true;  // 타이핑 중 플래그 설정
        textDi.text = "";  // 텍스트 UI 초기화
        foreach (char letter in currentLine)
        {
            textDi.text += letter;

            if (letter == ',')
            {
                textDi.text += "\n";  // '/' 문자 대신 줄바꿈 추가
            }
            yield return new WaitForSeconds(typingSpeed);  // 한 글자씩 출력하는 시간을 조절 가능.
        }

        isTyping = false;  // 타이핑 완료 후 플래그 해제
    }
    IEnumerator TextPosEnd()
    {
        //클리어 체크를 하는 게임 오브젝트가 활성화되어 있는 경우
        if (questManager.questClearMark[questManager.questProgress].activeSelf)
        {
            //포탈을 탈경우 맵이 비활성화 되기 때문에 "죽음의 수로"에 있지 않을경우 아래코드 실행 
            if (!questManager.questArea)
            {
                Debug.Log("신전");
                questManager.g_Quest_situation_Arear_Icon[0].gameObject.SetActive(true);
                questManager.g_Quest_situation_Arear_Icon[1].gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("던전");
                questManager.g_Quest_situation_Arear_Icon[3].gameObject.SetActive(true);
                questManager.g_Quest_situation_Arear_Icon[4].gameObject.SetActive(false);
            }

            questManager.f_textPos = -303f;
        }
        else//비활성화 되어 있는경우
        {
            if (!questManager.questArea)
            {
                Debug.Log("신전");
                questManager.g_Quest_situation_Arear_Icon[1].gameObject.SetActive(true);
                questManager.g_Quest_situation_Arear_Icon[0].gameObject.SetActive(false);
                questManager.g_Quest_situation_Arear_Icon[2].gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("던전");
                questManager.g_Quest_situation_Arear_Icon[4].gameObject.SetActive(true);
                questManager.g_Quest_situation_Arear_Icon[3].gameObject.SetActive(false);
                questManager.g_Quest_situation_Arear_Icon[5].gameObject.SetActive(false);
            }
            questManager.quest[questManager.questProgress].SetActive(true);//퀘스트 내용
            questManager.f_textPos = -359f;
        }
        yield return null;
        questManager.dialogueGameObject.SetActive(false);
    }
}

