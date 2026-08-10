using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Menu : MonoBehaviour
{
    public static Canvas_Menu Instance;
    [SerializeField] private ParticleSystem _upgradeParticleSystem;
    private void Awake()
    {
        Instance = this;
    }
    public void PlayUpgradeEffect(Vector3 pos)
    {
        if (_upgradeParticleSystem.gameObject.activeInHierarchy == false)
        {
            _upgradeParticleSystem.gameObject.SetActive(true);
        }
        _upgradeParticleSystem.transform.position = pos;
        _upgradeParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _upgradeParticleSystem.Play(true);
    }
}
