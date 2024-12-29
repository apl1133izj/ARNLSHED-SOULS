using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrog : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject beginDraggedItem;
    CanvasGroup group;
    public Vector3 startPosition;
    GameObject onDragParentGameObject;
    [SerializeField] Transform onDragParent;
    [HideInInspector] public Transform startParent;
    public Image delaysDrogImage;
    public float f_itemclikTime;
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;
    public ItemToolTip tip;
    public GameObject playerUI;
    public SkillTree skillTree;
    PutitemBag putItemBag;
    SkillItemFusion skillItemFusion;
    ItemType itemType;
    bool isDrag;
    public bool rayHit;
    public AudioClip onEndDragClip;
    void Start()
    {
        itemType = GetComponent<ItemType>();
        playerUI = GameObject.Find("Player UI");
        skillTree = playerUI.GetComponent<SkillTree>();
        onDragParentGameObject = GameObject.Find("Storage space");
        tip = onDragParentGameObject.GetComponent<ItemToolTip>();
        onDragParent = onDragParentGameObject.GetComponent<Transform>();
        group = GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
        putItemBag = FindAnyObjectByType<PutitemBag>();

        skillItemFusion = FindAnyObjectByType<SkillItemFusion>();
        gr = GetComponent<GraphicRaycaster>();
    }
    void InvokfillGauge()
    {
        tip.fillGauge = true;
    }
    private void Update()
    {

        //UI에 레이를 쏘는 코드
        // PointerEventData를 생성하고 마우스 위치를 설정
        var ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        // Raycast 결과를 저장할 리스트 생성
        List<RaycastResult> results = new List<RaycastResult>();

        // GraphicRaycaster를 통해 Raycast 실행
        gr.Raycast(ped, results);
        rayHit = false;
        // Raycast 결과 확인
        foreach (var hitItem in results)
        {

            rayHit = true;
            for (int i = 0; i < hitItem.gameObject.transform.childCount; i++)
            {
                Debug.Log("ItemDropg.cs hitItem.name:" + hitItem.gameObject.name);
                if (hitItem.gameObject.name == "StackPotion" &&
                    Input.GetMouseButtonDown(1))
                {

                    skillTree.skillPoint += 5 * itemType.get_itemcount;
                    Destroy(hitItem.gameObject);
                    if (putItemBag.firstStackPostionCount < 1)
                    {
                        putItemBag.firstStackPostionCount += 1;
                    }
                    break;
                }
                if (delaysDrogImage != null)
                {
                    break; // 첫 번째 Image 컴포넌트를 찾으면 종료
                }
            }


            // 마우스 클릭 상태 확인 && 레이가 쏴 맞은 게임 오브젝트가 layer가 아이템인 경우
            if (Input.GetMouseButton(0))
            {
                // 클릭 시간 증가
                f_itemclikTime += Time.deltaTime;
                delaysDrogImage.fillAmount = f_itemclikTime;
            }
            else
            {
                // 초기화
                if (delaysDrogImage != null)
                {
                    f_itemclikTime = 0;
                    delaysDrogImage.fillAmount = 0;
                }
            }
        }
        if (delaysDrogImage != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                f_itemclikTime = 0;
                delaysDrogImage.fillAmount = 0;
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작 시 fillAmount 조건 확인
        if (delaysDrogImage == null || delaysDrogImage.fillAmount < 1)
        {
            isDrag = false; // 드래그 상태를 false로 설정
            return;
        }

        isDrag = true;
        beginDraggedItem = gameObject;

        // 드래그 시작 시 현재 위치와 부모 저장
        startPosition = transform.position;
        startParent = transform.parent;

        // 드래그 중 blocksRaycasts 비활성화
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;

        // 드래그 상태에서 부모를 onDragParent로 변경
        transform.SetParent(onDragParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 상태가 true일 때만 작동
        if (!isDrag)
            return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;
        SoundManager.PlaySound(onEndDragClip);
        // 드래그 실패 또는 상태 초기화
        if (!isDrag)
        {
            RestoreOriginalState(); // 원래 위치로 복구
            return;
        }

        // 드래그 성공 후 상태 초기화
        isDrag = false; // 드래그 상태 종료
        f_itemclikTime = 0;

        if (delaysDrogImage != null)
            delaysDrogImage.fillAmount = 0;

        skillItemFusion.corutinitem = true; // 추가 동작 수행
    }

    // 원래 부모와 위치로 복구
    private void RestoreOriginalState()
    {
        // 원래 위치와 부모가 유효한지 확인
        if (startParent != null)
        {
            transform.SetParent(startParent); // 원래 부모로 복구
            transform.position = startPosition; // 원래 위치로 복구
        }
        else
        {
            Debug.LogWarning("Start parent is null. Cannot restore item.");
        }

        // 상태 초기화
        isDrag = false;
        f_itemclikTime = 0;

        if (delaysDrogImage != null)
            delaysDrogImage.fillAmount = 0;
    }
}
