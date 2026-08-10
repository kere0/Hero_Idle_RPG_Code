using System;
using UnityEngine;

public enum SKillCategory
{
    Passive,
    Active
}
public enum SkillType
{
    Attack,
    Buff
}

public enum SkillGrade
{
    Normal,
    Legend
}
[CreateAssetMenu(menuName = "Table/SkillTableSO")]
public class SkillTableSO : ScriptableObject
{
    public SkillInfo[] skills;
    [Serializable]
    public class SkillInfo
    {
        public string skillName;
        public SkillID id;
        public SKillCategory skillCategory;
        public SkillType skillType;
        public SkillGrade skillGrade;
        public string description;
        public int unlockLevel;
        public int mana;
        public int manaCostIncreaseValue;
        public float cooldown;
        public float duration;
        public int value;
        public int valueIncreaseRate;
        public Sprite sprite;
        public string animationName;
        public int animationHash;
        public float castTiming;
    }
}
