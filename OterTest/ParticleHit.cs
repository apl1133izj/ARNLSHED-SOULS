using System.Collections;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    public GameObject hitObject;
    GameObject lookAtPlayer;

    private void Start()
    {
        
    }
    IEnumerator Hit()
    {
        hitObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
    void LookAtPlayer(GameObject playerRotation)
    {
        Vector3 direction = playerRotation.transform.position - transform.position;
        direction.y = 0;
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        }
    }
    private void Update()
    {
       /* lookAtPlayer = GameObject.Find("Player");
        Debug.Log(lookAtPlayer);
        Debug.Log(transform.position);
        transform.position = Vector3.MoveTowards(transform.position, lookAtPlayer.transform.position,  10 * Time.deltaTime);*/
        //LookAtPlayer(lookAtBoneDragon);
        //this.transform.LookAt(lookAtBoneDragon.transform.position);
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("hit");
            StartCoroutine(Hit());
            Destroy(gameObject);
        }
    }
}
