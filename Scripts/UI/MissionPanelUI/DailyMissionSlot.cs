using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionSlot : MonoBehaviour
{
    public int _slotNum;
    public DailyMissionPanelUI dailyMissionPanel;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private Image _progressImage;
    [SerializeField] private Button _rewardButton;
    private bool _isRewardReceived = false;
    private void Awake()
    {
        _rewardButton.onClick.AddListener(CompleteButtonClick);
    }
    public void InitInfo(int slotNum, string description, int currentValue, int maxValue, int reward, bool isCompleted)
    {
        _slotNum = slotNum;
        _descriptionText.text = description;
        int minCurrentValue = Mathf.Min(currentValue, maxValue);
        _progressText.text = $"{minCurrentValue} / {maxValue}";
        _rewardText.text = reward.ToString();
        _progressImage.fillAmount = (float)currentValue / maxValue;
        if (_isRewardReceived == false)
        {
            _rewardButton.interactable = isCompleted;
        }
    }
    public void Refresh(int currentValue, int maxValue, bool isCompleted)
    {
        int minCurrentValue = Mathf.Min(currentValue, maxValue);
        _progressText.text = $"{minCurrentValue} / {maxValue}";
        _progressImage.fillAmount = (float)currentValue / maxValue;
        if (_isRewardReceived == false)
        {
            _rewardButton.interactable = isCompleted;
        }
    }
    private void CompleteButtonClick()
    {
        GameContainer.Instance.UIFeedbackManager.Play(_rewardButton.transform);
        dailyMissionPanel.DailyMissionCompleteButtonClick(_slotNum);
        _isRewardReceived = true;
        _rewardButton.interactable = false;
    }
}


