using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;

public class skill3Inst : MonoBehaviour
{

    public GameObject skill3,arear;
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(Skill3Inst());
    }
    IEnumerator Skill3Inst()
    {
        GameObject skill3ArerInst = Instantiate(arear, player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(4);
        GameObject skill3s = Instantiate(skill3, new Vector3(player.transform.position.x, player.transform.position.y + 25, player.transform.position.z), Quaternion.identity);
        skill3s.transform.Rotate(new Vector3(90, 0, 0));
        yield return new WaitForSeconds(1);
        Destroy(skill3ArerInst); 
        yield return new WaitForSeconds(3);
        Destroy(skill3s);
    }
}
