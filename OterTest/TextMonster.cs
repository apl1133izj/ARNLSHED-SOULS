using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMonster : MonoBehaviour
{
    public float hp = 100000;
    public AttackAndCombo andCombo;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("R"))
        {
            hp -= 5;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HandAttack"))
        {
            Debug.Log("HandAttack");
            switch (AttackAndCombo.comboAttakccount)
            {
                case 0:
                    hp -= andCombo.attackDamage[0];
                    break;
                case 1:
                    hp -= andCombo.attackDamage[1];
                    break;
                case 2:
                    hp -= andCombo.attackDamage[2];
                    break;
                case 3:
                    hp -= andCombo.attackDamage[3];
                    break;
            }
        }
    }
}
