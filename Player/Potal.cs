using UnityEngine;

public class Potal : MonoBehaviour
{
    public Transform potalPos;
    public CharacterController characterController;
    public bool dungenon1_2Go;
    public GameObject[] map; //0:신전 1: 포탈방 2: 1던전 3:2던전 4:보스 방
    public bool b_temple;
    public bool b_potalRoom;
    public bool b_Darkmap;
    public bool[] b_Bose;
    public DemoCharacter demoCharacter;
    public AudioClip potalSound;
    public QuestManager questManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.PlaySound(potalSound);
            Debug.Log(other.gameObject.name);
            other.transform.localPosition = potalPos.position;
            characterController.enabled = false;
            if (b_temple)
            {
                map[0].SetActive(true);
                questManager.questArea = false;
                demoCharacter.floorArer = DemoCharacter.FloorArer.Temple;
            }
            else if (b_potalRoom)
            {
                questManager.questArea = true;
                demoCharacter.floorArer = DemoCharacter.FloorArer.Dungeon;
                map[1].SetActive(true);
                map[2].SetActive(false);
                map[3].SetActive(false);

            }
            else if (b_Darkmap)
            {
                demoCharacter.floorArer = DemoCharacter.FloorArer.Dungeon;
                map[1].SetActive(false);
                map[2].SetActive(true);
            }

            if (dungenon1_2Go)
            {
                demoCharacter.floorArer = DemoCharacter.FloorArer.Dungeon;
                map[1].SetActive(false);
                map[3].SetActive(true);
                other.gameObject.transform.localScale = new Vector3(2, 2, 2);
            }
            else
            {
                demoCharacter.floorArer = DemoCharacter.FloorArer.Dungeon;
                other.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            if (b_Bose[0])
            {
                demoCharacter.floorArer = DemoCharacter.FloorArer.BoseRoom;
                map[3].SetActive(false);
                map[4].SetActive(true);
            }
            else if(b_Bose[1])
            {
                demoCharacter.floorArer = DemoCharacter.FloorArer.BoseRoom;
                map[2].SetActive(false);
                map[5].SetActive(true);
            }
            else if (b_Bose[2])
            {
                demoCharacter.floorArer = DemoCharacter.FloorArer.BoseRoom;
                map[0].SetActive(false);
                map[6].SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!b_temple)
            {
                map[0].SetActive(false);
            }
            else if(!b_potalRoom)
            {
                map[1].SetActive(false);
            }
            else if(!b_Darkmap) 
            {
                map[2].SetActive(false);
            }
            else if(!dungenon1_2Go)
            {
                map[3].SetActive(false);    
            }
            if (!b_Bose[0])
            {
                map[4].gameObject.SetActive(false);
            }
            other.transform.localPosition = potalPos.position;
        }
    }
}
