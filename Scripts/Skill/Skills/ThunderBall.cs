using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBall : BaseSkill
{
    public ThunderBall(SkillTableSO.SkillInfo skillInfo) : base(skillInfo)
    {
    }
    public override bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        if (isAuto == true)
        {
            if (skillContext.Target == null)
            {
                Debug.Log("타겟이 없습니다");
                return false;
            }
        }
        return true;
    }
    public override void Execute(SkillContext skillContext, int attack, int skillValue, bool isCritical)
    {
        ThunderBallObject go = Managers.Resource.Instantiate("ThunderBall", pooling: true).GetComponent<ThunderBallObject>();
        int totalDamage = attack * skillValue / 100;
        Debug.Log(totalDamage + "데미지");
        go.Init(skillContext.Caster.EffectPos.position, skillContext.Caster.EffectPos.right, totalDamage , isCritical);
    }
}
