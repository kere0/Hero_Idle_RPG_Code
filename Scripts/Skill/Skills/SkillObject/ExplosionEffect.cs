using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private ParticleSystem _particle;
    private float _lifeTime = 2f;
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
    public void Init(Vector3 pos)
    {
        transform.position = pos;
        GameContainer.Instance.CameraShakeManager.CameraShake(0.15f);
        Debug.Log("2");

        SoundManager.Instance.PlaySFX("ExplosionStrike", 0.5f);

        Debug.Log("3");

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
