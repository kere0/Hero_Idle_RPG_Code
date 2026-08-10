using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionStrike : BaseSkill
{
    public ExplosionStrike(SkillTableSO.SkillInfo skillInfo) : base(skillInfo)
    {
    }
    public override bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        if (skillContext.Target == null)
        {
            Debug.Log("타겟이 없습니다");
            return false;
        }
        return true;
    }
    public override void Execute(SkillContext skillContext, int attack, int skillValue, bool isCritical)
    {
        if (skillContext.Target == null) return;
        ExplosionEffect go = Managers.Resource.Instantiate("Explosion", pooling: true).GetComponent<ExplosionEffect>();
        go.Init(skillContext.Target.EffectPos.position);
        int totalDamage = attack * skillValue / 100;
        CombatEvent combatEvent = new CombatEvent()
        {
            Receiver = skillContext.Target,
            Damage = totalDamage,
            IsCritical = isCritical
        };
        Debug.Log(totalDamage + "데미지");
        GameContainer.Instance.CombatSystem.AddCombatEvent(combatEvent);
    }
}
