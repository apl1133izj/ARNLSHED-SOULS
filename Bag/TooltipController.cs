using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TooltipController : MonoBehaviour
{
    public GameObject tooltipPanel;
    public Text tooltipNameText;
    public Text tooltipLevelText;
    public Text tooltipNextLevelText;
    public Text tooltipCommendText;
    public Text tooltipDetailsCommendText;
    public void ShowTooltip(string name, string level, string NextLevel , string commend, string detailscommend)
    {
        tooltipPanel.SetActive(true);
        tooltipNameText.text = name;
        tooltipLevelText.text = level;
        tooltipNextLevelText.text = NextLevel;
        tooltipCommendText.text = "\t\t" + commend;
        tooltipDetailsCommendText.text = detailscommend;
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    void Start()
    {
        HideTooltip();
    }
}
