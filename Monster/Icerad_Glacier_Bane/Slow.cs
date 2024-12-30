using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            DemoCharacter demoCharacter = other.GetComponent<DemoCharacter>();
            demoCharacter.abnormalstatus[0].SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DemoCharacter demoCharacter = other.GetComponent<DemoCharacter>();
            demoCharacter.abnormalstatus[0].SetActive(false);
        }
    }
}
