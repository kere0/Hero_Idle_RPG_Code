using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonSlot : MonoBehaviour
{
    private DungeonPanelUI _dungeonPanelUI;
    private int _slotNum;
    [SerializeField] private Button button;
    private int _currentDungeonLevel;
    [SerializeField] private TextMeshProUGUI _dungeonLevelText;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private TextMeshProUGUI _rewardText;
    private void Awake()
    {
        button.onClick.AddListener(SlotClick);
        _leftButton.onClick.AddListener(LeftButtonClick);
        _rightButton.onClick.AddListener(RightButtonClick);
    }
    private void OnEnable()
    {
        _currentDungeonLevel = Managers.PlayerManager.playerData.MaxDungeonLevel[_slotNum];
        _dungeonLevelText.text = _currentDungeonLevel.ToString();
        LeftRightCheck();
        _rewardText.text = ValueCalculator.GetDungeonReward((DungeonType)_slotNum, _currentDungeonLevel).ToString();
    }
    public void Init(DungeonPanelUI dungeonPanelUI, int slotNum, int currentDungeonLevel)
    {
        _dungeonPanelUI = dungeonPanelUI;
        _slotNum = slotNum;
        _currentDungeonLevel = currentDungeonLevel;
        _rewardText.text = ValueCalculator.GetDungeonReward((DungeonType)_slotNum, _currentDungeonLevel).ToString();
    }
    private void SlotClick()
    {
        _dungeonPanelUI.DungeonButtonClick(_slotNum, _currentDungeonLevel);
    }

    private void LeftButtonClick()
    {
        bool result = _currentDungeonLevel != 1;
        if (result)
        {
            _currentDungeonLevel--;
            _dungeonLevelText.text = _currentDungeonLevel.ToString();
        }
        _rightButton.gameObject.SetActive(_currentDungeonLevel != Managers.PlayerManager.playerData.MaxDungeonLevel[_slotNum]);
        _rewardText.text = ValueCalculator.GetDungeonReward((DungeonType)_slotNum, _currentDungeonLevel).ToString();
        LeftRightCheck();
    } 
    private void RightButtonClick()
    {
        bool result = _currentDungeonLevel != Managers.PlayerManager.playerData.MaxDungeonLevel[_slotNum];
        if(result)
        {
            _currentDungeonLevel++;
            _dungeonLevelText.text = _currentDungeonLevel.ToString();
        }
        _leftButton.gameObject.SetActive(_currentDungeonLevel != 1);
        _rewardText.text = ValueCalculator.GetDungeonReward((DungeonType)_slotNum, _currentDungeonLevel).ToString();
        LeftRightCheck();
    }

    private void LeftRightCheck()
    {
        if (_currentDungeonLevel == 1)
        {
            _leftButton.gameObject.SetActive(false);
        }
        else
        {
            _leftButton.gameObject.SetActive(true);
        }
        if (_currentDungeonLevel != Managers.PlayerManager.playerData.MaxDungeonLevel[_slotNum])
        {
            _rightButton.gameObject.SetActive(true);
        }
        else
        {
            _rightButton.gameObject.SetActive(false);
        }
    }
}
