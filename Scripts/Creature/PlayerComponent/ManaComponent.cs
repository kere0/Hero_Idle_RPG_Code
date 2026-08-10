using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaComponent
{
    private PlayerManager _playerManager;
    public float CurrentMana { get; private set; }
    public float MaxMana { get; private set; }
    private Bar _mpBar;
    public void Init(PlayerManager playerManager, Bar mpBar)
    {
        _playerManager = playerManager;
        MaxMana = _playerManager.playerData.DefaltMana;
        CurrentMana = MaxMana;
        _mpBar = mpBar;
    }
    public void UseMana(int value)
    {
        Debug.Log("현재마나: " + CurrentMana);
        Debug.Log("UseMana: " + value);
        CurrentMana -= value;
        Debug.Log("최종마나: " + CurrentMana);
        CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);
        float fillAmount = CurrentMana / MaxMana;
        _mpBar.SetFillAmount(fillAmount);
    }

    public void ManaPercentRecovery(int value)
    {
        float recoveryValue = MaxMana * (value / 100f);
        CurrentMana += recoveryValue;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);
        _mpBar.SetFillAmount(CurrentMana / MaxMana);
    }
    public void UpdateManaRegenPerSecond()
    {
        if (CurrentMana < MaxMana)
        {
            CurrentMana += _playerManager.GetTotalManaRegenPerSecond() * Time.deltaTime;
            CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);
            _mpBar.SetFillAmount(CurrentMana / MaxMana);
        }
    }
    public void Reset()
    {
        CurrentMana = MaxMana;
        _mpBar.SetFillAmount(CurrentMana / MaxMana);

    }
}
