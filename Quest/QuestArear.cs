using UnityEngine;
using UnityEngine.UI;
public class QuestArear : MonoBehaviour
{
    public QuestManager questManager;
    public TextReaderExample textReader;
    public FindLoad findLoad;
    public Text findLoadText;
    public GameObject findArrowLookAt;
    public DemoCharacter character;

    public bool b_CheckfindLoad;
    public static bool b_textReadPlay;
    public RecoveryArear recoveryArear;
    public bool smallRecoveryArear;

    private void Start()
    {
        if (textReader == null)
        {
            textReader = FindObjectOfType<TextReaderExample>();
        }
    }
    private void Update()
    {
        if (smallRecoveryArear)//작은 성소 로 인한 회복
        {
            if ((questManager.questProgress > 2))
            {
                recoveryArear.enabled = true;
            }
            else
            {
                recoveryArear.enabled = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //플레이어 태그가 부딪쳤거나 퀘스트가 진행중인 상태가 아닌경우에만 발동
        if (other.gameObject.CompareTag("Player") && !questManager.questBool[questManager.questProgress])
        {
            if (questManager.i_questMosterCount[questManager.questProgress] != 0)
            {
                Debug.Log("대화창실행");
                questManager.conversationStatus = true;
                DemoCharacter.noneSpeed = true;
            }
            b_textReadPlay = true;
        }

        if (other.gameObject.CompareTag("Player") && questManager.b_questClearProgress[questManager.questProgress])
        {
            questManager.QuestClear(questManager.questProgress);
            questManager.questBool[questManager.questProgress] = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && questManager.questProgress == 0)
        {
            if (!FindLoad.findLoadBool)
            {
                findLoadText.enabled = true;
                findArrowLookAt.SetActive(true);
                findLoad.questStartBool = true;
            }
        }
    }
}
