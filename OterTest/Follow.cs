using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject darkKnight;
    void LateUpdate()
    {
        transform.position =  new Vector3(darkKnight.transform.position.x,transform.position.y, darkKnight.transform.position.z);
    }
}
