using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private ParticleSystem _particle;
    private float _lifeTime = 2f;
    private float _timer = 0f;
    private Vector3 _defaultScale = new Vector3(3.5f, 3.5f, 1f);
    private void Awake()
    {
        TryGetComponent(out _particle);
    }
    private void OnEnable()
    {
        transform.localScale = _defaultScale;
        _timer = 0f;
        _particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _particle.Play(true);
    }
    protected virtual void Update()
    {
        UpdateLifeTime();
    }
    protected void UpdateLifeTime()
    {
        _timer += Time.deltaTime;
        if (_lifeTime <= _timer)
        {
            Managers.Pool.ObjPush(gameObject);
        }
    }
}
