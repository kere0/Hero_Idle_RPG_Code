using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSystem
{
    private PlayerData _playerData;
    private float _playTime;
    public PlayerTimeSystem(PlayerData playerData)
    {
        _playerData = playerData;
    }
    public void InitData()
    {
        _playTime = 0f;
    }
    public void Update()
    {
        _playTime += Time.deltaTime;
        if (_playTime < 1f) return;
        _playTime -= 1f;
        Managers.PlayerManager.MissionSystem.IncreasePlayTime(1f);
    }
}
