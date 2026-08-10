using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFactory
{
    public static SkillInstance Create(SkillTableSO.SkillInfo skillInfo)
    {
        BaseSkill skill = null;
        switch (skillInfo.id)
        {
            case SkillID.ThunderBall:
                skill = new ThunderBall(skillInfo);
                break;
            case SkillID.ExplosionStrike:
                skill = new ExplosionStrike(skillInfo);
                break;
            case SkillID.ThunderStrike:
                skill = new ThunderStrike(skillInfo);
                break;
            case SkillID.VoidStrike:
                skill = new VoidStrke(skillInfo);
                break;
            case SkillID.Acceleration:
                skill = new Acceleration(skillInfo);
                break;
            case SkillID.FightingSpirit:
                skill = new FightingSpirit(skillInfo);
                break;
            case SkillID.Frenzy:
                skill = new Frenzy(skillInfo);
                break;
            case SkillID.Berserk:
                skill = new Berserk(skillInfo);
                break;
            case SkillID.ManaRecovery:
                skill = new ManaRecovery(skillInfo);
                break;
            case SkillID.ThunderCharge:
                skill = new ThunderCharge(skillInfo);
                break;
        }
        if (skillInfo.skillCategory == SKillCategory.Passive)
        {
            return new PassiveSkillInstance(skill);
        }
        return new SkillInstance(skill);
    }
}
