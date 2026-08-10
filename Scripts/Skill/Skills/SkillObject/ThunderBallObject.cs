using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBallObject : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    // 사정거리 넘어가면 사라짐
    private Vector3 _dir;
    private int _damage;
    private float _dist = 20f;
    private float _speed = 15f;
    [Header("Distance Settings")]
    private float _maxDistanceSqr;  // 사정거리의 제곱값
    private Vector3 _startPosition; // 발사 시점의 위치
    
    private ParticleSystem _particle;

    private LayerMask _layerMask;
    private List<Collider2D> _hitTargets = new List<Collider2D>();
    private readonly Collider2D[] _hits = new Collider2D[10];

    private bool _isCritical;
    private void Awake()
    {
        TryGetComponent(out _rigidbody2D);
        TryGetComponent(out _particle);
        _maxDistanceSqr = _dist * _dist;
        _layerMask = LayerMask.NameToLayer("Monster");
    }
    private void OnEnable()
    {
        _startPosition = transform.position;
        _particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _particle.Play(true);
    }
    public void Init(Vector3 pos, Vector3 direction, int damage, bool isCritical)
    {
        transform.position = pos;
        _dir = direction;
        _damage = damage;
        _isCritical = isCritical;
        _startPosition = transform.position; // 여기서 초기 위치 저장
        CheckInitialCollision();
    }
    private void CheckInitialCollision()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, 0.5f, _hits, _layerMask);
        for (int i = 0; i < size; i++)
        {
            ApplyDamage(_hits[i]);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != _layerMask) return;
        ApplyDamage(other);
    }
    private void ApplyDamage(Collider2D other)
    {
        if(other.gameObject.layer != _layerMask) return;
        foreach (Collider2D hitTarget in _hitTargets)
        {
            if (other == hitTarget) return;
        }
        GameContainer.Instance.CameraShakeManager.CameraShake(0.1f);
        IDamageable monster = other.GetComponent<IDamageable>();
        if (monster != null)
        {
            SoundManager.Instance.PlaySFX("HitThunderBall");
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
        _rigidbody2D.velocity = _dir * _speed;
        CheckDistance();
    }
    private void CheckDistance()
    {
        if ((transform.position - _startPosition).sqrMagnitude >= _maxDistanceSqr)
        {
            Managers.Pool.ObjPush(gameObject);
        }
    }
    private void OnDisable()
    {
        _hitTargets.Clear();
    }
}
