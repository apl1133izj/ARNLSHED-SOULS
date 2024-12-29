using UnityEngine;

public class SkillTreeControll : MonoBehaviour
{
    public PlayerUI playerUI;
    public int bc;
    bool skillBookState;
    public GameObject skillBookGameObject;
    void Update()
    {
        SkillBookUI();
    }
    void SkillBookUI()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            bc++;
        }
        if (bc == 1)
        {
            playerUI.uiOpen = true;
            skillBookGameObject.SetActive(true);
        }
        else if(bc == 2) 
        {
            playerUI.uiOpen = false;
            skillBookGameObject.SetActive(false);
        }
        else if(bc == 3)
        {
            bc = 1;
        }
    }
}
