using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount == 2)
        {
            if (transform.GetChild(1) != null)
            {
                Destroy(transform.GetChild(1).gameObject);
            }
        }
    }
}
