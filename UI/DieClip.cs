using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DieClip : MonoBehaviour
{
    public HP hp;
    public DemoCharacter demoCharacter;
    public CharacterController characterController;

    public void NoonMove()
    {
        Debug.Log("NoonMove");
        characterController.enabled = false;
        demoCharacter.enabled = false;
        DemoCharacter.noneSpeed = true;
        hp.isDie = true;
    }

    public void Die_Clip()
    {
        Debug.Log("Die_Clip");
        characterController.enabled = true;
        hp.InvokeReStart();
    }
    
}
