using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class Raisthemouse : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float lerpTime = 5f;
    public GameObject[] moveGameObject;
    float currentTime = 0;
    public float duration = 1.0f; // 이동에 걸리는 시간
    public GameObject exitGameObject;//저장 로드 하는 페이지 비활성화
    public GameObject savePageExitGameObject;//저장하는 게임 오브젝트 비활성화
    public AudioClip buttonClip;
    private void Start()
    {
        for (int i = 0; i < moveGameObject.Length; i++)
        {
            moveGameObject[i].transform.position = startPosition.position;
        }
    }
    public void NextButton(int buttonN)
    {
        SoundManager.PlaySound(buttonClip);
        StartCoroutine(LerpNextPage(buttonN));
    }
    public void PreviousButton(int buttonN)
    {
        SoundManager.PlaySound(buttonClip);
        StartCoroutine(LerpPreviousPage(buttonN));
    }
    IEnumerator LerpNextPage(int buttonN)
    {
        currentTime = 0f; // 초기화

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;
            moveGameObject[buttonN].transform.position = Vector3.Lerp(startPosition.position, endPosition.position, t);
            yield return null; // 다음 프레임까지 대기
        }

        // 마지막 위치 보정 (정확히 endPosition에 도달)
        moveGameObject[buttonN].transform.position = endPosition.position;
    }

    IEnumerator LerpPreviousPage(int buttonN)
    {
        currentTime = 0f; // 초기화

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / duration;
            moveGameObject[buttonN].transform.position = Vector3.Lerp(endPosition.position, startPosition.position, t);
            yield return null; // 다음 프레임까지 대기
        }

        // 마지막 위치 보정 (정확히 endPosition에 도달)
        moveGameObject[buttonN].transform.position = startPosition.position;
    }
    public void SaveUIExit()
    {
        SoundManager.PlaySound(buttonClip);
        exitGameObject.SetActive(false);
    }

    public void UIExit() 
    {
        SoundManager.PlaySound(buttonClip);
        savePageExitGameObject.SetActive(false);
    }
}
