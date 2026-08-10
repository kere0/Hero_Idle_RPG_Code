using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SkillInstance
{
    public BaseSkill skill;
    public int Level { get; private set;}
    public bool isUnlocked;
    public float cooldown;
    
    public int Value { get; private set; }
    public int ManaCost { get; private set; }
    public SkillInstance(BaseSkill skill)
    {
        Level = 0;
        isUnlocked = false;
        this.skill = skill;
        cooldown = 0;
        Value = skill.GetValue(Level);
        ManaCost = skill.GetManaCost(Level);
    }
    public void SkillLevelUp()
    {
        Level++;
        Value = skill.GetValue(Level);
        ManaCost = skill.GetManaCost(Level);
        if (skill.skillInfo.unlockLevel >= Level)
        {
            isUnlocked = true;
        }
    }
    public virtual bool CanExecute(SkillContext skillContext, bool isAuto)
    {
        if (cooldown > 0) return false;
        if (GameContainer.Instance.Player.CurrentMana < ManaCost) return false;
        bool result = skill.CanExecute(skillContext, isAuto);
        if (result == false) return false;
        return true;
    }
    public virtual void TryExecute(SkillContext skillContext, int attack, bool isCritical = false)
    {
        skill.Execute(skillContext, attack, Value, isCritical);
        if (skill.skillInfo.skillType == SkillType.Buff)
        {
            GameContainer.Instance.BuffPanelUI.BuffSlotCreate(skill.skillInfo);
        }
    }
    public void ResetCooldown()
    {
        cooldown = skill.skillInfo.cooldown;
    }
    public void UpdateCooldown()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }
    public virtual void Reset()
    {
        cooldown = 0;
    }
}
