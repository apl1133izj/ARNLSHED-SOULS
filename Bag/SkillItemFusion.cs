using System.Collections;
using UnityEngine;
//아이템과 스텟 으로 인한 스팩
public class SkillItemFusion : MonoBehaviour//장비 착용시 능력치 적용
{
    HP hpScript;
    Stamina staminaScript;
    AttackAndCombo attackAndComboScript;
    DemoCharacter demoCharacterScript;
    PutitemBag putitemBagScript;
    public float[] hpHap;
    public float[] staminaHap;
    public int[] speedHap;
    public int[] healthRegenerationHap;
    public int[] staminaRegenerationHap;
    public float[] defenseHap;
    public int[] attackDamageHap;
    public int[] curseHap;
    public int[] attackSpeedHap;
    public int itemNotNull;
    public int itemNull;
    public bool corutinitem;

    public bool healthRegenerationBool;
    void Start()
    {
        hpScript = GetComponent<HP>();
        staminaScript = GetComponent<Stamina>();
        attackAndComboScript = GetComponent<AttackAndCombo>();
        demoCharacterScript = GetComponent<DemoCharacter>();
        putitemBagScript = GetComponent<PutitemBag>();
    }

    private void Update()
    {
        NullArmor();
        if (corutinitem)
        {
            StartCoroutine(ItemCoroutin());
        }
        for (int i = 83; i <= 88; i++)
        {
            if (healthRegenerationHap[i - 83] > 0)
            {
                healthRegenerationBool = true;
            }

        }
    }
    IEnumerator ItemCoroutin()
    {
        corutinitem = false;
        /*        armorStatus(1, hpHap, staminaHap, speedHap, healthRegenerationHap, staminaRegenerationHap,
                          defenseHap, attackDamageHap, curseHap);*/
        ItemPluse();
        yield return null;
    }
    void NullArmor()
    {
        for (int i = 83; i <= 88; i++)
        {
            Transform slotTransform = putitemBagScript.ItemSlot[i].transform;
            if (slotTransform.childCount > 0) // 자식이 최소 2개 이상 있는지 확인
            {
                itemNotNull = i - 83;
            }
            else
            {
                itemNull = i - 83;
                hpHap[itemNull] = 0;
                staminaHap[itemNull] = 0;
                speedHap[itemNull] = 0;
                healthRegenerationHap[itemNull] = 0;
                staminaRegenerationHap[itemNull] = 0;
                defenseHap[itemNull] = 0;
                attackDamageHap[itemNull] = 0;
                curseHap[itemNull] = 0;
                attackSpeedHap[itemNull ] = 0;
            }
        }

    }
    public void ItemPluse()
    {
        // 83~88번 슬롯 아이템 확인
        for (int i = 83; i <= 88; i++)
        {
            if (putitemBagScript.ItemSlot[i] != null)
            {

                if (putitemBagScript.ItemSlot[i].transform.childCount > 1)
                {   // 아이템 성능 값 출력 (디버깅)

                    Transform item = putitemBagScript.ItemSlot[i].transform.GetChild(1);
                    ItemPerformance[] itemPerformance = item.GetComponents<ItemPerformance>();
                    if (itemPerformance.Length == 0)
                    {
                        continue; // 다음 슬롯으로 진행
                    }

                    for (int j = 0; j < itemPerformance.Length; j++)
                    {
                        //Debug.Log($"Slot {i}, Performance Index {j}: ");
                        // hpHap[1]에 값이 이미 들어있다면 해당 아이템을 건너뛰기
                        if (hpHap[i - 83] > 0 || staminaHap[i - 83] > 0 || speedHap[i - 83] > 0 || healthRegenerationHap[i - 83] > 0
                         || staminaRegenerationHap[i - 83] > 0 || defenseHap[i - 83] > 0 || attackDamageHap[i - 83] > 0 || curseHap[i - 83] > 0 || attackSpeedHap[i - 83] > 0)
                        {
                            continue; // 이미 값이 들어있다면 이 아이템을 건너뛰기.
                        }

                        hpHap[i - 83] += itemPerformance[j].itemSpec_1;
                        staminaHap[i - 83] += itemPerformance[j].itemSpec_2;
                        speedHap[i - 83] += itemPerformance[j].itemSpec_3;
                        healthRegenerationHap[i - 83] += itemPerformance[j].itemSpec_4;
                        staminaRegenerationHap[i - 83] += itemPerformance[j].itemSpec_5;
                        defenseHap[i - 83] += itemPerformance[j].itemSpec_6;
                        attackDamageHap[i - 83] += itemPerformance[j].itemSpec_7;
                        curseHap[i - 83] += itemPerformance[j].itemSpec_8;
                        attackSpeedHap[i - 83] += itemPerformance[j].itemSpec_9;
                    }
                }

            }

        }
    }
}
