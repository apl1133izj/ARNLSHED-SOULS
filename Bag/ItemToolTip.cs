using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    CanvasGroup group;
    private GraphicRaycaster gr;
    private PointerEventData ped;
    GameObject onDragParentGameObject;
    [SerializeField] Transform onDragParent;
    [HideInInspector] public Transform startParent;
    public GameObject itemToolTip;
    public bool fillGauge;
    public bool rayHit;

    PutitemBag putItemBag;


    /*[0("체력")]
      [1("스테미나")]
      [2("속도")]
      [3("체력 재생")]
      [4("기력 재생")]
      [5("방어력")]
      [6("공격력")]
      [7("저주")]
      [8("공격 속도")]
    */
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI[] specText;
    public TextMeshProUGUI[] items;
    public TextMeshProUGUI postion;
    [Header("체력")]
    public int itemSpec_1;
    [Header("스테미나")]
    public int itemSpec_2;
    [Header("속도")]
    public int itemSpec_3;
    [Header("체력 재생")]
    public int itemSpec_4;
    [Header("기력 재생")]
    public int itemSpec_5;
    [Header("방어력")]
    public float itemSpec_6;
    [Header("공격력")]
    public int itemSpec_7;
    [Header("저주")]
    public int itemSpec_8;
    [Header("공격 속도")]
    public int itemSpec_9;
    [Header("아이템 설명")]
    public string description;
    void Start()
    {
        onDragParentGameObject = GameObject.Find("Storage space");
        onDragParent = onDragParentGameObject.GetComponent<Transform>();
        group = GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
        putItemBag = FindAnyObjectByType<PutitemBag>();
        gr = GetComponent<GraphicRaycaster>();
    }
    private void Update()
    {

        Ray();
        ToolTip();

    }
    void Ray()
    {
        var ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        // GraphicRaycaster를 통해 Raycast 실행
        gr.Raycast(ped, results);
        itemToolTip.transform.position = ped.position;

        rayHit = false;
        // Raycast 결과 확인
        foreach (var hitItem in results)
        {
            rayHit = true;
            // 마우스 클릭 상태 확인 && 레이가 쏴 맞은 게임 오브젝트가 layer가 아이템인 경우
            for (int i = 0; i < hitItem.gameObject.transform.childCount; i++)
            {
                if (hitItem.gameObject.transform.childCount > 1)
                {
                    Transform item = hitItem.gameObject.transform.GetChild(1);
                    ItemPerformance itemPerformance = item.GetComponent<ItemPerformance>();
                    ItemDrog itemDrog = item.GetComponent<ItemDrog>();
                    if (itemDrog != null)
                    {
                        if (itemDrog.f_itemclikTime >= 0.9f)
                        {
                            fillGauge = true;
                        }
                        else
                        {
                            fillGauge = false;
                        }
                    }
                    if (itemPerformance != null)
                    {
                        postion.gameObject.SetActive(false);
                        if (itemPerformance.transform.GetChild(1))
                        {
                            itemName.text = itemPerformance.name;
                            object[] itemSpecs = new object[9]
                            {
                                    itemPerformance.itemSpec_1, // int
                                    itemPerformance.itemSpec_2, // int
                                    itemPerformance.itemSpec_3, // int
                                    itemPerformance.itemSpec_4, // int
                                    itemPerformance.itemSpec_5, // int
                                    itemPerformance.itemSpec_6, // float
                                    itemPerformance.itemSpec_7, // int
                                    itemPerformance.itemSpec_8, // int
                                    itemPerformance.itemSpec_9,  // int
                            };
                            Vector2 startPosition = new Vector2(0, 0); // 초기 기준 위치
                            float verticalSpacing = 0.1f; // 각 요소 간의 세로 간격

                            int activeIndex = 0; // 활성화된 요소를 추적하기 위한 인덱스
                            for (int r = 0; r < itemSpecs.Length; r++)
                            {
                                if (itemSpecs[r] is int intValue && intValue > 0) // int 값이 0 이상일 때 처리
                                {
                                    specText[activeIndex].text = intValue.ToString();
                                    specText[activeIndex].transform.localPosition = startPosition - new Vector2(-60, verticalSpacing * activeIndex);
                                    specText[activeIndex].gameObject.SetActive(true);
                                    items[activeIndex].gameObject.SetActive(true);
                                    activeIndex++;
                                }
                                else if (itemSpecs[r] is float floatValue && floatValue > 0) // float 값이 0 이상일 때 처리
                                {
                                    specText[activeIndex].text = floatValue.ToString("F3");
                                    specText[activeIndex].transform.localPosition = startPosition - new Vector2(-60, verticalSpacing * activeIndex);
                                    specText[activeIndex].gameObject.SetActive(true);
                                    items[activeIndex].gameObject.SetActive(true);
                                    activeIndex++;
                                }
                                else
                                {
                                    specText[r].gameObject.SetActive(false); // 비활성화
                                    items[r].gameObject.SetActive(false);
                                }
                            }

                            // 나머지 비활성화
                            for (int s = activeIndex; s < specText.Length; s++)
                            {
                                specText[s].gameObject.SetActive(false);
                                items[s].gameObject.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        for (int r = 0; r < items.Length; r++)
                        {
                            items[r].gameObject.SetActive(false);
                        }
                        postion.gameObject.SetActive(true);
                        if (item.gameObject.name == "스탯증가 포션")
                        {

                            itemName.text = item.name;
                            postion.text = "스탯을 5 얻을수 있는 물약 입니다." +
                                            "마우스 오른쪽 버튼을 누를시 사용 가능 합니다.";
                        }
                        else if (item.gameObject.name == "HpPotion")
                        {
                            itemName.text = item.name;
                            postion.text = "체력을 회복 하는 물약 입니다. 물약 칸에 자동 장착 됨니다";
                        }
                        else if (item.gameObject.name == "StaminaPotion")
                        {
                            itemName.text = item.name;
                            postion.text = "기력을 회복 하는 물약 입니다. 물약 칸에 자동 장착 됨니다";
                        }
                        else if (item.gameObject.name == "CompensationHpPotion")
                        {
                            itemName.text = item.name;
                            postion.text = "체력을 회복 하는 물약 입니다. 물약 칸에 자동 장착 됨니다";
                        }
                        else if (item.gameObject.name == "영혼의 귀환석")
                        {
                            itemName.text = item.name;
                            postion.text = "아주 특별한 힘을 가지고 있는돌 죽어도 기억을 가진 채 다시 태어날 수 있습니다.[보스전에서는 사용할수 없습니다]";
                        }
                        else
                        {
                            for (int r = 0; r < items.Length; r++)
                            {
                                items[r].gameObject.SetActive(true);
                            }
                            postion.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
    void ToolTip()
    {
        if (rayHit)
        {
            itemToolTip.gameObject.SetActive(true);
        }
        else
        {
            itemToolTip.gameObject.SetActive(false);
        }
    }
}
