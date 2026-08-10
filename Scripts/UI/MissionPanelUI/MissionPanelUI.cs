using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanelUI : MonoBehaviour
{
    [SerializeField] private DailyMissionPanelUI _dailyMissionPanelUI;
    [SerializeField] private AchievementPanelUI  _achievementPanelUI;
    [SerializeField] private Button _dailyMissionPanelButton;
    [SerializeField] private Button _achievementPanelButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _backgroundPanel;
    
    [SerializeField] private GameObject _dailyMissionOutline;
    [SerializeField] private GameObject _achievementOutline;
    [SerializeField] private TextMeshProUGUI _dailyMissionText;
    [SerializeField] private TextMeshProUGUI _achievementText;
    
    private Color _defaultColor =  new Color(0.7f, 0.7f, 0.7f, 1f);
    private void Awake()
    {
        _dailyMissionPanelButton.onClick.AddListener(DailyMissionPanelButtonClick);
        _achievementPanelButton.onClick.AddListener(AchievementButtonClick);
        _closeButton.onClick.AddListener(()=> gameObject.SetActive(false));
        _backgroundPanel.onClick.AddListener(()=> gameObject.SetActive(false));
    }

    private void Start()
    {
        _dailyMissionOutline.gameObject.SetActive(true);
        _achievementOutline.gameObject.SetActive(false);
        _dailyMissionText.color = Color.white;
        _achievementText.color = _defaultColor;
    }

    private void DailyMissionPanelButtonClick()
    {
        _dailyMissionPanelUI.gameObject.SetActive(true);
        _achievementPanelUI.gameObject.SetActive(false);
        _dailyMissionOutline.gameObject.SetActive(true);
        _achievementOutline.gameObject.SetActive(false);
        _dailyMissionText.color = Color.white;
        _achievementText.color = _defaultColor;
    }
    private void AchievementButtonClick()
    {
        _dailyMissionPanelUI.gameObject.SetActive(false);
        _achievementPanelUI.gameObject.SetActive(true);
        _dailyMissionOutline.gameObject.SetActive(false);
        _achievementOutline.gameObject.SetActive(true);
        _dailyMissionText.color = _defaultColor;
        _achievementText.color = Color.white;
    }
}
