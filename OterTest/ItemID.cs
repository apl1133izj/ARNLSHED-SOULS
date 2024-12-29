using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemID : MonoBehaviour
{
    public string itemID; // 고유한 아이템 ID

    void Start()
    {
        // itemID 및 저장된 값 확인 (디버그용)
        Debug.Log($"Item ID: {itemID}, Saved Value: {PlayerPrefs.GetInt(itemID, 0)}");

        // 저장된 값이 1이면 아이템 비활성화
        if (PlayerPrefs.GetInt(itemID, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void Collect()
    {
        // 아이템을 먹은 것으로 표시
        Debug.Log($"Item Collected: {itemID}");
        PlayerPrefs.SetInt(itemID, 1);
        PlayerPrefs.Save();

        // 아이템 비활성화
        gameObject.SetActive(false);
    }

}
