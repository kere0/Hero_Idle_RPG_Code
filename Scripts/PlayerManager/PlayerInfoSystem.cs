using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoSystem
{
    private PlayerManager _playerManager;
    private PlayerData _playerData;
    public int MaxExp;
    public PlayerInfoSystem(PlayerManager playerManager, PlayerData playerData)
    {
        _playerManager = playerManager;
        _playerData = playerData;
    }
    public void InitData()
    {
        _playerData.CurrentStage = 1;
        _playerData.Level = 1;
        _playerData.DefaultHp = 100;
        _playerData.DefaltMana = 100;
        _playerData.HpRegenPerSecond = 1;
        _playerData.ManaRegenPerSecond = 1;
        _playerData.DefaultAttackSpeed = 1;
        _playerData.Exp = 0;
        _playerData.Gold = 10000;
        _playerData.Diamond = 10000;
        _playerData.EnhanceStone = 1000;
        MaxExp =  ValueCalculator.GetPlayerMaxExp(_playerData.Level);
    }
    public void StageUp()
    {
        _playerData.CurrentStage++;
    }
    // 성장
    public void GainExp(int amount)
    {
        _playerData.Exp += amount;
        while (_playerData.Exp >= MaxExp)
        {
            _playerData.Exp -= MaxExp;
            _playerData.Level++;
            _playerData.StatPoint++;
            _playerData.SkillPoint++;
            _playerManager.OnLevelUp?.Invoke();
        }
        _playerManager.OnExpChanged?.Invoke();
    }
    public void LevelUp()
    {
        _playerData.Level++;
        _playerData.StatPoint++;
        _playerData.SkillPoint++;
        _playerManager.OnLevelUp.Invoke();
        _playerManager.playerData.Exp = 0;
        MaxExp =  ValueCalculator.GetPlayerMaxExp(_playerData.Level);
        _playerManager.OnExpChanged?.Invoke();
        _playerManager.OnAchievementValueChanged?.Invoke(AchievementType.CharacterLevelUp);
    }
    public void GainGold(int amount)
    {
        _playerData.Gold += amount;
        _playerManager.OnGoldChanged?.Invoke();
    }
    public void GainEnhanceStone(int amount)
    {
        _playerData.EnhanceStone += amount;
        _playerManager.OnEnhanceStoneChanged?.Invoke();
    }
    public void GainDiamond(int amount)
    {
        _playerData.Diamond += amount;
        _playerManager.OnDiamondChanged();
    }
    public void UseDiamond(int amount)
    {
        _playerData.Diamond -= amount;
        _playerManager.OnDiamondChanged();
    }
}
