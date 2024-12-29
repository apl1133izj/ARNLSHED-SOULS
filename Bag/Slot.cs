using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    GameObject player;
    HP hP;
    Stamina stamina;
    public bool postionCheck;
    public bool armerCheck;
    public int itemCount;
    // 허용된 아이템 타입
    public ItemType.ItemTypes allowedItemType;
    public bool isEquipmentSlot = false;  // 장비 슬롯인지 여부를 구분하는 변수

    void Awake()
    {
        player = GameObject.Find("Player");
        hP = player.GetComponent<HP>();
        stamina = player.GetComponent<Stamina>();
    }
    public GameObject Item()
    {
        if (transform.childCount <= 1)//아이템 겟수가 1개 이상일경우 널을 반환
        {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            else
                return null;
        }

        return null;//슬롯에 아이템이 있을 경우 이동 하지 못하도록 함
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedItem = ItemDrog.beginDraggedItem;

        if (draggedItem != null)
        {
            // 드래그된 아이템의 타입 확인
            ItemType itemType = draggedItem.GetComponent<ItemType>();

            // 슬롯의 자식 개수 확인
            if (transform.childCount >= 2) // 자식이 2개 이상인 경우
            {
                //이전 부모 슬롯에 되돌리기
                ItemDrog itemDrog = draggedItem.GetComponent<ItemDrog>();
                draggedItem.transform.SetParent(itemDrog.startParent);
                draggedItem.transform.position = itemDrog.startPosition;
                Debug.Log("슬롯이 꽉 찼습니다. 아이템을 넣을 수 없습니다.");
                return; // 아이템을 슬롯에 넣지 않고 종료
            }

            if (isEquipmentSlot)
            {
                if (itemType != null)
                {
                    // 드래그된 아이템을 슬롯에 배치
                    draggedItem.transform.SetParent(transform);
                    draggedItem.transform.position = transform.position;
                }
                else
                {
                    Debug.Log($"이 슬롯에는 {allowedItemType} 타입만 넣을 수 있습니다.");
                }
            }
            else
            {
                if (itemType != null)
                {
                    // 아이템 슬롯이면 타입 상관없이 넣기
                    draggedItem.transform.SetParent(transform);
                    draggedItem.transform.position = transform.position;
                }
            }
        }
    }

}


