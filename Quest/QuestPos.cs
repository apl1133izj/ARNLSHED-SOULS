using UnityEngine;

public class QuestPos : MonoBehaviour
{
    public QuestManager questManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (questManager.questProgress == 1)
            {
                questManager.i_questMosterCount[1] = 0;
            }

        }
    }
}
