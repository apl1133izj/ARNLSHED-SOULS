using System;
using System.Collections;
using TMPro;
using UnityEngine;
public class PutitemBag : MonoBehaviour
{
    public GameObject[] ItemSlot;
    GameObject item;
    public Transform[] itemPos;
    public GameObject[] insItem;
    private bool isCoroutineRunning = false;
    public bool ispluseItem = false;
    public string itemTag;                   //아이템 테그 확인
    public string[] itemNameString;
    public int itemTypeNum;
    public int[] itemCount;          //아이템 겟수
    public string[] s_itemCount;
    string itemnameUpdate;
    public Select select;
    public int bagSize = 90;
    public TextMeshProUGUI bagSizeUI;
    public Canvas blocksRaycastsCheckCanvas;
    public int firstStackPostionCount;
    public int firstSavePostionCount;
    public AudioClip itemEatSound;
    private void Start()
    {
        if (DataManager.instance != null)
        {
            Debug.Log("저장된 아이템이 있는지 확인");
            itemNameString = DataManager.instance.nowPlayer.itemnameData;
            itemCount = DataManager.instance.nowPlayer.itemCountData;
            for (int j = 0; j < itemNameString.Length; j++)
            {
                //수정) 0입력 됨으로 실행될경우가 있음 
                //비어있지 않을 경우
                if (!string.IsNullOrEmpty(itemNameString[j]) && itemNameString[j] != "0")
                {
                    // 아이템 오브젝트 생성
                    GameObject armorPrefab = Resources.Load<GameObject>($"Item/{itemNameString[j]}");
                    BagSize();
                    GameObject item = Instantiate(armorPrefab, ItemSlot[j].transform);
                    item.name = itemNameString[j];
                    ItemType itemType = item.GetComponent<ItemType>();
                    itemType.get_itemcount = itemCount[j];
                    // 아이템 인스턴스 생성
                    /*                        InsItem(itemNameString[j], j);
                                            item.name = "Item";
                                            // 디버그 로그 출력
                                            Debug.Log(j + ": " + itemNameString[j]);*/
                }
            }
        }

    }
    //아이템생성(아이템 테그 저장,아이템 위치 저장)
    public void InsItem(string _itemTag, int _itemPos)
    {

        if (_itemTag == "HpPosion")
        {
            item = Instantiate(insItem[0], itemPos[_itemPos]);
        }
        else if (_itemTag == "StaminaPosion")
        {
            item = Instantiate(insItem[1], itemPos[_itemPos]);
        }
        else if (_itemTag == "Helmet")
        {
            item = Instantiate(insItem[2], itemPos[_itemPos]);
        }
        else if (_itemTag == "Torso")
        {
            item = Instantiate(insItem[3], itemPos[_itemPos]);
        }
        else if (_itemTag == "ArmsRight")
        {
            item = Instantiate(insItem[4], itemPos[_itemPos]);
        }
        else if (_itemTag == "ArmsLeft")
        {
            item = Instantiate(insItem[5], itemPos[_itemPos]);
        }
        else if (_itemTag == "Legs")
        {
            item = Instantiate(insItem[6], itemPos[_itemPos]);
        }
        else if (_itemTag == "Feet")
        {
            item = Instantiate(insItem[7], itemPos[_itemPos]);
        }
    }
    private void Update()
    {
        ItemNameUpdate();
        ItemUpdate();
        if (DataManager.instance != null)
        {
            DataManager.instance.nowPlayer.itemCountData = itemCount;
            DataManager.instance.nowPlayer.itemnameData = itemNameString;
        }
    }


    public void ItemNameUpdate()
    {
        for (int i = 0; i < ItemSlot.Length; i++) // 모든 슬롯 순회
        {
            // 슬롯에 자식 객체가 있는 경우
            if (ItemSlot[i].transform.childCount > 1)
            {
                Transform child1 = ItemSlot[i].transform.GetChild(1); // 자식 인덱스 1 가져오기
                if (child1.gameObject.layer == 18) // 올바른 레이어 확인
                {
                    ItemType itemType = child1.GetComponent<ItemType>(); ;
                    if (itemType != null)
                    {
                        itemNameString[i] = itemType.itemName;   // 아이템 이름 동기화
                    }
                }
            }
            else // 슬롯이 비어 있는 경우
            {

                itemNameString[i] = "";        // 이름 정보 초기화
                itemCount[i] = 0;
            }
        }
    }

    public void ItemUpdate()
    {
        for (int i = 0; i < ItemSlot.Length; i++) // 슬롯 순회
        {
            if (ItemSlot[i].transform.childCount > 1)
            {
                Transform childName = ItemSlot[i].transform.GetChild(1);

                // 기존 슬롯에 동일한 아이템이 있는지 확인
                if (itemNameString[i] == childName.name)
                {
                    //Debug.Log("슬롯위치:" + i + childName.name);
                    itemCount[i] = childName.GetComponent<ItemType>().get_itemcount;
                }
                if (itemNameString[i] == "")
                {

                    itemCount[i] = 0;
                }
            }
        }
    }
    /// <summary>
    /// Beta1 : EmptyItemSlot  : 아이템이 생성 되어 있는지를 확인, 슬롯에 아이템이 있는경우 이름과 테그를 확인후
    ///                          모두 같을 경우, 아이템 겟수를 증가 시키고 만약 일치하지 않는 경우 아이템을 
    ///                          먹을때 아이템이름을 저장 했다가 Resources폴더에서 같은이름의 prefab의 슬롯에
    ///                          생성 시킵니다.
    /// final : EmptyItemSlot2 : beta1과 비슷한 방식이지만 itemNameString 즉 슬롯에 생성된 아이템이름을
    ///                          itemNameString배열에 저장후 beta1와 같은 방식으로 이름과 테그를 확인후
    ///                          모두 같을 경우, 아이템 겟수를 증가 시키고 만약 일치하지 않는 경우 아이템을
    ///                          생성 하도록 합니다.
    /// </summary>
    IEnumerator EmptyItemSlot(string Itemname)
    {
        // itemCheck();//아이템 생성시 테그에 맞는 아이템 가방에 생성
        if (isCoroutineRunning)//아이템겟수 2씩증가 방지
            yield break;

        isCoroutineRunning = true;
        //162~186 슬롯에 아이템이 있는지 확인 하고 있을경우 아이템 겟수만 증가 
        for (int i = 0; i < ItemSlot.Length; i++) // 슬롯을 순회하며 처리
        {
            Transform itemSlotTransform = ItemSlot[i].gameObject.transform;
            bool isEmptySlot = (itemSlotTransform.childCount <= 1); // 빈 슬롯인지 확인
            bool isMatchingSlot = false; // 현재 슬롯이 조건에 맞는 슬롯인지 확인

            if (!isEmptySlot) // 빈 슬롯이 아닌 경우, 슬롯의 아이템 정보를 검사
            {
                Transform child1 = itemSlotTransform.GetChild(1);

                if (child1.tag == itemTag) // 태그가 동일한 경우
                {
                    if (child1.name == Itemname) // 이름도 동일하면 해당 슬롯에서 아이템 수 증가
                    {
                        int index = Array.IndexOf(itemNameString, Itemname);
                        if (index != -1)
                        {
                            itemCount[index] += 1; // 동일한 이름의 아이템 개수 증가
                            s_itemCount[index] += 1;
                        }
                        isMatchingSlot = true;
                        break; // 조건에 맞는 슬롯을 찾았으므로 순회 종료
                    }
                }
            }
            // 빈 슬롯에 새로운 아이템 추가
            if (isEmptySlot && !isMatchingSlot)
            {
                // 프리팹 로드 및 생성
                GameObject armorPrefab = Resources.Load<GameObject>("Armor/" + Itemname);
                GameObject newItem = Instantiate(armorPrefab, itemPos[i]);
                newItem.name = Itemname; // 이름 설정 (Clone 제거)
                newItem.tag = itemTag;  // 태그 설정

                // 배열 및 카운트 초기화
                itemNameString[i] = Itemname; // 슬롯 이름 기록
                itemCount[i] = 1; // 새로운 아이템의 개수는 1로 고정
                s_itemCount[i] = "1";
                BagSize(); // 가방 크기 업데이트
                DataManager.instance.nowPlayer.itemCountData = itemCount; // 데이터 저장

                Debug.Log($"빈 슬롯에 새로운 아이템 '{Itemname}'을 추가했습니다.");
                break; // 아이템을 추가했으므로 순회 종료
            }
        }
        yield return null;

        isCoroutineRunning = false;

    }
    IEnumerator EmptyItemSlot2(string Itemname)
    {
        Debug.Log($"Trying to add item: {Itemname}");

        if (isCoroutineRunning) yield break; // 중복 코루틴 방지
        isCoroutineRunning = true;

        bool itemUpdated = false;  // 기존 아이템이 업데이트되었는지 확인
        bool itemAdded = false;    // 새 아이템이 추가되었는지 확인

        try
        {
            // 1. 기존 슬롯에서 동일한 아이템 업데이트
            for (int i = 0; i < ItemSlot.Length; i++)
            {
                if (itemNameString[i] == Itemname)
                {
                    Transform childName = ItemSlot[i].transform.GetChild(1);
                    ItemType itemType = childName.GetComponent<ItemType>();
                    itemCount[i] += 1;   // 개수 증가
                    s_itemCount[i] = itemCount[i].ToString(); // 문자열 동기화
                    itemType.get_itemcount = itemCount[i];
                    BagSize();
                    Debug.Log($"[Update] Slot {i}: {Itemname}, Count: {itemCount[i]}");
                    itemUpdated = true;
                    itemTag = "";
                    break;
                }
            }

            // 2. 빈 슬롯에 새 아이템 추가
            if (!itemUpdated)
            {
                for (int i = 0; i < ItemSlot.Length; i++)
                {
                    if (string.IsNullOrEmpty(itemNameString[i])) // 빈 슬롯 확인
                    {
                        itemNameString[i] = Itemname;
                        Debug.Log($"[Add] Slot {i}: {Itemname}, Count: {itemCount[i]}");
                        itemAdded = true;

                        // 아이템 오브젝트 생성
                        GameObject armorPrefab = Resources.Load<GameObject>($"Item/{Itemname}");
                        Debug.Log("armorPrefab이름" + armorPrefab);
                        if (armorPrefab == null)
                        {
                            Debug.LogError($"Failed to load prefab for item: {Itemname}");
                            break;
                        }
                        BagSize();
                        GameObject item = Instantiate(armorPrefab, ItemSlot[i].transform);
                        item.name = Itemname;
                        ItemType itemType = item.GetComponent<ItemType>();
                        itemCount[i] += itemType.get_itemcount;   // 개수 증가
                        s_itemCount[i] = itemCount[i].ToString(); // 문자열 동기화
                        break;
                    }
                }
            }

            // 3. 가방이 가득 찬 경우
            if (!itemUpdated && !itemAdded)
            {
                Debug.LogWarning($"No empty slots available. Cannot add item: {Itemname}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in EmptyItemSlot2: {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            itemTag = "";  // 태그 초기화
            isCoroutineRunning = false; // 코루틴 플래그 해제
        }

        yield break;
    }

    void BagSize()
    {
        bagSize -= 1;
        bagSizeUI.text = bagSize.ToString();
    }
    void itemCheck()
    {
        switch (itemTag)
        {
            case "HpPosion": itemTypeNum = 0; break;
            case "StaminaPosion": itemTypeNum = 1; break;
            case "Helmet": itemTypeNum = 2; break;
            case "Torso": itemTypeNum = 3; break;
            case "ArmsRight": itemTypeNum = 4; break;
            case "ArmsLeft": itemTypeNum = 5; break;
            case "Legs": itemTypeNum = 6; break;
            case "Feet": itemTypeNum = 7; break;
            case "SaveItem": itemTypeNum = 8; break;
            default: itemTypeNum = 9; break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 18 && bagSize != 0)
        {
            SoundManager.PlaySound(itemEatSound);
            itemTag = other.gameObject.tag;
            Destroy(other.gameObject);
            //StartCoroutine(EmptyItemSlot(other.gameObject.name));
            StartCoroutine(EmptyItemSlot2(other.gameObject.name));
        }
        //아이템을 고유 아이디를 가지고 온후 
        ItemID item = other.GetComponent<ItemID>();
        //null이 아닐경우 Collect()함수 실행
        if (item != null)
        {
            item.Collect();
        }
    }
}
