using System.Collections;
using UnityEngine;

public class SpawEnemy : MonoBehaviour
{

    /*
    고블린 전사  Goblin Warrior
    어둠의 늑대 Dark Wolf
    스켈레톤 궁수 Skeleton Archer
    불타는 좀비 Burning Zombie
    독침 전갈 Poison Stinger Scorpion
    미노타우로스 Minotaur
    서리 거미 Frost Spider
    망령 기사 Wraith Knight
    바위 골렘 Stone Golem
    맹독 슬라임 Venomous Slime
    저주받은 나무정령 Cursed Treant
    혈흔 박쥐 Bloodstain Bat
    맹수 하피  Savage Harpy
    그림자 망령 Savage Harpy
    야수 거미 Beast Spider
    용암 정령 Lava Elemental
    얼음 거인 Ice Giant
    돌연변이 오우거 Mutant Ogre
    방랑하는 좀비 Wandering Zombie
    망자 군단병 Undead Legionnaire
    */



    public enum enemyType { Zombi, DarkWolf, SkeletonArcher };//
    public enemyType enemySpawType;
    public GameObject[] enemySpawIns;//소환 가능한 몬스터 종류
    public int enemyMaxSpaw;  //맵에 최대로 소환 가능한 몬스터의 수
    private bool isSpawning;
    [SerializeField]
    float spawTimeDelay;
    float spawPos;
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemySpawType.ToString());
        if (enemies.Length <= enemyMaxSpaw && !isSpawning)
        {
            StartCoroutine(EnemySpawCoroutine(spawTimeDelay));
        }
    }

    IEnumerator EnemySpawCoroutine(float spawTime)
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawTime);
        EnemySpawIns();
        isSpawning = false;
    }
    void EnemySpawIns()
    {
        Instantiate(enemySpawIns[(int)enemySpawType], new Vector3(transform.position.x + spawPos,
                    transform.position.y, transform.position.z + spawPos), Quaternion.identity);
        spawPos = Random.Range(3, 5f);
    }
}
