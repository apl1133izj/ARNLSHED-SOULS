using UnityEngine;
public class ShildAttackUI : MonoBehaviour
{
    public float f_shildTime;
    public GameObject attackImage;
    int i_count;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            i_count += 1;
        }
        if (i_count == 1)
        {
            f_shildTime += Time.deltaTime;
        }
        if (f_shildTime >= 4)
        {
            attackImage.SetActive(true);
            if (i_count == 2)
            {
                attackImage.SetActive(false);
                f_shildTime = 0f;
                i_count = 0;
            }
            if (f_shildTime >= 8)
            {
                attackImage.SetActive(false);
                f_shildTime = 0f;
                i_count = 0;
            }
        }
    }
}
