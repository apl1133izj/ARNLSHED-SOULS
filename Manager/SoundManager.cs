using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // 싱글톤 패턴
    public AudioSource audioSource; // 메인 사운드 출력 AudioSource

    void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 유지
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    // 정적 메서드로 사운드 재생
    public static void PlaySound(AudioClip clip)
    {
        if (Instance != null && Instance.audioSource != null && clip != null)
        {
            Instance.audioSource.PlayOneShot(clip);
        }
    }
}
