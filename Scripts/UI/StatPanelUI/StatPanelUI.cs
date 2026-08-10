using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _manaText;
    [SerializeField] private TextMeshProUGUI _attackSpeedText;
    [SerializeField] private TextMeshProUGUI _critAttackText;
    [SerializeField] private TextMeshProUGUI _critChanceText;
    [SerializeField] private TextMeshProUGUI _hpRegenText;
    [SerializeField] private TextMeshProUGUI _manaRegenText;
    
    private int _lastAttack;
    private int _lastMaxHp;
    private int _lastCurrentHp;
    private int _lastMaxMana;
    private int _lastCurrentMana;
    private float _lastAttackSpeed;
    private int _lastCritAttack;
    private int _lastCritChance;
    private int _lastHpRegen;
    private float _lastManaRegen;

    private float _refreshTimer;

    private void OnEnable()
    {
        RefreshStats();
    }

    private void Update()
    {
        _refreshTimer += Time.deltaTime;

        if (_refreshTimer < 0.2f)
            return;

        _refreshTimer = 0;

        RefreshStats();
    }
    private void RefreshStats()
    {
        int attack = Managers.PlayerManager.GetTotalDamage();
        if (_lastAttack != attack)
        {
            _lastAttack = attack;
            _attackText.SetText("{0}", attack);
        }
        int maxHp = Managers.PlayerManager.GetTotalMaxHp();
        int currentHp = Mathf.RoundToInt(GameContainer.Instance.Player.CurrentHp);
        if (_lastCurrentHp != currentHp || _lastMaxHp != maxHp)
        {
            _lastMaxHp = maxHp;
            _lastCurrentHp = currentHp;
            _hpText.SetText("{0} / {1}", currentHp, maxHp);
        }
        int maxMana = Managers.PlayerManager.playerData.DefaltMana;
        int currentMana = Mathf.RoundToInt(GameContainer.Instance.Player.CurrentMana);
        if (_lastCurrentMana != currentMana || _lastMaxMana != maxMana)
        {
            _lastMaxMana = maxMana;
            _lastCurrentMana = currentMana;
            _manaText.SetText("{0} / {1}", currentMana, maxMana);
        }
        float attackSpeed = 1 / Managers.PlayerManager.GetAttackInterval();
        if (!Mathf.Approximately(_lastAttackSpeed, attackSpeed))
        {
            _lastAttackSpeed = attackSpeed;
            _attackSpeedText.SetText("{0:0.00}", attackSpeed);
        }
        int critAttack = Managers.PlayerManager.playerData.EnhanceData.CritAttackLevel;
        if (_lastCritAttack != critAttack)
        {
            _lastCritAttack = critAttack;
            _critAttackText.SetText("{0}%", critAttack);
        }
        int critChance = Managers.PlayerManager.playerData.EnhanceData.CritChanceLevel;
        if (_lastCritChance != critChance)
        {
            _lastCritChance = critChance;
            _critChanceText.SetText("{0}%", critChance);
        }
        int hpRegen = Managers.PlayerManager.GetTotalHpRegenPerSecond();
        if (!Mathf.Approximately(_lastHpRegen, hpRegen))
        {
            _lastHpRegen = hpRegen;
            _hpRegenText.SetText("{0}", hpRegen);
        }
        float manaRegen = Managers.PlayerManager.GetTotalManaRegenPerSecond();
        if (!Mathf.Approximately(_lastManaRegen, manaRegen))
        {
            _lastManaRegen = manaRegen;
            _manaRegenText.SetText("{0:0.0}", manaRegen);
        }
    }
}
