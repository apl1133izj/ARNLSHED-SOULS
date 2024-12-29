using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class FindLoad : MonoBehaviour
{
    public GameObject playerPos;//플레이어 위치
    public GameObject destination;//도착 위치
    Vector3 meter;               //도착 위치까지의 거리
    public Text meterText;
    public SpriteRenderer arrowImage;//길찾기 화살표 이미지 색 변경
    typing typingS;
    bool effectBool = true;
    public static bool findLoadBool; //길을 찾았을때
    public bool questStartBool;//퀘스트가 진행중인지
    private void Start()
    {
        typingS = GetComponent<typing>();
    }
    void Update()
    {
        if (questStartBool)
        {
            Color color = arrowImage.color;
            color.a = 1f;
            arrowImage.color = color;
            LookAtPlayer(destination);

            //플레이어와 도착 위치까지의 거리 계산
            meter.x = playerPos.transform.position.x - destination.transform.position.x;


            if (Mathf.Round(meter.x) == 0)
            {
                meterText.color = Color.red;
                meterText.text = "퀘스트 장소 도착";
                
                if (effectBool)
                {
                    StartCoroutine(TextEffect());
                }
            }
            else
            {
                if (!findLoadBool)
                {
                    typingS.enabled = false;
                    meterText.text = "퀘스트 장소까지" + Mathf.Round(meter.x) + "M";
                }
            }
        }else
        {
            Color color = arrowImage.color;
            color.a = 0f;
            arrowImage.color = color;
        }
    }
    //문자 이펙트(이미지,문자)
    IEnumerator TextEffect()
    {
        yield return new WaitForSeconds(0.5f);
        meterText.enabled = false;
        gameObject.SetActive(false);
        effectBool = false;
        typingS.enabled = true;
        yield return new WaitForSeconds(4.05f);
        typingS.enabled = false;
        meterText.enabled = false;
        findLoadBool = true;
        gameObject.SetActive(false);
        yield return null;
    }
    //플레이어 방향 화살표
    void LookAtPlayer(GameObject loadRotation)
    {
        Vector3 direction = loadRotation.transform.position - transform.position;
        direction.y = 0;
        {
            //
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(81.55f, rotation.eulerAngles.y, 74.481f);
        }
    }


}
