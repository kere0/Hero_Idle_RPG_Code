using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderChargeObject : MonoBehaviour
{
    private ParticleSystem _particle;
    private float _lifeTime = 0.55f;
    private float _timer = 0f;
    private int _damage;
    private Vector3 _defaultScale = new Vector3(3f, 1.5f, 1f);
    private BaseCreature _player;
    private LayerMask _layerMask;
    private List<Collider2D> _hitTargets = new List<Collider2D>();
    private bool _isCritical;
    private readonly Collider2D[] _hits = new Collider2D[10];
    private void Awake()
    {
        TryGetComponent(out _particle);
        _layerMask = LayerMask.GetMask("Monster");
    }
    private void OnEnable()
    {
        _timer = 0f;
        transform.localScale = _defaultScale;
        _particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _particle.Play(true);
    }
    public void Init(BaseCreature player, int damage, bool isCritical)
    {
        _player = player;
        transform.position = player.transform.position;
        _damage = damage;
        _isCritical = isCritical;
        SoundManager.Instance.PlaySFX("BoltCharge", pitch:1.5f);
    }
    private void Update()
    {
        transform.position = _player.EffectPos.position;
        CheckInitialCollision();
        UpdateLifeTime();
    }

    private void CheckInitialCollision()
    {
        int size = Physics2D.OverlapBoxNonAlloc(transform.position, new Vector2(12f, 6.5f), 0f, _hits, _layerMask);
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
            GameContainer.Instance.CameraShakeManager.CameraShake(0.3f, 0.3f);
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
    private void UpdateLifeTime()
    {
        _timer += Time.deltaTime;
        if (_lifeTime <= _timer)
        {
            Managers.Pool.ObjPush(gameObject);
        }
    }
    private void OnDisable()
    {
        _hitTargets.Clear();
        _player = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(12f, 6.5f, 0));
    }
}
