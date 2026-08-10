using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidStrikeObject : MonoBehaviour
{
    private ParticleSystem _particle;
    private float _lifeTime = 1.5f;
    private float _timer = 0f;
    private int _damage;
    private LayerMask _layerMask;
    private List<Collider2D> _hitTargets = new List<Collider2D>();
    private bool _isCritical;
    private readonly Collider2D[] _hits = new Collider2D[10];
    private Vector3 offset = new Vector3(5.75f, 1.75f, 0);
    private void Awake()
    {
        TryGetComponent(out _particle);
        _layerMask = LayerMask.GetMask("Monster");
    }
    private void OnEnable()
    {
        _timer = 0f;
        _particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _particle.Play(true);

    }
    public void Init(Vector3 pos, int damage, bool isCritical)
    {
        transform.position = pos + offset;
        _damage = damage;
        _isCritical = isCritical;
        GameContainer.Instance.CameraShakeManager.CameraShake(0.5f, 0.5f);
    }
    private void Update()
    {
        CheckInitialCollision();
        UpdateLifeTime();
    }
    private void CheckInitialCollision()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, 4.7f, _hits, _layerMask);
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
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4.7f);
    }
}
