using TMPro;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public Transform[] TeleportationPos;
    public TMP_Dropdown dropdown;
    public GameObject player;
    public DemoCharacter character;
    public HP hp_s;
    public Stamina stamina_s;
    public CharacterController characterController;
    public GameObject[] Map;
    public PlayerUI playerUI;

    void Invokes()
    {
        characterController.enabled = true;
    }
    public void Teleportation()
    {

        if (dropdown.value == 0)
        {
            Debug.Log("치트 사용X");
        }
        if (dropdown.value == 1)
        {
            hp_s.hp = hp_s.hpMax;
            stamina_s.stamina = stamina_s.staminaMax;
            playerUI.f_CurseProgression = playerUI.f_Maxf_CurseProgression;
            character.SizeSpeed();
            player.transform.localPosition = TeleportationPos[0].transform.localPosition;
            stamina_s.staminaMax = 5000f;
            characterController.enabled = false;
            character.floorArer = DemoCharacter.FloorArer.Temple;
            Invoke("Invokes", 0.2f);
            for (int i = 0; i <= 5; i++)
            {
                if (i == 0)
                {           
                    Map[i].gameObject.SetActive(true);
                }
                else
                {
                    Map[i].gameObject.SetActive(false);
                }
            }
        }
        if (dropdown.value == 2)
        {
            hp_s.hp = hp_s.hpMax;
            stamina_s.stamina = stamina_s.staminaMax;
            character.floorArer = DemoCharacter.FloorArer.Dungeon;
            playerUI.f_CurseProgression = playerUI.f_Maxf_CurseProgression;
            stamina_s.staminaMax = 5000f;
            character.SizeSpeed();
            characterController.enabled = false;
            Invoke("Invokes", 0.2f);
            for (int i = 0; i <= 5; i++)
            {
                if (i == 1)
                {
                    Map[i].gameObject.SetActive(true);
                }
                else
                {
                    Map[i].gameObject.SetActive(false);
                }
            }
            player.transform.localScale = new Vector3(1, 1, 1);
            player.transform.localPosition = TeleportationPos[1].transform.localPosition;
        }
        if (dropdown.value == 3)
        {
            hp_s.hp = hp_s.hpMax;
            stamina_s.stamina = stamina_s.staminaMax;
            character.floorArer = DemoCharacter.FloorArer.BoseRoom;
            playerUI.f_CurseProgression = playerUI.f_Maxf_CurseProgression;
            stamina_s.staminaMax = 5000f;
            character.SizeSpeed();
            characterController.enabled = false;
            Invoke("Invokes", 0.2f);
            for (int i = 0; i <= 5; i++)
            {
                if (i == 2)
                {
                    Map[i].gameObject.SetActive(true);
                }
                else
                {
                    Map[i].gameObject.SetActive(false);
                }
            }
            player.transform.localScale = new Vector3(2, 2, 2);
            player.transform.localPosition = TeleportationPos[2].transform.localPosition;
        }
        if (dropdown.value == 4)
        {
            hp_s.hp = hp_s.hpMax;
            stamina_s.stamina = stamina_s.staminaMax;
            playerUI.f_CurseProgression = playerUI.f_Maxf_CurseProgression;
            character.floorArer = DemoCharacter.FloorArer.BoseRoom;
            stamina_s.staminaMax = 5000f;
            player.transform.localScale = new Vector3(1, 1, 1);
            characterController.enabled = false;
            Invoke("Invokes", 0.2f);
            for (int i = 0; i <= 5; i++)
            {
                if (i == 3)
                {
                    Map[i].gameObject.SetActive(true);
                }
                else
                {
                    Map[i].gameObject.SetActive(false);
                }
            }
            player.transform.localPosition = TeleportationPos[3].transform.localPosition;
        }
        if (dropdown.value == 5)
        {
            hp_s.hp = hp_s.hpMax;
            stamina_s.stamina = stamina_s.staminaMax;
            playerUI.f_CurseProgression = playerUI.f_Maxf_CurseProgression;
            character.floorArer = DemoCharacter.FloorArer.BoseRoom;
            stamina_s.staminaMax = 5000f;
            player.transform.localScale = new Vector3(1, 1, 1);
            characterController.enabled = false;
            player.transform.localPosition = TeleportationPos[4].transform.localPosition;
            Invoke("Invokes", 1f);
            for (int i = 0; i <= 5; i++)
            {
                if (i == 4)
                {
                    Map[i].gameObject.SetActive(true);
                }
                else
                {
                    Map[i].gameObject.SetActive(false);
                }
            }
           // hp_s.hp = hp_s.hpMax;
        }
        if (dropdown.value == 6)
        {
            hp_s.hp = hp_s.hpMax;
            stamina_s.stamina = stamina_s.staminaMax;
            stamina_s.staminaMax = 5000f;
            playerUI.f_CurseProgression = playerUI.f_Maxf_CurseProgression;
            character.floorArer = DemoCharacter.FloorArer.BoseRoom;
            player.transform.localScale = new Vector3(1, 1, 1);
            characterController.enabled = false;
            player.transform.localPosition = TeleportationPos[5].transform.localPosition;
            Invoke("Invokes", 1f);
            for (int i = 0; i <= 5; i++)
            {
                if (i == 5)
                {
                    Map[i].gameObject.SetActive(true);
                }
                else
                {
                    Map[i].gameObject.SetActive(false);
                }
            }
            //stamina_s.stamina = stamina_s.staminaMax;
        }
    }
    
}
