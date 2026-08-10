using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    AttackBuff,
   AttackSpeedBuff
}
public class BuffSystem
{
    private PlayerManager _playerManager;
    private PlayerData _playerData;
    public float TotalAttackBuffMultiplier { get; private set; } = 1;
    public float TotalAttackSpeedMultiplier { get; private set; } = 1;
    private List<Coroutine> _buffCoroutines = new List<Coroutine>();
    public BuffSystem(PlayerManager playerManager, PlayerData playerData)
    {
        _playerManager = playerManager;
        _playerData = playerData;
    }
    public void AddActiveBuff(BuffType buffType, float value, float duration)
    {
        Coroutine coroutine = null;
        coroutine = GameManager.Instance.StartCoroutine(ActiveBuffCoroutine(buffType, value, duration, () =>
        {
            _buffCoroutines.Remove(coroutine);
        }));
        _buffCoroutines.Add(coroutine);
    }
    public void AddPassiveBuff(BuffType buffType, float value)
    {
        float multiplier = 1f + value / 100f;
        switch (buffType)
        {
            case BuffType.AttackBuff:
                TotalAttackBuffMultiplier *= multiplier;
                Debug.Log(multiplier + "공격력 증가");
                Debug.Log(_playerManager.GetTotalDamage() + "최종 공격력");
                break;
            case BuffType.AttackSpeedBuff:
                TotalAttackSpeedMultiplier *= multiplier;
                Debug.Log(multiplier + "공격속도 증가");
                Debug.Log(_playerManager.GetAttackInterval() + "최종 공격간격");
                break;
        }
    }
    // 스테이지 재시작시 초기화
    private IEnumerator ActiveBuffCoroutine(BuffType buffType, float value, float duration, Action onComplete)
    {
        float multiplier = 1f + value / 100f;
        switch (buffType)
        {
            case BuffType.AttackBuff:
                TotalAttackBuffMultiplier *= multiplier;
                Debug.Log(multiplier + "공격력 증가");
                Debug.Log(_playerManager.GetTotalDamage() + "최종 공격력");
                break;
            case BuffType.AttackSpeedBuff:
                TotalAttackSpeedMultiplier *= multiplier;
                Debug.Log(multiplier + "공격속도 증가");
                Debug.Log(_playerManager.GetAttackInterval() + "최종 공격간격");
                break;
        }
        yield return new WaitForSeconds(duration);
        switch (buffType)
        {
            case BuffType.AttackBuff:
                TotalAttackBuffMultiplier /= multiplier;
                break;
            case BuffType.AttackSpeedBuff:
                TotalAttackSpeedMultiplier /= multiplier;
                break;
        }
        onComplete?.Invoke();
    }
    public void ResetBuffs()
    {
        foreach (Coroutine coroutine in _buffCoroutines)
        {
            if (coroutine != null)
                GameManager.Instance.StopCoroutine(coroutine);
        }
        _buffCoroutines.Clear();
        TotalAttackBuffMultiplier = 1f;
        TotalAttackSpeedMultiplier = 1f;
    }

    public void ResetBuff(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.AttackBuff:
                TotalAttackBuffMultiplier = 1;
                break;
            case BuffType.AttackSpeedBuff:
                TotalAttackSpeedMultiplier = 1;
                break;
        }
    }
}
