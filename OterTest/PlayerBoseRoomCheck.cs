using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoseRoomCheck : MonoBehaviour
{
    public GameObject bose;
    public bool playerCheck;
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerCheck = true;
            bose.gameObject.SetActive(true);
        }
    }
}
