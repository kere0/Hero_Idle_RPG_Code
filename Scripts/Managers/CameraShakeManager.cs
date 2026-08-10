using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    private float currentForce; // 현재 흔들림 강도 저장
    private float resetTimer;   // 강도 초기화 타이머
    private void Awake()
    {
        TryGetComponent(out impulseSource);
    }
    private void Update()
    {
        if (resetTimer > 0)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
            {
                currentForce = 0;
            }
        }
    }

    public void CameraShake(float force = 1f, float sustainTime = 0.15f)
    {
        if (force <= currentForce) return;
        
        currentForce = force;
        resetTimer = sustainTime;
        
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = sustainTime;
        impulseSource.GenerateImpulseWithForce(force);
    }
}
