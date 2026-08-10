using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BaseSummonUI : MonoBehaviour
{
    [SerializeField] private Button _oneSummonButton;
    [SerializeField] private Button _tenSummonButton;

    [SerializeField] private EquipmentGachaManager _equipmentGachaManager;
    protected EquipmentType _equipmentType;
    [SerializeField] protected TextMeshProUGUI _summonLevelText;
    [SerializeField] protected Image _summonGaugeImage;
    [SerializeField] protected TextMeshProUGUI _summonGaugeText;
    protected PlayerManager _playerManager;

    protected bool _isInit = false;
    private void Awake()
    {
        _oneSummonButton.onClick.AddListener(OneButtonClick);
        _tenSummonButton.onClick.AddListener(TenButtonClick);
        _equipmentGachaManager.OnSummonFinished += EnableButtons;
        Managers.PlayerManager.OnDiamondChanged += EnableButtons;
    }
    
    private void OnEnable()
    {
        if (_isInit == false)
        {
            _isInit = true;
            Init();
        }
        Refresh();
        EnableButtons();
    }
    protected virtual void Init(){}
    private void OneButtonClick()
    {
        _equipmentGachaManager.RequestSummon(_equipmentType, false);
    }
    private void TenButtonClick()
    {
        _equipmentGachaManager.RequestSummon(_equipmentType, true);
    }
    private void EnableButtons()
    {
        bool enableOneSummon = Managers.PlayerManager.playerData.Diamond >= SummonSystem.OneSummonDiamondCost;
        bool enableTenSummon = Managers.PlayerManager.playerData.Diamond >= SummonSystem.TenSummonDiamondCost;
        _oneSummonButton.interactable = enableOneSummon;
        _tenSummonButton.interactable = enableTenSummon;
    }
    protected virtual void Refresh() { }
    private void OnDestroy()
    {
        _equipmentGachaManager.OnSummonFinished -= EnableButtons;
        Managers.PlayerManager.OnDiamondChanged -= EnableButtons;
    }
}
