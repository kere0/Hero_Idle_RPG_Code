using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseSkill
{
    public SkillTableSO.SkillInfo skillInfo;
    public BaseSkill(SkillTableSO.SkillInfo skillInfo)
    {
        this.skillInfo = skillInfo;
    }
    public int GetValue(int level)
    {
        int value = 0;
        if (level != 0)
        {
            value = skillInfo.value + (level - 1) * skillInfo.valueIncreaseRate;
        }
        else
        {
            value = skillInfo.value;
        }
        return value;
    }
    public virtual int GetManaCost(int level)
    {
        int manaCost = skillInfo.mana + (level - 1) * skillInfo.manaCostIncreaseValue ;
        return manaCost;
    }
    public abstract bool CanExecute(SkillContext skillContext, bool isAuto);
    public abstract void Execute(SkillContext skillContext, int attack, int value, bool isCritical = false);

    public virtual void Reset() { }
}
  