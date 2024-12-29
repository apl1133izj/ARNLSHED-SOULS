using System.Collections;
using UnityEngine;
public class QuestManager : MonoBehaviour
{
    public bool[] questCountBool; //퀘스트를 깬경우:false 진행중인 경우:true
    public bool[] questBool;//퀘스트를 이미 받았다면
    public GameObject[] questGameObject;
    public float f_textPos;
    public GameObject[] g_Quest_situation_Arear_Icon;//퀘스트 상태를 에리어 내 아이콘 으로 확인
    public GameObject dialogueGameObject;
    public GameObject[] quest;
    float transformY;
    public int questProgress;  //퀘스트 진행도
    public bool conversationStatus = false;// 대화가 종료 되었을경우(1:대화 시작 , 0: 대화 종료)

    public static string questName;
    public static string questClearName;
    public int[] i_questMosterCount; //0:creep ,1:등등 
    public bool[] b_questClearProgress;//클리어 진행정도
    public GameObject[] questClearMark;//퀘스트를 클리어 했을시 체크마크 를 활성화
    public TextReaderExample textReader;
    [Range(-1000, 1000)]
    public float textpos;
    public RectTransform[] uiElement; // 이동할 UI 요소

    public GameObject[] compensationGameObject;//보상
    public Transform[] itemInstPos;
    public bool smallsanctum;

    public GameObject[] arearEffect;
    //소리
    public AudioClip questClearSound;
    public BoxCollider bose1BoxCollider;

    public bool questArea; //false:신전 true:던전
    private void Start()
    {
        if (questProgress > 0)
        {
            for (int j = 0; j < questProgress; j++)
            {
                questBool[j] = true;
            }
        }
        questCountBool[questProgress] = false;
        uiElement[questProgress].anchoredPosition = new Vector2(uiElement[questProgress].anchoredPosition.x, -303f);
        //questProgress = 1;
    }
    void Update()
    {

        DataManager.instance.nowPlayer.questProgressData = questProgress;
        Quest(questProgress);
        QuestClear(questProgress);
        PostionText(f_textPos);
    }
    public void Compensation()
    {
        if (questBool[1])
        {
            GameObject item = Instantiate(compensationGameObject[0], itemInstPos[0].position + Vector3.up, Quaternion.identity);
            item.name = compensationGameObject[0].name;
        }
        else if (questBool[2])
        {
            GameObject item2 = Instantiate(compensationGameObject[1], itemInstPos[0].position + Vector3.up, Quaternion.identity);
            item2.name = compensationGameObject[1].name;
            GameObject item3 = Instantiate(compensationGameObject[2], itemInstPos[1].position + Vector3.up, Quaternion.identity);
            item3.name = compensationGameObject[2].name;
        }
        else if (questBool[5])
        {
            GameObject item4 = Instantiate(compensationGameObject[4], itemInstPos[0].position + Vector3.up, Quaternion.identity);
            item4.name = compensationGameObject[4].name;
        }
    }

    public void QuestClear(int inputQuestProgress)
    {
        if (i_questMosterCount[inputQuestProgress] <= 0)
        {
            questCountBool[questProgress] = true;
            StartCoroutine(QuestGameObjectFalse());
            Debug.Log($"퀘스트 {inputQuestProgress} 클리어");
            SoundManager.PlaySound(questClearSound);
            // 퀘스트 종료 상태 초기화
            textReader.b_endQuest = false;
            b_questClearProgress[inputQuestProgress + 1] = true;
            questClearMark[inputQuestProgress].gameObject.SetActive(true);
            if (arearEffect[0].activeSelf)
            {
                g_Quest_situation_Arear_Icon[0].gameObject.SetActive(false);
                g_Quest_situation_Arear_Icon[1].gameObject.SetActive(false);
                g_Quest_situation_Arear_Icon[2].gameObject.SetActive(true);
            }
            else if (arearEffect[1].activeSelf)
            {
                g_Quest_situation_Arear_Icon[3].gameObject.SetActive(false);
                g_Quest_situation_Arear_Icon[4].gameObject.SetActive(false);
                g_Quest_situation_Arear_Icon[5].gameObject.SetActive(true);
            }

            // 진행도 증가
            questProgress++;
        }
    }
    IEnumerator QuestGameObjectFalse()
    {
        f_textPos = -303f;
        yield return new WaitForSeconds(4);
        questGameObject[questProgress].gameObject.SetActive(false);
    }
    //퀘스트 진행도 마다 읽어오는 퀘스트 text파일이 달라짐
    public void Quest(int questS)//홀수는 퀘스트 짝수는 클리어
    {
        if (conversationStatus)
        {
            // questProgress = questS;
            switch (questS)
            {
                case 0:
                    questName = "Q1";
                    dialogueGameObject.SetActive(true);
                    i_questMosterCount[questS] = 2;
                    break;
                case 1:
                    questName = "C1";
                    dialogueGameObject.SetActive(true);
                    break;
                case 2:
                    questName = "Q2";
                    dialogueGameObject.SetActive(true);
                    i_questMosterCount[questS] = 4;
                    break;
                case 3:
                    questName = "C2";
                    dialogueGameObject.SetActive(true);
                    break;
                case 4:
                    questName = "Q3";
                    bose1BoxCollider.enabled = true;
                    dialogueGameObject.SetActive(true);
                    break;
                case 5:
                    questName = "C3";
                    compensationGameObject[3].SetActive(true);
                    dialogueGameObject.SetActive(true);
                    break;
                case 6:
                    questName = "Q4";
                    dialogueGameObject.SetActive(true);
                    break;
                case 7:
                    questName = "C4";
                    dialogueGameObject.SetActive(true);
                    break;
                case 8:
                    questName = "Q5";
                    dialogueGameObject.SetActive(true);
                    break;
                case 9:
                    questName = "C5";
                    dialogueGameObject.SetActive(true);
                    break;
                case 10:
                    questName = "Q6";
                    dialogueGameObject.SetActive(true);
                    break;
                case 11:
                    questName = "C6";
                    dialogueGameObject.SetActive(true);
                    break;
                case 12:
                    questName = "Q7";
                    dialogueGameObject.SetActive(true);
                    break;
                case 13:
                    questName = "C7";
                    dialogueGameObject.SetActive(true);
                    break;
                default:
                    Debug.LogError($"정의되지 않은 questProgress: {questProgress}");
                    break;
            }
        }
    }
    public void PostionText(float pos)
    {
        if (questCountBool[questProgress])
        {
            // t 값(비율)을 따로 계산해서 사용
            float currentY = uiElement[questProgress].anchoredPosition.y;

            //현재 Y 위치 에서 pos까지 부드럽게 이동
            float newY = Mathf.MoveTowards(currentY, pos, 90 * Time.deltaTime);

            // 위치 업데이트
            uiElement[questProgress].anchoredPosition = new Vector2(uiElement[questProgress].anchoredPosition.x, newY);
        }
    }
}
