using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : BaseSkill
{
    public Berserk(SkillTableSO.SkillInfo skillInfo) : base(skillInfo)
    {
    }
    public override bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        return true;
    }

    public override void Execute(SkillContext skillContext, int attack, int skillValue, bool isCritical)
    {
        BuffEffect go = Managers.Resource.Instantiate("Active_DamageIncrease", pooling: true).GetComponent<BuffEffect>();
        go.SetOwner(GameContainer.Instance.Player);
        Managers.PlayerManager.BuffSystem.AddActiveBuff(BuffType.AttackBuff, skillValue, skillInfo.duration);
    }
}
