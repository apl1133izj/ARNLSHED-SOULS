using UnityEngine;
using UnityEngine.UI;
public class Potion : MonoBehaviour
{
    public int hpPotion;
    public int staminaPotion;

    public float maxHpRecovery;
    public float maxStaminaRecovery;
    PutitemBag bag;
    public float hpPostionRecovery;//200
    public float staminaPostionRecovery;//200
    public float stackPostionRecovery;//아이템 회복 효과 증가
    HP hpS;
    Stamina staminaS;
    AttackAndCombo attackAndCombo;
    //UI
    public Text hpText;
    public Text staminaText;
    PutitemBag putitemBag;
    public ItemType itemType;
    public int hpPotionItem;
    public int itemStaminaPostion;
    public float hpPostionCoolTime;
    public float staminaPostionCoolTime;
    public bool hpPotionIteminstallation;
    public bool StaminaPotionIteminstallation;
    void Start()
    {
        bag = GetComponent<PutitemBag>();
        Debug.Log("hp:" + hpPotion);
        Debug.Log("stamina:" + staminaPotion);


        hpPotion = DataManager.instance.nowPlayer.hpPositionData;
        staminaPotion = DataManager.instance.nowPlayer.staminaPositionData;

        hpS = GetComponent<HP>();
        staminaS = GetComponent<Stamina>();
        attackAndCombo = GetComponent<AttackAndCombo>();
        putitemBag = GetComponent<PutitemBag>();
    }
    private void Update()
    {
        hpPotion = putitemBag.itemCount[81];
        staminaPotion = putitemBag.itemCount[82];
        if (DataManager.instance != null)
        {
            DataManager.instance.nowPlayer.hpPositionData = hpPotion;
            DataManager.instance.nowPlayer.staminaPositionData = staminaPotion;
        }
        if (itemType != null)
        {
            hpPotionIteminstallation = true;
            if (Input.GetKey(KeyCode.Alpha1) && hpPotion >= 0 &&
            hpS.hp != hpS.hpMax && hpPostionCoolTime >= 3) //hp
            {
                hpPostionCoolTime = 0;
                itemType.DecreaseItemCount(1);
                hpPotion = itemType.get_itemcount;
            }
            else
            {
                hpPostionCoolTime += Time.deltaTime;
            }
        }
        else
        {
            hpPotionIteminstallation = false;
            //Debug.LogWarning("ItemType component not found on child of ItemSlot[81].");
        }
        if (itemType != null)
        {
            StaminaPotionIteminstallation = true;
            if (Input.GetKey(KeyCode.Alpha1) && staminaPotion >= 0 &&
            staminaS.stamina != staminaS.staminaMax && staminaPostionCoolTime >= 3) //hp
            {
                staminaPostionCoolTime = 0;
                itemType.DecreaseItemCount(1);
                staminaPotion = itemType.get_itemcount;
            }
            else
            {
                staminaPostionCoolTime += Time.deltaTime;
            }
        }
        else
        {
            StaminaPotionIteminstallation = false;
            //Debug.LogWarning("ItemType component not found on child of ItemSlot[82].");
        }
    }

    public void HpPotion()
    {
        if (hpPotion >= 1)
        {
            if (attackAndCombo.potionDrinkBool && hpS.hp < hpS.hpMax)
            {
                if (hpS.hp < hpS.hpMax - (hpPostionRecovery + stackPostionRecovery))
                {
                    hpS.hp += (hpPostionRecovery + stackPostionRecovery);
                }
                else
                {
                    hpS.hp += hpS.hpMax - hpS.hp;
                }

            }

        }
    }
    public void StaminaPotion()
    {
        if (staminaPotion >= 1)
        {
            if (attackAndCombo.potionDrinkBool && staminaS.stamina < staminaS.staminaMax)
            {
                if (staminaS.stamina < staminaS.staminaMax - (staminaPostionRecovery + stackPostionRecovery))
                {
                    staminaS.stamina += (staminaPostionRecovery + stackPostionRecovery);
                }
                else
                {
                    staminaS.stamina += staminaS.staminaMax - staminaS.stamina;
                }

            }
        }
    }

}
