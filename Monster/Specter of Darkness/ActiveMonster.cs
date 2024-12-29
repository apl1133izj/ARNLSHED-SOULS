using UnityEngine;

public class ActiveMonster : MonoBehaviour
{
    public GameObject monster;
    public MeshRenderer well;
    public BoxCollider boxCollider;
    public QuestManager questManager;
    public bool clear;

    public GameObject playerNullCheck;
    public void Update()
    {
        if (questManager.questProgress == 1 || clear) 
        {
            boxCollider.isTrigger = true;
            well.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            monster.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            well.enabled = true;
            boxCollider.isTrigger = false;
            //playerNullCheck = other.gameObject;
        }
    }
}
