using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SkillContext
{
    public BaseCreature Caster;
    public IDamageable Target;
    public int Attack;
}
public class SkillController
{
    private SkillInstance[] _skills;
    private PlayerController _player;
    private bool _isInit = false;
    public SkillController(PlayerController player)
    {
        _player = player;
    }
    public void Init()
    {
        _skills = Managers.PlayerManager.playerData.EquippedSkillInstances;
        _isInit = true;
        GameManager.Instance.OnGameStart -= Init;
    }
    public SkillInstance GetSkill(int index)
    {
        return _skills[index];
    }
    // 여기서 먼저 사용할수있는지 없는지 알아야함
    public bool CanUseSkill(int index, bool isAuto)
    {
        SkillContext skillContext = new SkillContext()
        {
            Caster = _player,
            Target = _player.CurrentTarget,
        };
        return _skills[index].CanExecute(skillContext, isAuto);
    }
    public void UseSkill(int index, int attack = 0, bool isCritical = false)
    {
        SkillInstance skillInstance = GetSkill(index);
        Debug.Log(skillInstance.skill.skillInfo.skillName + " is used");
        SkillContext skillContext = new SkillContext()
        {
            Caster = _player,
            Target = _player.CurrentTarget,
        };
        skillInstance.TryExecute(skillContext, attack, isCritical);
        skillInstance.ResetCooldown();
    }
    public void Update()
    {
        if (_isInit == true)
        {
            foreach (SkillInstance skill in _skills)
            {
                if (skill == null) continue;
                skill.UpdateCooldown();
            }
        }
    }
}
