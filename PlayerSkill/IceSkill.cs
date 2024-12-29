using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IceSkill : MonoBehaviour//몬스터를 얼림
{
    public GameObject[] monsterSerch;
    float monsterDis;
    public Vector3[] monsterPos;
    bool isMoveIceSkill;
    void Start()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        if (monsters.Length == 0) // 예외 처리
        {
            return;
        }

        // 임시 리스트를 사용해 거리 조건에 맞는 몬스터만 저장
        List<GameObject> validMonsters = new List<GameObject>();

        foreach (GameObject monster in monsters)
        {
            if (monster == null) continue; // 예외 처리

            float disMonster = Vector3.Distance(gameObject.transform.position, monster.transform.position);
            if (disMonster <= 50) // 거리 조건
            {
                validMonsters.Add(monster);
            }
        }

        // 거리 조건에 맞는 몬스터들로 배열 초기화
        monsterSerch = validMonsters.ToArray();
        monsterPos = new Vector3[monsterSerch.Length];

        // 몬스터 위치 저장
        for (int i = 0; i < monsterSerch.Length; i++)
        {
            monsterPos[i] = monsterSerch[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < monsterSerch.Length; i++)
        {
            monsterPos[i] = monsterSerch[i].transform.position + new Vector3(0, 5, 0);
        }
        if (!isMoveIceSkill)
        {
            StartCoroutine(MonsterMove());
        }
    }

    IEnumerator MonsterMove()
    {
        isMoveIceSkill = true;
        for (int i = 0; i < monsterSerch.Length; i++)
        {
            while (Vector3.Distance(gameObject.transform.position, monsterPos[i]) > 0.1f)
            {
                // MoveTowards의 반환 값을 위치에 적용
                gameObject.transform.position = Vector3.MoveTowards(
                    gameObject.transform.position, monsterPos[i], 30f * Time.deltaTime
                );
                yield return null;
            }
            yield return new WaitForSeconds(2);
        }
        Destroy(gameObject);
        Debug.Log("종료");
    }
}
