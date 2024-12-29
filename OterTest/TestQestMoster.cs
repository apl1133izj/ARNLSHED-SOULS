using UnityEngine;

public class TestQestMoster : MonoBehaviour
{
    public QuestManager questManage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HandAttack"))
        {
            Debug.Log("∏ÛΩ∫≈Õ");
            questManage.i_questMosterCount[0]--;
            //Destroy(gameObject);
        }
    }
}
