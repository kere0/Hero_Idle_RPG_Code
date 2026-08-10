using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSlot : MonoBehaviour
{
    public int _slotNum;
    public AchievementPanelUI achievementPanelUI;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private Image _progressImage;
    [SerializeField] private Button _rewardButton;
    private void Awake()
    {
        _rewardButton.onClick.AddListener(CompleteButtonClick);
    }
    public void InitInfo(int slotNum, string description, int currentValue, int maxValue, int reward, bool isCompleted)
    {
        _slotNum = slotNum;
        _descriptionText.text = description;
        _progressText.text = $"{currentValue} / {maxValue}";
        _rewardText.text = reward.ToString();
        _progressImage.fillAmount = (float)currentValue / maxValue;
        _rewardButton.interactable = isCompleted;
    }
    public void Refresh(int currentValue, int maxValue, bool isCompleted)
    {
        _progressText.text =  $"{currentValue} / {maxValue}";
        _progressImage.fillAmount = (float)currentValue / maxValue;
        _rewardButton.interactable = isCompleted;
    }
    private void CompleteButtonClick()
    {
        GameContainer.Instance.UIFeedbackManager.Play(_rewardButton.transform);
        achievementPanelUI.AchievementCompleteButtonClick(_slotNum);
    }
}
