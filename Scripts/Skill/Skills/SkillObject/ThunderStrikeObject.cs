using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeObject : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private Animator _animator;
    // 사정거리 넘어가면 사라짐
    private Vector3 _dir;
    private int _damage;

    private LayerMask _layerMask;
    private List<Collider2D> _hitTargets = new List<Collider2D>();
    private static readonly int THUNDER = Animator.StringToHash("Thunder");
    private bool _isCritical;
    private readonly Collider2D[] _hits = new Collider2D[10];
    private void Awake()
    {
        TryGetComponent(out _rigidbody2D);
        _layerMask = LayerMask.GetMask("Monster");
    }
    public void Init(Vector3 pos, int damage, bool isCritical)
    {
        transform.position = pos;
        _damage = damage;
        _isCritical = isCritical;
        _animator.Play("Thunder", 0, 0f);
        SoundManager.Instance.PlaySFX("ThunderStrike", 0.15f);
        GameContainer.Instance.CameraShakeManager.CameraShake(0.2f);
    }
    private void CheckInitialCollision()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, 0.5f, _hits, _layerMask);
        for (int i = 0; i < size; i++)
        {
            ApplyDamage(_hits[i]);
        }
    }
    private void ApplyDamage(Collider2D other)
    {
        foreach (Collider2D hitTarget in _hitTargets)
        {
            if (other == hitTarget) return;
        }
        IDamageable monster = other.GetComponent<IDamageable>();
        if (monster != null)
        {
            CombatEvent combatEvent = new CombatEvent()
            {
                Receiver = monster,
                Damage = _damage,
                IsCritical = _isCritical
            };
            GameContainer.Instance.CombatSystem.AddCombatEvent(combatEvent);
            _hitTargets.Add(other);
        }
    }
    private void Update()
    {
        CheckInitialCollision();
        AnimatorEndCheck();
    }
    protected void AnimatorEndCheck()
    {
        AnimatorStateInfo animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.shortNameHash == THUNDER)
        {
            if (animatorStateInfo.normalizedTime >= 1)
            {
                Managers.Pool.ObjPush(gameObject);
            }
        }
    }
    private void OnDisable()
    {
        _hitTargets.Clear();
    }
}
