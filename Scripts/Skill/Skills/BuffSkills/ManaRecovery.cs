using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRecovery : BaseSkill
{
    public ManaRecovery(SkillTableSO.SkillInfo skillInfo) : base(skillInfo)
    {
    }
    public override int GetManaCost(int level)
    {
        return 0;
    }
    public override bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        if (Mathf.Approximately(GameContainer.Instance.Player.ManaComponent.CurrentMana, GameContainer.Instance.Player.ManaComponent.MaxMana))
        {
            return false;
        }
        return true;
    }
    public override void Execute(SkillContext skillContext, int attack, int skillValue, bool isCritical)
    {
        BuffEffect go = Managers.Resource.Instantiate("ManaRecovery", pooling: true).GetComponent<BuffEffect>();
        go.SetOwner(GameContainer.Instance.Player);
        GameContainer.Instance.Player.ManaComponent.ManaPercentRecovery(skillValue);
    }
}
