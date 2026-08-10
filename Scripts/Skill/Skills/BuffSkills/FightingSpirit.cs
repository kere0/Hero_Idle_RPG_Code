using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingSpirit : BaseSkill
{
    public FightingSpirit(SkillTableSO.SkillInfo skillInfo) : base(skillInfo)
    {
    }
    public override int GetManaCost(int level)
    {
        return 0;
    }
    public override bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        return true;
    }

    public override void Execute(SkillContext skillContext, int attack, int skillValue, bool isCritical)
    {
        BuffEffect go = Managers.Resource.Instantiate("Passive_DamageIncrease", pooling: true).GetComponent<BuffEffect>();
        go.SetOwner(GameContainer.Instance.Player);
        Managers.PlayerManager.BuffSystem.AddPassiveBuff(BuffType.AttackBuff, skillValue);
    }
}
