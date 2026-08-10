using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderCharge : BaseSkill
{
    public ThunderCharge(SkillTableSO.SkillInfo skillInfo) : base(skillInfo)
    {
    }
    public override bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        return true;
    }
    public override void Execute(SkillContext skillContext, int attack, int skillValue, bool isCritical)
    {
        ThunderChargeObject go = Managers.Resource.Instantiate("ThunderCharge", pooling: true).GetComponent<ThunderChargeObject>();
        int totalDamage = attack * skillValue / 100;
        Debug.Log(totalDamage + "데미지");
        go.Init(skillContext.Caster, totalDamage, isCritical);
    }
}
