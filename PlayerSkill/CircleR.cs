using System.Collections;
using UnityEngine;

public class CircleR : MonoBehaviour
{
    public bool b_circleR;
    public GameObject[] skill;
    public float circleR; //반지름
    public float deg; //각도
    public float f_randonDeg = 360;
    public float objSpeed; //원운동 속도
    public int objSize = 5;
    public GameObject player;
    void Update()
    {
        if (b_circleR)
        {
            deg += Time.deltaTime * objSpeed;
            if (deg < 360)
            {
                for (int i = 0; i < objSize; i++)
                {
                    var rad = Mathf.Deg2Rad * (deg + (i * (360 / objSize)));
                    var x = circleR * Mathf.Sin(rad);
                    var z = circleR * Mathf.Cos(rad);
                    skill[i].transform.position = transform.position + new Vector3(x, 1, z);
                    skill[i].transform.rotation = Quaternion.Euler(0, 0, (deg + (i * (360 / objSize))) * -1);
                }
            }
            else
            {
                //b_circleR = false;
                deg = 0;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                objSize = Mathf.Max(0, objSize - 1); // objSize가 음수가 되지 않도록 제한
                for (int i = 0; i <= skill.Length - 1; i++)
                {
                    if (skill[i] != null)
                    {
                        StartCoroutine(Shoot(i));
                    }
                }
            }
        }
    }
    IEnumerator Shoot(int i)
    {
        // 배열 유효성 검사
        if (skill == null || skill.Length == 0 || i < 0 || i >= skill.Length)
        {
            Debug.LogError($"Invalid index {i}. Skill array is not properly initialized.");
            yield break;
        }

        // 배열 요소 유효성 검사
        if (skill[i] == null)
        {
            Debug.LogError($"Skill at index {i} is null.");
            yield break;
        }

        // Rigidbody 컴포넌트 확인
        Rigidbody rigidbody = skill[i].GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            Debug.LogError($"Rigidbody is missing on skill[{i}] object.");
            yield break;
        }

        Debug.Log($"{i}: {skill[i].name}");

        // Rigidbody 속도 설정
        //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        yield return null;
        rigidbody.velocity = player.transform.forward * 30;
    }

}
