using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackEffect : MonoBehaviour
{
    private ParticleSystem _particle;
    private readonly float _lifeTime = 2f;
    private float _timer = 0f;
    private void Awake()
    {
        TryGetComponent(out _particle);
    }
    private void OnEnable()
    {
        _timer = 0f;
        _particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _particle.Play(true);
    }
    private void Update()
    {
        UpdateLifeTime();
    }
    private void UpdateLifeTime()
    {
        _timer += Time.deltaTime;
        if (_lifeTime <= _timer)
        {
            Managers.Pool.ObjPush(gameObject);
        }
    }
}
