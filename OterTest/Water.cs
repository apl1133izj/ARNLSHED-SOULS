using UnityEngine;

public class Water : MonoBehaviour
{
    //맞았을때 히트 파티클(Demon_damaged)
    public GameObject g_HitFire;

    public GameObject g_WaterHitFire;
    private void OnParticleCollision(GameObject other)
    {
        if (other.transform.CompareTag("Luncher"))
        {
            Debug.Log("Luncher");
            Destroy(other.gameObject);
        }
        if (other.transform.CompareTag("Oil"))
        {
            Debug.Log("oil");
            GameObject hitPaticle = Instantiate(g_WaterHitFire, other.transform.position - new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
            Destroy(hitPaticle, 3);
        }
    }
}
