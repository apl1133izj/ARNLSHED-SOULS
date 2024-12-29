using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum skillTreeTooltip
    {
        Hp, Stamina, RecoverySpeed, ReduceCoolTime, PotionEffect, CurseReduction, StaminaRecovery, AttackPower,
        DamageReduction, MoveSpeed, AttackSpeed, StaminaCostReduction, AttackRange, UltimateSkill
    };
    public skillTreeTooltip skillname;
    int skilLevel;
    string tname;
    string tLevel;
    string tNextLevel;
    string tCommend;
    string tDetailsCommend;
    private TooltipController tooltipController;
    SkillTree skillTree;
    void Start()
    {
        tooltipController = GetComponent<TooltipController>();
        skillTree = GetComponentInParent<SkillTree>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillLevel();
        tooltipController.ShowTooltip(tname, tLevel, tNextLevel, tCommend, tDetailsCommend);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipController.HideTooltip();
    }
    /// <summary>
    /// 스킬레벨에 따라 스펙증가
    /// </summary>
    public void SkillLevel()
    {
        if (skillname == skillTreeTooltip.Hp)
        {
            switch (skillTree.skillLevel[0])
            {
                case 1:
                    tname = "체력 증진\n";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = " 체력이 증가합니다." +
                             "\n\t\t 체력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "1000    ->     1200\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "체력 증진\n";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = " 체력이 증가합니다." +
                             "\n\t\t 체력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->      Lv3\n"
                                     + "1200    ->      1500\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "체력 증진\n";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = " 체력이 증가합니다." +
                             "\n\t\t 체력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->      Lv4\n"
                                     + "1500    ->      1900\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "체력 증진\n";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = " 체력이 증가합니다." +
                             "\n\t\t 체력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5      ->     최고 레벨\n"
                                     + "1900     ->        2400\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "체력 증진\n";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨\n";
                    tCommend = " 체력이 증가합니다.\n" +
                             "\n\t\t 체력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다\n";
                    tDetailsCommend = "최고 레벨 달성\n"
                                     + "체력 증진 :2400        \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }

        }
        if (skillname == skillTreeTooltip.Stamina)
        {
            switch (skillTree.skillLevel[1])
            {
                case 1:
                    tname = "기력 증진";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = " 기력이 증가합니다." +
                             "\n\t\t 기력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "1000    ->     1200\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "기력 증진";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = " 기력이 증가합니다." +
                             "\n\t\t 기력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + "1200    ->     1500\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "기력 증진";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = " 기력이 증가합니다." +
                             "\n\t\t 기력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "1500    ->     1900\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "기력 증진";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = " 기력이 증가합니다." +
                             "\n\t\t 기력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "1900    ->        2400\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "기력 증진";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = " 기력이 증가합니다." +
                             "\n\t\t 기력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = "최고 레벨 달성\n"
                                     + "기력 증진 : 2400       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다\n";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.RecoverySpeed)
        {
            switch (skillTree.skillLevel[2])
            {
                case 1:
                    tname = "체력 재생 효과 증가";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = "체력 재생 효과 증가가 합니다." +
                             "\n\t\t 체력 재생 효과 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + " 1      ->      3\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "체력 재생 효과 증가";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = "체력 재생 효과 증가가 합니다." +
                             "\n\t\t 체력 재생 효과 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + " 3      ->      6\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "체력 재생 효과 증가";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = "체력 재생 효과 증가가 합니다." +
                             "\n\t\t 체력 재생 효과 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + " 6      ->      10\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "체력 재생 효과 증가";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = " 체력 재생 효과 증가가 합니다." +
                             "\n\t\t 체력 재생 효과 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "10      ->        15\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "체력 재생 효과 증가";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = " 체력 재생 효과 증가가 합니다." +
                             "\n\t\t 체력 재생 효과 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 달성         \n"
                                     + "체력 재생 효과 : 15                 \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.ReduceCoolTime)
        {
            switch (skillTree.skillLevel[3])
            {
                case 1:
                    tname = "재사용 대기시간 감소";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = " 재사용 대기시간이 감소 합니다." +
                             "\n\t\t 재사용 대기시간 감소는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "0.2초   ->     0.6초\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "재사용 대기시간 감소";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = "재사용 대기시간이 감소 합니다." +
                             "\n\t\t 재사용 대기시간 감소는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2      ->     Lv3\n"
                                     + "0.6초    ->     1.2초\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "재사용 대기시간 감소";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = "재사용 대기시간이 감소 합니다." +
                             "\n\t\t 재사용 대기시간 감소는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "1.2초   ->     2.0초\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "재사용 대기시간 감소";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = "재사용 대기시간이 감소 합니다." +
                             "\n\t\t 재사용 대기시간 감소는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "2.0초   ->     3.0초\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "재사용 대기시간 감소";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = "재사용 대기시간이 감소 합니다." +
                             "\n\t\t 재사용 대기시간 감소는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 달성         \n"
                                     + "재사용 대기시간 : 3.0초       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.PotionEffect)
        {
            switch (skillTree.skillLevel[4])
            {
                case 1:
                    tname = "포션 효과 증가";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = " 포션 효과가 증가합니다." +
                             "\n\t\t 포션 효과가 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->      Lv2\n"
                                     + "50      ->      150\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "포션 효과 증가";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = " 포션 효과가 증가합니다." +
                             "\n\t\t 포션 효과가 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + "150     ->     300\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "포션 효과 증가";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = " 포션 효과가 증가합니다." +
                             "\n\t\t 포션 효과가 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "300     ->     500\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "포션 효과 증가";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = " 포션 효과가 증가합니다." +
                             "\n\t\t 포션 효과가 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "500     ->     750\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "포션 효과 증가";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = " 포션 효과가 증가합니다." +
                             "\n\t\t 포션 효과가 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 달성         \n"
                                     + "포션 효과 증가 : 750       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.CurseReduction)
        {
            switch (skillTree.skillLevel[5])
            {
                case 1:
                    tname = "저주 효과 감소";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = "저주 효과 감소가 증가합니다." +
                             "\n\t\t 저주 효과 감소가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "0.025   ->     0.075\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "저주 효과 감소";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = "저주 효과 감소가 증가합니다." +
                             "\n\t\t 저주 효과 감소가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + "0.075   ->     0.15\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "저주 효과 감소";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = "저주 효과 감소가 증가합니다." +
                             "\n\t\t 저주 효과 감소가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "0.15    ->     0.25\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "저주 효과 감소";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = "저주 효과 감소가 증가합니다." +
                             "\n\t\t 저주 효과 감소가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "0.25    ->     0.375\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "저주 효과 감소";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = "저주 효과 감소가 증가합니다." +
                             "\n\t\t 저주 효과 감소가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 달성         \n"
                                     + "저주효과 감소 : 0.375       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.StaminaRecovery)
        {
            switch (skillTree.skillLevel[6])
            {
                case 1:
                    tname = "기력 재생 효과 증가";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = "기력 재생 효과 증가가 합니다." +
                             "\n\t\t 기력 재생 효과 증가는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "1       ->      3\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "기력 재생 효과 증가";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = "기력 재생 효과 증가가 증가합니다." +
                             "\n\t\t 기력 재생 효과 증가는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + "3       ->     6\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "기력 재생 효과 증가";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = "기력 재생 효과 증가가." +
                             "\n\t\t 기력 재생 효과 증가는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "6       ->      10\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "기력 재생 효과 증가";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = "기력 재생 효과 증가가." +
                             "\n\t\t 기력 재생 효과 증가는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "10      ->         15\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "기력 재생 효과 증가";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = "기력 재생 효과 증가가." +
                             "\n\t\t 기력 재생 효과 증가는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 당성          \n"
                                     + "기력 재생 : 15       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.AttackPower)
        {
            switch (skillTree.skillLevel[7])
            {
                case 1:
                    tname = "공격력 증가";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = " 공격력이 증가 합니다." +
                             "\n\t\t 공격력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "1       ->      3\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "공격력 증가";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = " 공격력이 증가합니다." +
                             "\n\t\t 공격력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + "3       ->      6\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "공격력 증가";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = " 공격력이 증가합니다." +
                             "\n\t\t 공격력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "6       ->      10\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "공격력 증가";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = " 공격력이 증가합니다." +
                             "\n\t\t 공격력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "10      ->         15\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "공격력 증가";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = "공격력이 증가합니다" +
                             "\n\t\t 공격력 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 달성          \n"
                                     + "공격력 : 15       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.DamageReduction)
        {
            switch (skillTree.skillLevel[8])
            {
                case 1:
                    tname = "피해량 감소";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = "피해량 감소가 증가합니다." +
                             "\n\t\t 피해량 감소가 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "2       ->      6\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "피해량 감소";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = " 피해량 감소가 증가합니다." +
                             "\n\t\t 피해량 감소가 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->      Lv3\n"
                                     + "6       ->       12\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "피해량 감소";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = " 피해량 감소가 증가합니다." +
                             "\n\t\t 피해량 감소가 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "12      ->     20\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "피해량 감소";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = " 피해량 감소가 증가합니다." +
                             "\n\t\t 피해량 감소가 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "20      ->        30\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "피해량 감소";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = " 피해량 감소가 증가합니다." +
                             "\n\t\t 피해량 감소가 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 달성         \n"
                                     + "피해 감소 : 30       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.MoveSpeed)
        {
            switch (skillTree.skillLevel[9])
            {
                case 1:
                    tname = "이동 속도 증가";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = " 이동 속도가 증가합니다." +
                             "\n\t\t 이동 속도 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "0.15    ->     0.45\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "이동 속도 증가";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = " 이동 속도가 증가합니다." +
                             "\n\t\t 이동 속도 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + "0.45    ->     0.9\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "이동 속도 증가";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = " 이동 속도가 증가합니다." +
                             "\n\t\t 이동 속도 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "0.9     ->     1.5\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "이동 속도 증가";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = " 이동 속도가 증가합니다." +
                             "\n\t\t 이동 속도 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "1.5     ->        2.25\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "이동 속도 증가";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = " 이동 속도가 증가합니다." +
                             "\n\t\t 이동 속도 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 달성         \n"
                                     + "이동속도 : 2.25       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.AttackSpeed)
        {
            switch (skillTree.skillLevel[10])
            {
                case 1:
                    tname = "공격 속도 증가";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = "공격 속도가 증가합니다." +
                             "\n\t\t 공격 속도 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "0.03    ->     0.09\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "공격 속도 증가";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = " 공격 속도가 증가합니다." +
                             "\n\t\t 공격 속도 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + "0.09    ->     0.18\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "공격 속도 증가";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = " 공격 속도가 증가합니다." +
                             "\n\t\t 공격 속도 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "0.18    ->     0.3\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "공격 속도 증가";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = " 공격 속도가 증가합니다." +
                             "\n\t\t 공격 속도 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "0.3     ->        0.45\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "공격 속도 증가";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = " 공격 속도가 증가합니다." +
                             "\n\t\t 공격 속도 증가량은 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 달성         \n"
                                     + "공격 속도 : 0.45       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.StaminaCostReduction)
        {
            switch (skillTree.skillLevel[11])
            {
                case 1:
                    tname = "기력 사용 비용 감소";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = "기력 사용 비용이 감소 합니다." +
                             "\n\t\t 기력 사용 비용이 증가량은 Level에 따라 점차 감소 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "5       ->     15\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "기력 사용 비용 감소";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = "기력 사용 비용이 감소 합니다." +
                             "\n\t\t 기력 사용 비용이 증가량은 Level에 따라 점차 감소 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + "15      ->     30\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "기력 사용 비용 감소";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = "기력 사용 비용이 감소 합니다." +
                             "\n\t\t 기력 사용 비용이 증가량은 Level에 따라 점차 감소 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "30      ->     50\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "기력 사용 비용 감소";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = "기력 사용 비용이 감소 합니다." +
                             "\n\t\t 기력 사용 비용이 증가량은 Level에 따라 점차 감소 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->    최고 레벨\n"
                                     + "50      ->        75\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "기력 사용 비용 감소";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = "기력 사용 비용이 감소 합니다." +
                             "\n\t\t 기력 사용 비용이 증가량은 Level에 따라 점차 감소 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 달성         \n"
                                     + "기력 사용 비용 : 75       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.AttackRange)
        {
            switch (skillTree.skillLevel[12])
            {
                case 1:
                    tname = "공격 범위 증가";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = " 공격 범위가 증가합니다." +
                             "\n\t\t 공격 범위는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "1       ->      2\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "공격 범위 증가";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = " 공격 범위가 증가합니다." +
                             "\n\t\t 공격 범위는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + "2       ->     3\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "공격 범위 증가";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = " 공격 범위가 증가합니다." +
                             "\n\t\t 공격 범위는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->     Lv4\n"
                                     + "3       ->      4\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "공격 범위 증가";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = " 기공격 범위가력이 증가합니다." +
                             "\n\t\t 공격 범위는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->     최고 레벨\n"
                                     + "4       ->         5\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "공격 범위 증가";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = " 공격 범위가 증가합니다." +
                             "\n\t\t 공격 범위는 Level에 따라 점차 증가 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " 최고 레벨 달성         \n"
                                     + "사거리 : 5       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
        if (skillname == skillTreeTooltip.UltimateSkill)
        {
            switch (skillTree.skillLevel[12])
            {
                case 1:
                    tname = "궁극기 생성";
                    tLevel = "1";
                    tNextLevel = "2";
                    tCommend = "R을 눌러 궁극기 를 사용 할 수 있습니다." +
                             "\n\t\t 재사용 대기시간은 Level에 따라 점차 감소 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv1     ->     Lv2\n"
                                     + "20    ->       19\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 2:
                    tname = "궁극기[R] 쿨타임 감소";
                    tLevel = "2";
                    tNextLevel = "3";
                    tCommend = "궁극기의 재사용 대기시간이 감소합니다" +
                             "\n\t\t 재사용 대기시간은 Level에 따라 점차 감소 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv2     ->     Lv3\n"
                                     + "19     ->      17\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 3:
                    tname = "궁극기[R] 쿨타임 감소";
                    tLevel = "3";
                    tNextLevel = "4";
                    tCommend = "궁극기의 재사용 대기시간이 감소합니다" +
                             "\n\t\t 재사용 대기시간은 Level에 따라 점차 감소 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv3     ->    Lv4\n"
                                     + "17    ->      14\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 4:
                    tname = "궁극기[R] 쿨타임 감소";
                    tLevel = "4";
                    tNextLevel = "5";
                    tCommend = "궁극기의 재사용 대기시간이 감소합니다" +
                             "\n\t\t 재사용 대기시간은 Level에 따라 점차 감소 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = " Lv5     ->    최고 레벨\n"
                                     + "14    ->         9\n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
                case 5:
                    tname = "궁극기[R] 쿨타임 감소";
                    tLevel = "5";
                    tNextLevel = "           최고 레벨";
                    tCommend = "궁극기의 재사용 대기시간이 감소합니다" +
                             "\n\t\t 재사용 대기시간은 Level에 따라 점차 감소 \n\t\t 하며 Level이 MAX일경우 추가 효과를\n" +
                             "\t\t 획득합니다";
                    tDetailsCommend = "최고 레벨 달성         \n"
                                     +"재새용 대시 시간 : 9       \n" +
                                      "SkillLevel에 따라 SkillPoint사용량\n이 증가합니다";
                    break;
            }
        }
    }
}
