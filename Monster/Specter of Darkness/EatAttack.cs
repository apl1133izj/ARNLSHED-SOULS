using UnityEngine;

public class EatAttack : MonoBehaviour
{

    GameObject player;
    HP playerHp;
    public GameObject[] b_attackTrigger;
    private void Start()
    {
        player = GameObject.Find("Player");
        playerHp = player.GetComponent<HP>();
    }
    public void EatAttacks()
    {
        playerHp.hp -= 80;
    }
    public void Attack(int leftRight)
    {
       b_attackTrigger[leftRight].SetActive(true);
    }
}
