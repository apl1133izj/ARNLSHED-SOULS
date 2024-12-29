using System.Collections;
using TMPro;
using UnityEngine;
public class KeyTip : MonoBehaviour
{
    public bool firstMove = false;
    private bool firestItemEat = false;
    private bool firestIStat = false;
    private bool firestISave = false;
    private int firstitemEatCount;
    private int firstSaveCount;
    private bool buttonPush;
    public GameObject mainMenu;
    public GameObject firestMoveCheckUI;
    public GameObject[] movekey;
    public TextMeshProUGUI keyText;

    public GameObject firestItemEatCheckUI;
    public GameObject firestIStatCheckUI;
    public GameObject firestISaveCheckUI;
    public GameObject[] tipCompensation;//보상
    public PutitemBag bag;
    public PlayerUI playerUI;
    public GameObject bagSizeZero;


    void Update()
    {
        FirstButton();
        DataManager.instance.nowPlayer.movetip = firstMove;
    }
    void FirstButton()
    {
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.Escape))
        {
            buttonPush = true;
        }
        else
        {
            buttonPush = false;
        }
        if (!mainMenu.activeSelf && !firstMove)
        {
            StartCoroutine(MoveTip());
        }

        if (bag.firstStackPostionCount == 1)
        {
            if (!firestIStat)
            {
                firestIStat = true;
                StartCoroutine(SkillOpenTip());
            }
        }
        if (firestItemEat)
        {
            firestItemEat = false;
            StartCoroutine(BagTip());
        }

        if (!firestISave && bag.firstSavePostionCount == 1)
        {
            firestISave = true;
            StartCoroutine(SaveTip());
        }
    }
    IEnumerator MoveTip()
    {
        firstMove = true;

        firestMoveCheckUI.gameObject.SetActive(true);
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                movekey[0].SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                movekey[1].SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                movekey[2].SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                movekey[3].SetActive(false);
            }
            if (!movekey[0].activeSelf && !movekey[1].activeSelf &&
                !movekey[2].activeSelf && !movekey[3].activeSelf)
            {
                StartCoroutine(AttackTip());
                yield return null;
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator AttackTip()
    {
        keyText.text = "왼쪽 마우스 버튼을 누르면 공격 할 수 있습니다.";
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(RollingTip());
                yield return null;
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator RollingTip()
    {
        keyText.text = "버튼을 누르면 공격 을 회피 할 수 있습니다.";
        movekey[4].gameObject.SetActive(true);
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                yield return new WaitForSeconds(1f);
                movekey[4].gameObject.SetActive(false);
                yield return null;
                StartCoroutine(JumpTip());
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator JumpTip()
    {
        keyText.text = "버튼을 누르면 점프를 할 수 있습니다.";
        movekey[5].gameObject.SetActive(true);
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yield return new WaitForSeconds(1f);
                movekey[5].gameObject.SetActive(false);
                yield return new WaitForSeconds(1);
                keyText.text = "플레이어 조작 튜토리얼을 종료 합니다.";
                yield return new WaitForSeconds(2);
                firestMoveCheckUI.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator BagTip()
    {
        firestItemEatCheckUI.gameObject.SetActive(true);
        while (true)
        {
            if (buttonPush)
            {
                firestItemEatCheckUI.gameObject.SetActive(false);
                GameObject tipCompensations = Instantiate(tipCompensation[0], transform.position, Quaternion.identity);
                tipCompensations.name = tipCompensation[0].name;
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator SaveTip()
    {
        firestISaveCheckUI.gameObject.SetActive(true);
        while (true)
        {
            if (buttonPush)
            {
                firestISaveCheckUI.gameObject.SetActive(false);
                GameObject tipCompensations = Instantiate(tipCompensation[1], transform.position, Quaternion.identity);
                tipCompensations.name = tipCompensation[1].name;
                playerUI.bagCount = 2;
                yield break;
            }
            yield return null;
        }

    }
    IEnumerator SkillOpenTip()
    {
        firestIStatCheckUI.gameObject.SetActive(true);
        while (true)
        {
            if (buttonPush)
            {
                bagSizeZero.transform.localScale = Vector3.zero;
                firestIStatCheckUI.gameObject.SetActive(false);
                GameObject tipCompensations = Instantiate(tipCompensation[1], transform.position, Quaternion.identity);
                tipCompensations.name = tipCompensation[1].name;
                playerUI.bagCount = 2;
                yield break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 18)
        {
            if (firstitemEatCount < 1)
            {
                firestItemEat = true;
            }
            firstitemEatCount += 1;
        }
    }
}
