using UnityEngine;
public class RecoveryArear : MonoBehaviour
{
    public GameObject RecoveryArearIntypingText;
    public typing[] RecoveryArearTyping;

    public HP hpS;
    public Stamina staminaS;
    public PlayerUI playerUI;
    public bool smallsanctum;
    public GameObject sanctumEffect;
    private void Update()
    {
        if (smallsanctum)
        {
            sanctumEffect.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("RecoveryArear에 입장");
            RecoveryArearIntypingText.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (hpS.hp <= hpS.hpMax)//플레이어 스테미너가 maximumSteminerRecovery 보다 클경우 아래 코드 실행
            {

                if (sanctumEffect)//작은 성소 회복
                {
                    hpS.hp += Time.deltaTime * 10;
                }
                else//큰성소 회복
                {
                    hpS.hp += Time.deltaTime * 20;
                }
            }
            if (staminaS.stamina <= staminaS.staminaMax)//플레이어 스테미너가 maximumSteminerRecovery 보다 클경우 아래 코드 실행
            {
                if (sanctumEffect)
                {
                    staminaS.stamina += Time.deltaTime * 10;
                }
                else
                {
                    staminaS.stamina += Time.deltaTime * 20;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("RecoveryArear에 퇴장");
            RecoveryArearIntypingText.SetActive(false);
            RecoveryArearTyping[0].count = 0;
            RecoveryArearTyping[1].count = 0;
        }
    }
}
