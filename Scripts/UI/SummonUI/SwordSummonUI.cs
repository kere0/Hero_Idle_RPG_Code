using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSummonUI : BaseSummonUI
{
    protected override void Init()
    {
        _playerManager = Managers.PlayerManager;
        _equipmentType = EquipmentType.Sword;
        _playerManager.OnSwordSummon += Refresh;
    }
    protected override void Refresh()
    {
        _summonLevelText.text = $"소환 레벨 {_playerManager.playerData.SwordSummonLevel}";
        _summonGaugeText.text = $"{_playerManager.playerData.SwordSummonCount} / {_playerManager.SummonSystem.GetMaxSummonCount(_equipmentType)}";
        _summonGaugeImage.fillAmount = (float)_playerManager.playerData.SwordSummonCount / _playerManager.SummonSystem.GetMaxSummonCount(_equipmentType);
    }
    private void OnDestroy()
    {
        _playerManager.OnRingSummon -= Refresh;
    }
}
