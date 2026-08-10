using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect : MonoBehaviour
{
    private ParticleSystem particle;
    private float lifeTime = 2f;
    private float timer = 0f;
    protected PlayerController _player;
    private void Awake()
    {
        TryGetComponent(out particle);
    }
    private void OnEnable()
    {
        timer = 0f;
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particle.Play(true);
    }
    public void SetOwner(PlayerController player)
    {
        transform.position = player.EffectPos.position;
        _player = player;
    }
    protected virtual void Update()
    {
        UpdateLifeTime();
        if (_player == null) return;
        transform.position = _player.EffectPos.position;
    }
    protected void UpdateLifeTime()
    {
        timer += Time.deltaTime;
        if (lifeTime <= timer)
        {
            Managers.Pool.ObjPush(gameObject);
        }
    }
    private void OnDisable()
    {
        _player = null;
    }
}
