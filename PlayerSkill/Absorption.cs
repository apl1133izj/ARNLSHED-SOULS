using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Absorption : MonoBehaviour
{
    GameObject playerAbsorptionPos;
    GameObject playerPos;
    public GameObject instSkill;
    public bool b_Absorption;
    public GameObject[] skillDis;

    Skill skillCount;
    public GameObject g_playerUI;
    PlayerUI playerUI;
    public AudioClip absorptionClip;
    float speed = 1;

    // SerializedObject 관련 변수 (UnityEditor에서만 사용)
#if UNITY_EDITOR
    SerializedObject serializedObject;
    // 필요한 SerializedObject를 사용하는 경우 추가 코드
#endif

    void Start()
    {
        playerPos = GameObject.Find("Player");
        playerAbsorptionPos = GameObject.Find("Fighter_10_Armors_Mesh");
        float speedDistance = Vector3.Distance(gameObject.transform.position, playerAbsorptionPos.transform.position);
        if (speedDistance >= 3)
        {
            speed = speedDistance / 2; Debug.Log(speed);
        }
    }
    void Update()
    {
        // 플레이어의 흡수 스킬을 사용한 건틀릿으로 이동
        if (Input.GetKeyDown(KeyCode.Tab)&&Distance(playerAbsorptionPos) <= 10)
        {
            skillDis = GameObject.FindGameObjectsWithTag("Skill");
            skillCount = playerPos.GetComponent<Skill>();
            g_playerUI = GameObject.Find("Player UI");
            playerUI = g_playerUI.GetComponent<PlayerUI>();
            b_Absorption = true;
        }
        if (b_Absorption && Distance(playerAbsorptionPos) <= 10)
        {
            ArearSkillAbsorptionCount();
        }
        if (Distance(playerAbsorptionPos) >= 10)
        {
            b_Absorption = false;
        }
    }

    // Distance 함수 이름 수정
    float Distance(GameObject player)
    {
        return Vector3.Distance(gameObject.transform.position, player.transform.position);
    }

    /// <summary>
    /// 가장 가까운 영혼부터 흡수하는 시스템
    /// </summary>
    void ArearSkillAbsorptionCount()
    {
        // 가장 가까운 스킬 찾기
        GameObject closestSkill = null;
        Vector3 startPos = Vector3.zero;
        float closestDistance = float.MaxValue;
        

        foreach (var skillObj in skillDis)
        {
            if (skillObj == null) continue; // Null 체크
            float distance = Vector3.Distance(skillObj.transform.position, playerAbsorptionPos.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSkill = skillObj.gameObject;
                startPos = closestSkill.transform.position;
            }
        }
        // 가장 가까운 스킬이 없으면 함수 종료
        if (closestSkill == null)
        {
            Debug.Log("가까운 스킬 오브젝트가 없음");
            return;
        }
        // 흡수 완료 조건: 위치 차이가 작을 경우
        if (Vector3.Distance(closestSkill.transform.position, playerAbsorptionPos.transform.position) <= 0.1f)
        {
            UpdateSkillDisArray(closestSkill);
        }
        else
        {
            closestSkill.transform.position = Vector3.Lerp(startPos, playerAbsorptionPos.transform.position,speed  * Time.deltaTime);
        }
    }

    // skillDis 배열을 업데이트하는 헬퍼 함수
    void UpdateSkillDisArray(GameObject removedSkill)
    {
        List<GameObject> updatedList = new List<GameObject>(skillDis);

        // 삭제된 오브젝트 제외
        updatedList.Remove(removedSkill);

        skillDis = updatedList.ToArray();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && b_Absorption)
        {
            Debug.Log("플레이어 와 부딪침");
            for (int i = 0; i < skillCount.skillName.Length; i++)
            {

                if (skillCount.skillName[i] == gameObject.name)//이름이 같을 경우
                {
                    if (playerUI.f_CurseProgression >= playerUI.f_Maxf_CurseProgression - 99)
                    {
                        float curseRecovery = playerUI.f_Maxf_CurseProgression - playerUI.f_CurseProgression;
                        playerUI.f_CurseProgression += curseRecovery;
                    }
                    else
                    {
                        playerUI.f_CurseProgression += 100;
                    }
                    SoundManager.PlaySound(absorptionClip);
                    skillCount.skillAbsorptionCount[i] -= 1;
                    if(skillCount.skillAbsorptionCount[i] == 0)
                    {
                        skillCount.skillEnhanceCount[i] += 1;//강화
                    }
                    gameObject.SetActive(false); 
                    Destroy(gameObject,3);
                }
            }
        }
    }
#if UNITY_EDITOR
    // SerializedObject 메모리 관리 및 안전한 사용
    private void OnDestroy()
    {
        if (serializedObject != null)
        {
            serializedObject.Dispose(); // 메모리 해제
            serializedObject = null;     // 참조 해제
        }
    }
#endif
}
