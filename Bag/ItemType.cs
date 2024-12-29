using TMPro;
using UnityEngine;

public class ItemType : MonoBehaviour
{
    public enum ItemTypes
    {
        hpPosion, staminPostion,
        Helmet,       // 머리
        Torso,      // 상체
        ArmsRight,  // 오른 팔
        ArmsLeft,  // 왼 팔
        Legs,       // 하체
        Feet,        // 발
        saveItem,
        Skill
    }
    public ItemTypes itemtype;
    public TextMeshProUGUI itemCount_Text;
    PutitemBag putitemBag;
    TMP_Text tmpText;  // TMP_Text 참조
    public int get_itemcount;
    public string displayedText;
    public string itemName;
    Potion position;
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        position = player.GetComponent<Potion>();
        putitemBag = FindAnyObjectByType<PutitemBag>();
        itemCount_Text = GetComponentInChildren<TextMeshProUGUI>();
        tmpText = GetComponentInChildren<TMP_Text>();
    }
    void Update()
    {
        // TMP_Text에서 현재 텍스트 값 가져오기
        itemCount_Text.text = get_itemcount.ToString();
        displayedText = itemCount_Text.text;
        itemName = gameObject.name;
        position.hpText.text = putitemBag.itemCount[81].ToString();
        position.staminaText.text = putitemBag.itemCount[82].ToString();
        if(get_itemcount == 0)
        {
            Destroy(gameObject);
        }

    }
    public int GetItemCount()
    {
        return get_itemcount;
    }
    public void DecreaseItemCount(int amount)
    {
        if (get_itemcount - amount >= 0) // 0 이하로 떨어지지 않도록 제어
        {
            get_itemcount -= amount;
            Debug.Log($"Item count decreased by {amount}. New count: {get_itemcount}");
        }
        else
        {
            Debug.LogWarning("Cannot decrease item count below 0.");
        }
    }
}
