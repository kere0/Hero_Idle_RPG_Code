using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillInstance : SkillInstance
{
    public const int MaxPassiveStack = 5;
    private int _passiveStack = 0;
    public PassiveSkillInstance(BaseSkill skill) : base(skill)
    {
    }
    public override bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        if (_passiveStack >= MaxPassiveStack - 1) return false;
        if (cooldown > 0) return false;
        bool result = skill.CanExecute(skillContext, isAuto);
        if (result == false) return false;
        return true;
    }
    public override void TryExecute(SkillContext skillContext, int attack, bool isCritical = false)
    {
        _passiveStack++;
        skill.Execute(skillContext, attack, Value, isCritical);
        if (_passiveStack == 1 && skill.skillInfo.skillType == SkillType.Buff)
        {
            GameContainer.Instance.BuffPanelUI.BuffSlotCreate(skill.skillInfo);
        }
    }
    public override void Reset()
    {
        cooldown = 0;
        _passiveStack = 0;
    }
}
