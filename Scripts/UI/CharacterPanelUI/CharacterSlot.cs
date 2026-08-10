using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [SerializeField] protected GameObject _slotImage;
    [SerializeField] protected HoldButton _upgradeButton;
    [SerializeField] protected TextMeshProUGUI _costText;
    [SerializeField] protected TextMeshProUGUI _upgradeInfoText;
    [SerializeField] protected TextMeshProUGUI _upgradeLevelText;
    protected PlayerManager PlayerManager;
    private float _upgradeTimer;
    private const float UpgradeInterval = 0.09f;
    protected virtual void Awake()
    {
        _upgradeButton.onClick.AddListener(UpgradeButtonClick);
        PlayerManager = Managers.PlayerManager;
    }
    private void Update()
    {
        if (!_upgradeButton.isHolding)
        {
            _upgradeTimer = 0f;
            return;
        }
        if (!_upgradeButton.IsLongPressing)
            return;
        _upgradeTimer -= Time.deltaTime;

        if (_upgradeTimer <= 0f)
        {
            UpgradeButtonClick();
            _upgradeTimer = UpgradeInterval;
        }
    }
    protected virtual void UpgradeButtonClick() { }
}
