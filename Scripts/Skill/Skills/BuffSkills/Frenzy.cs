using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frenzy : BaseSkill
{
    public Frenzy(SkillTableSO.SkillInfo skillInfo) : base(skillInfo)
    {
    }
    public override bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        return true;
    }
    public override void Execute(SkillContext skillContext, int attack, int skillValue, bool isCritical)
    {
        BuffEffect go = Managers.Resource.Instantiate("Active_AttackSpeedIncrease", pooling: true).GetComponent<BuffEffect>();
        go.SetOwner(GameContainer.Instance.Player);
        Managers.PlayerManager.BuffSystem.AddActiveBuff(BuffType.AttackSpeedBuff, skillValue, skillInfo.duration);
    }
}
