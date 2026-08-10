using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public enum CreatureType
{
    Player,
    Monster
}
public class BaseCreature : MonoBehaviour, IDamageable
{
    public static readonly int IDLE = Animator.StringToHash("Idle");
    public static readonly int ATTACK = Animator.StringToHash("Attack");
    public static readonly int KBATTACK = Animator.StringToHash("KBAttack");
    public static readonly int HIT = Animator.StringToHash("Hit");
    public static readonly int DEAD = Animator.StringToHash("Dead");
    public Animator animator;
    public Collider2D _collider;
    protected float _attackRange;
    public float speed; 
    private Action _onHpChange;
    private Action<float> _onTakeDamage;
    [SerializeField] private Transform effectPos;
    protected float _maxHp;
    protected float _currentHp;
    protected bool _isDead;
    public Bar hpBar;
    
    [SerializeField] protected MeshRenderer _meshRenderer;
    protected MaterialPropertyBlock _mpb;
    
    protected Coroutine _resetCoroutine;
    protected static readonly int ColorID = Shader.PropertyToID("_Color");
    protected static readonly WaitForSeconds ResetColorDelay = new WaitForSeconds(0.07f);
    protected static readonly Color DeadColor = new Color(0.35f, 0.05f, 0.05f, 1);

    // 넉백
    public bool isKnockback = false;
    public float knockTime = 1f;
    public float knockPower = 5f;
    public Vector3 knockDir;
    public float elapsed = 0f;
    //
    protected CreatureType _creatureType;
    #region IDamageAble
    public CreatureType CreatureType => _creatureType;
    public Collider2D Collider => _collider;
    public Vector3 Position => transform.position;

    public Transform EffectPos => effectPos;
    public float AttackRange => _attackRange;
    public bool IsDead => _isDead;
    public float MaxHp => _maxHp;
    public float CurrentHp
    {
        get => _currentHp;
        set => _currentHp = value;
    }
    #endregion
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        hpBar = GetComponentInChildren<Bar>();
        TryGetComponent(out _collider);
        _mpb = new MaterialPropertyBlock();
    }
    public virtual void TakeDamage(int damage) { }
    protected virtual void OnHit()
    {
        if (!gameObject.activeInHierarchy)
            return;
        _meshRenderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(ColorID, Color.red);
        _meshRenderer.SetPropertyBlock(_mpb);
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
        }
        _resetCoroutine = StartCoroutine(ResetColorCoroutine());
    }
    protected virtual IEnumerator ResetColorCoroutine()
    {
        yield return ResetColorDelay;
        _meshRenderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(ColorID, Color.white);
        _meshRenderer.SetPropertyBlock(_mpb);
    }
    protected virtual void DeadEffect()
    {
        if (!gameObject.activeInHierarchy)
            return;
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
        }
        _resetCoroutine = StartCoroutine(ResetColorCoroutine());
        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOVirtual.Float(0f, 1f, 0.7f, t =>
        {
            animator.speed = 0;
            _meshRenderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(ColorID, Color.Lerp(Color.red, DeadColor, t));
            _meshRenderer.SetPropertyBlock(_mpb);
        }));
        sequence.AppendCallback(() => hpBar.gameObject.SetActive(false));
        sequence.Append(DOVirtual.Float(1f, 0f, 0.5f, a =>
        {
            _meshRenderer.GetPropertyBlock(_mpb);
            Color color = DeadColor;
            color.a = a;
            _mpb.SetColor(ColorID, color);
            _meshRenderer.SetPropertyBlock(_mpb);
        }));
        sequence.OnComplete(() =>
        {
            Managers.Pool.ObjPush(gameObject);
        });
    }
    public virtual void Dead() { }
}
