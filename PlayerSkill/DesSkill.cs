using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesSkill : MonoBehaviour
{
    public AudioClip hitClip;
    public GameObject hitEffect;
    void Start()
    {
        Destroy(gameObject,5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 8 && other.gameObject.layer != 9)
        {
            SoundManager.PlaySound(hitClip);
            GameObject hitsEffect = Instantiate(hitEffect,transform.position,Quaternion.identity);
            Destroy(hitsEffect,2);
            Destroy(gameObject);
        }
    }
}
