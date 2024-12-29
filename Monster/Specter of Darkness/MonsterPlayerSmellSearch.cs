using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MonsterPlayerSmellSearch : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // NavMeshAgent가 NavMesh에 있고 활성화되어 있는지 먼저 확인
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            StartCoroutine(PlayerSearch());
        }
        else
        {
            Debug.LogError("NavMeshAgent가 활성화되지 않았거나 NavMesh에 배치되지 않았습니다.");
        }
    }

    private IEnumerator PlayerSearch()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (agent != null && agent.isOnNavMesh && agent.isActiveAndEnabled)
            {
                agent.SetDestination(player.position);
                float remainingDistance = agent.remainingDistance;
                Debug.Log("남은 거리: " + remainingDistance);

                if (remainingDistance < 2f)
                {
                    Debug.Log("플레이어에 접근함");
                    // 추가 로직 작성
                }
            }
            else
            {
                Debug.LogWarning("에이전트가 NavMesh에 없습니다.");
            }
        }
    }
}
