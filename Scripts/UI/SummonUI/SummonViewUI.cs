using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonViewUI : MonoBehaviour
{
    [SerializeField] private Button _oneSummonViewButton;
    [SerializeField] private Button _tenSummonViewButton;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private EquipmentGachaManager _equipmentGachaManager;
    private EquipmentType _equipmentType;
    private void Awake()
    {
        _oneSummonViewButton.onClick.AddListener(OneButtonClick);
        _tenSummonViewButton.onClick.AddListener(TenButtonClick);
        _confirmButton.onClick.AddListener(ConfirmButtonClick);
        _equipmentGachaManager.OnSummonStart += DisableButtons;
        _equipmentGachaManager.OnSummonFinished += EnableButtons;
    }
    private void OneButtonClick()
    {
        _equipmentGachaManager.RequestSummon(_equipmentType, false);
    }
    private void TenButtonClick()
    {
        _equipmentGachaManager.RequestSummon(_equipmentType, true);
    }
    private void ConfirmButtonClick()
    {
        _equipmentGachaManager.CloseSummonViewPanel();
    }
    public void SetSummonViewPanleType(EquipmentType equipmentType)
    {
        _equipmentType = equipmentType;
    }

    private void EnableButtons()
    {
        _oneSummonViewButton.gameObject.SetActive(true);
        _tenSummonViewButton.gameObject.SetActive(true);
        _confirmButton.gameObject.SetActive(true);
        bool enableOneSummon = Managers.PlayerManager.playerData.Diamond >= SummonSystem.OneSummonDiamondCost;
        bool enableTenSummon = Managers.PlayerManager.playerData.Diamond >= SummonSystem.TenSummonDiamondCost;
        _oneSummonViewButton.interactable = enableOneSummon;
        _tenSummonViewButton.interactable = enableTenSummon;
    }
    private void DisableButtons()
    {
        _oneSummonViewButton.gameObject.SetActive(false);
        _tenSummonViewButton.gameObject.SetActive(false);
        _confirmButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _equipmentGachaManager.OnSummonStart -= DisableButtons;
        _equipmentGachaManager.OnSummonFinished -= EnableButtons;
    }
}
