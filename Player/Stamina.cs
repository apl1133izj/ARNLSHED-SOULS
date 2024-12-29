using System.IO;//외부 파일 가져오기 json
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Stamina : MonoBehaviour
{
    public Image staminaBar;
    public float staminaMax = 1000f;//상점 에서 스팩증가 스택권을 사면 최대 최력 증가 
    public float stamina = 1000f;
    public float staminaRegeneration;

    public bool staminaRegenerationBool;
    SkillItemFusion skillItemFusion;
    public SkillTree skillTree;
    private void Start()
    {
        skillItemFusion = GetComponent<SkillItemFusion>();
        string filePath = DataManager.instance.path + DataManager.instance.nowSlot.ToString();
        if (File.Exists(filePath))
        {
            stamina = DataManager.instance.nowPlayer.staminaData;
            staminaMax = DataManager.instance.nowPlayer.staminaMaxData;
        }else
        {
            stamina = 1000f;
        }
    }
    private void Update()
    {

        DataManager.instance.nowPlayer.staminaData = stamina;
        DataManager.instance.nowPlayer.staminaMaxData = staminaMax;

        staminaMax = 1000 + (skillItemFusion.staminaHap.Take(6).Sum() + skillTree.staminaStack);
        staminaBar.fillAmount = stamina / staminaMax;
        StaminaRegeneration();
    }

    void StaminaRegeneration()
    {
        if (stamina < staminaMax)//플레이어 스테미너가 maximumSteminerRecovery 보다 클경우 아래 코드 실행
        {
            stamina += 0.1f + (skillItemFusion.staminaRegenerationHap.Take(6).Sum() + skillTree.staminaRecoveryStack) * Time.deltaTime;
        }
    }
}
