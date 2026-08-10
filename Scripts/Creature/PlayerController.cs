using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : BaseCreature
{
    public PlayerManager PlayerManager;
    private BattleManager _battleManager;

    public ManaComponent ManaComponent;
    public float defaultPlayerSpeed = 5;
    public static readonly int IDLE = Animator.StringToHash("Idle");
    public static readonly int MOVE = Animator.StringToHash("Move");
    public static readonly int ATTACK = Animator.StringToHash("CrossAttack");
    public static readonly int THRUSTATTACK = Animator.StringToHash("ThrustAttack");
    public static readonly int VOIDSTRIKE = Animator.StringToHash("VoidStrike");
    public static readonly int ThunderCharge = Animator.StringToHash("ThunderCharge");
    public static readonly int HITGROUND = Animator.StringToHash("HitGround");
    public static readonly int KNOCKBACK = Animator.StringToHash("KnockBack");
    public static readonly int DRINK = Animator.StringToHash("Drink");
    public static readonly int ACTIVEBUFF = Animator.StringToHash("ActiveBuff");
    private PlayerStateMachine _stateMachine;
    public SkillController PlayerSkillController;
    public IDamageable CurrentTarget;
    public bool isCastingSkill = false;
    
    public float timer = 9999f;
    [SerializeField] private Bar _mpBar;
    [SerializeField] private ParticleSystem _levelUpParticle;
    public float CurrentMana => ManaComponent.CurrentMana;
    public float MaxMana => ManaComponent.MaxMana;
    private bool _isAutoMode = false;
    protected override void Awake()
    {
        _stateMachine = new PlayerStateMachine(this);
        PlayerSkillController = new SkillController(this);
        ManaComponent = new ManaComponent();
        base.Awake();
    }
    private void Start()
    {
        _creatureType = CreatureType.Player;
        PlayerSkillController.Init();
        _stateMachine.Start();
        PlayerManager = Managers.PlayerManager;
        _maxHp = PlayerManager.GetTotalMaxHp();
        _currentHp = _maxHp;
        ManaComponent.Init(PlayerManager, _mpBar);
        speed = defaultPlayerSpeed;
        float fillAmount = _currentHp / _maxHp;
        hpBar.SetFillAmount(fillAmount);
        fillAmount = CurrentMana / MaxMana;
        _mpBar.SetFillAmount(fillAmount);
        PlayerManager.OnLevelUp += LevelUpEffect;
        _battleManager = GameContainer.Instance.BattleManager;
    }
    private void Update()
    {
        if (_isDead == true) return;
        if (PlayerManager.GetAttackInterval() > timer)
        {
            timer += Time.deltaTime;
        }
        UpdateHpRegenPerSecond();
        ManaComponent.UpdateManaRegenPerSecond();
        PlayerSkillController.Update();
        _stateMachine.Update();
        if (_isAutoMode == true)
        {
            if (_battleManager == null) return;
            if (_battleManager.isBattleStart == true)
            {
                if (_stateMachine.GetCurrentState() == _stateMachine.KnockBackState) return;
                Auto();
            }
               
        }
        else
        {
            PassiveAuto();
        }
    }
    public void ToggleAutoMode()
    {
        _isAutoMode = !_isAutoMode;
    }
    private void Auto()
    {
        for (int i = 0; i < SkillSystem.EquipSkillSlotCount; i++)
        {
            SkillInstance skill = PlayerSkillController.GetSkill(i);
            if(skill == null) continue;
            UseSkill(i , true);
        }
    }

    private void PassiveAuto()
    {
        for (int i = 0; i < SkillSystem.EquipSkillSlotCount; i++)
        {
            SkillInstance skill = PlayerSkillController.GetSkill(i);
            if(skill == null) continue;
            if (skill.skill.skillInfo.skillCategory == SKillCategory.Passive)
            {
                UseSkill(i);
            }
        }
    }
    private void UpdateHpRegenPerSecond()
    {
        if (_currentHp < _maxHp)
        {
            _currentHp += PlayerManager.GetTotalHpRegenPerSecond() * Time.deltaTime;
            _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);
            hpBar.SetFillAmount(_currentHp / _maxHp);
        }
    }
    public void UseSkill(int index, bool isAuto = false)
    {
        if (isCastingSkill == true) return;
        if (PlayerSkillController.CanUseSkill(index, isAuto) == true)
        {
            SkillInstance skill = PlayerSkillController.GetSkill(index);
            ManaComponent.UseMana(skill.ManaCost);
            // 애니메이션이 있으면
            if (skill.skill.skillInfo.skillGrade == SkillGrade.Legend)
            {
                FadeManager.Instance.SpriteFadeInOut(0.2f, 0.15f, 0.2f, FadeType.Skill);
            }
            if (skill.skill.skillInfo.animationHash != -1)
            {
                _stateMachine.SkillState.SetUseSkillSlotIndex(index);
                _stateMachine.ChangeState(_stateMachine.SkillState);
            }
            // 없으면
            else
            {
                PlayerSkillController.UseSkill(index);
            }
        }
    }
    public void ForceKill()
    {
        if (_isDead) return;
        _stateMachine.ChangeState(_stateMachine.DeathState);
    }
    private void LevelUpEffect()
    {
        if (_levelUpParticle.gameObject.activeSelf == false)
        {
            _levelUpParticle.gameObject.SetActive(true);
        }
        _levelUpParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _levelUpParticle.Play(true);

    }
    public void Reset()
    {
        if (_isDead == true)
        {
            _isDead = false;
        }
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
            _resetCoroutine = null;
        }
        CurrentTarget = null;
        _stateMachine.ChangeState(_stateMachine.MoveState);
        _currentHp = PlayerManager.GetTotalMaxHp();
        hpBar.SetFillAmount(_currentHp / _maxHp);
        ManaComponent.Reset();
    }
    public void KnockBack(float knockBackPower, float duration)
    {
        _stateMachine.KnockBackState.ApplyKnockBack(-transform.right, knockBackPower,duration);
        _stateMachine.ChangeState(_stateMachine.KnockBackState);
    }
    public override void TakeDamage(int damage)
    {
        if (_isDead == true) return;
        OnHit();
        _currentHp -= damage;
        float fillAmount = _currentHp / _maxHp;
        hpBar.SetFillAmount(fillAmount);
        // Effect
        if (_currentHp > 0)
        {
            animator.Play(HIT, 0, 0);
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.DeathState);
        }
    }
    public override void Dead()
    {
        _isDead = true;
        _currentHp = 0;
        float fillAmount = _currentHp / _maxHp;
        hpBar.SetFillAmount(fillAmount);
        GameContainer.Instance.BattleManager.OnPlayerDead?.Invoke(2f);
    }
    private void OnDestroy()
    {
        PlayerManager.OnLevelUp -= LevelUpEffect;
    }
}
