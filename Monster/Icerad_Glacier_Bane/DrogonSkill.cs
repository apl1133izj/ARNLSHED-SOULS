using System.Collections;
using UnityEngine;
public class DrogonSkill : MonoBehaviour
{
    GameObject player;
    Transform hitPos;
    Camera cameraEffect;
    public float maxEffect = 10f;  // 최대 데미지
    public float minEffect = 0;   // 최소 데미지
    public float effectRange = 10f; // 효과 범위
    public float shakeDuration = 0.5f; // 흔들림 지속 시간
    public float shakeMagnitude = 0.1f; // 흔들림 강도
    Rigidbody playerRb;
    MeshCollider meshCollider;
    [SerializeField]
    public int posInt;//인스펙터 창에서 설정
    public AudioClip audio;
    void Start()
    {
        // 플레이어 오브젝트를 찾아 변수에 할당
        player = GameObject.Find("Player");
        playerRb = player.GetComponent<Rigidbody>();
        cameraEffect = player.GetComponentInChildren<Camera>();
        // 새로운 GameObject를 생성하고 그 위치를 hitPos로 사용
        meshCollider = GetComponent<MeshCollider>();
        GameObject hitPosObject = new GameObject("HitPosition");
        hitPos = hitPosObject.transform;

        // hitPos의 위치를 플레이어의 현재 위치로 설정
        if (posInt == 0)
        {
            hitPos.position = player.transform.position;
        }
        else if (posInt == 1)
        {
            hitPos.position = new Vector3(player.transform.position.x + -3, player.transform.position.y, player.transform.position.z);
        }
        else if (posInt == 2)
        {
            hitPos.position = new Vector3(player.transform.position.x + -5, player.transform.position.y, player.transform.position.z);
        }
        else if (posInt == 3)
        {
            hitPos.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 3);
        }
        else if (posInt == 4)
        {

            hitPos.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 5);
        }


    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 60);
        transform.position = Vector3.MoveTowards(transform.position, hitPos.position, 55 * Time.deltaTime);
        if (transform.position.y <= hitPos.position.y)
        {

            //Destroy(gameObject);
        }
    }

    // 거리에 따라 카메라 흔들림을 계산하는 메서드
    IEnumerator CameraShake()
    {
        meshCollider.enabled = false;
        Vector3 originalPosition = cameraEffect.transform.localPosition;
        float elapsed = 0.0f;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        float effect = CalculateEffect(distance);

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-0.8f, 0.8f) * shakeMagnitude * effect;
            float y = Random.Range(-0.8f, 0.8f) * shakeMagnitude * effect;

            cameraEffect.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraEffect.transform.localPosition = originalPosition;
    }

    // 거리에 따라 카메라 흔들림을 계산하는 메서드
    float CalculateEffect(float distance)
    {
        // 거리가 가까울수록 카메라 흔들림이 커지도록 비율을 계산
        float distanceFactor = Mathf.Clamp01(1 - (distance / effectRange));

        // 최소 흔들림에서 최대 흔들림 사이의 값을 반환
        return Mathf.Lerp(minEffect, maxEffect, distanceFactor);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Ground"))
        {
            SoundManager.PlaySound(audio);
            StartCoroutine(CameraShake());
            //Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(CameraShake());
        }
    }
}
