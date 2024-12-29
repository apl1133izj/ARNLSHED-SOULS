using TMPro;
using UnityEngine;

public class ItemToolTipOpen : MonoBehaviour
{
    public ItemToolTip toolTip;
    public int tooltipCount = 0;
    public GameObject itemToolTipGameObject;
    public TextMeshProUGUI keyCheckText;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            tooltipCount += 1;
        }

        if (tooltipCount == 0)
        {
            toolTip.enabled = false;
            itemToolTipGameObject.SetActive(false);
        }
        else if (tooltipCount == 1)
        {
            toolTip.enabled = true;
            keyCheckText.gameObject.SetActive(false);
            itemToolTipGameObject.SetActive(true);
        }
        else if (tooltipCount == 2)
        {
            tooltipCount = 0;
        }
    }
}
