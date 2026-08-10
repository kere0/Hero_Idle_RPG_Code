using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSummonUI : BaseSummonUI
{
    protected override void Init()
    {
        _playerManager = Managers.PlayerManager;
        _equipmentType = EquipmentType.Ring;
        _playerManager.OnRingSummon += Refresh;
    }
    protected override void Refresh()
    {
        _summonLevelText.text = $"소환 레벨 {_playerManager.playerData.RingSummonLevel}";
        _summonGaugeText.text = $"{_playerManager.playerData.RingSummonCount} / {_playerManager.SummonSystem.GetMaxSummonCount(_equipmentType)}";
        _summonGaugeImage.fillAmount = (float)_playerManager.playerData.RingSummonCount / _playerManager.SummonSystem.GetMaxSummonCount(_equipmentType);
    }
    private void OnDestroy()
    {
        _playerManager.OnRingSummon -= Refresh;
    }
}
