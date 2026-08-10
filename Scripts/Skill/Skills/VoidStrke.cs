using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidStrke : BaseSkill
{
    public VoidStrke(SkillTableSO.SkillInfo skillInfo) : base(skillInfo)
    {
    }

    public override bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        
        if (skillContext.Target == null) 
        { 
            Debug.Log("타겟이 없습니다"); return false;
        }
        return true;
    }
    public override void Execute(SkillContext skillContext, int attack, int skillValue, bool isCritical)
    {
        if (skillContext.Target == null) return;
        int totalDamage = attack * skillValue / 100;
        VoidStrikeObject go = Managers.Resource.Instantiate("VoidStrikeObject", pooling: true).GetComponent<VoidStrikeObject>();
        go.Init(skillContext.Caster.EffectPos.position, totalDamage , isCritical);

    }
}
