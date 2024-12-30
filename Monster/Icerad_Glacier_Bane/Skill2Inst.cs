using System.Collections;
using UnityEngine;

public class Skill2Inst : MonoBehaviour
{
    public GameObject playerLooak;
    public GameObject skill2Arear;
    public Material skill2ArearMaterial;
    public GameObject[] skillPrefab;
    public float duration = 3f; // 색상 변화에 걸리는 시간
    void Start()
    {
        playerLooak = GameObject.Find("Player");
        //스킬생성시마다 메테리얼 rpga값 기본 초기화
        float r = 0f / 255f;
        float g = 0f / 255f;
        float b = 0f / 255f;
        float a = 120f / 255f;

        // Material의 색상을 설정
        Color color = new Color(r, g, b, a);
        skill2ArearMaterial.color = color;
        StartCoroutine(skill2());

    }
    IEnumerator skill2()
    {
        Debug.Log("드레곤 스킬 범위");
        Vector3 plyerPos = playerLooak.transform.localPosition;
        GameObject skill2InstArear = Instantiate(skill2Arear, new Vector3(plyerPos.x, 23.8f, plyerPos.z), Quaternion.identity);

        float elapsedTime = 0f;
        Color startColor = skill2ArearMaterial.color;// 현재 색상
        Color targetColor = new Color32(0x00, 0xC9, 0xFF, 0xFF);
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // 색상 보간
            skill2ArearMaterial.color = Color.Lerp(startColor, targetColor, t);
            // 시간 증가
            elapsedTime += Time.deltaTime;
            // 다음 프레임까지 대기
            yield return null;
        }
        skill2ArearMaterial.color = targetColor;
        Destroy(skill2InstArear);
        yield return null;
        GameObject skill2Inst = Instantiate(skillPrefab[2], new Vector3(plyerPos.x, 23.8f, plyerPos.z), Quaternion.identity);
    }
}
