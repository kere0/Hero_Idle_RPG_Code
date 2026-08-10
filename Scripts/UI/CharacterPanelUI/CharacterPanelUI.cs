using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelUI : MonoBehaviour
{
    [SerializeField] private EnhancePanelUI enhancePanelUI;
    [SerializeField] private GrowthPanelUI growthPanelUI;
    [SerializeField] private Button enhancePanelButton;
    [SerializeField] private Button growthPanelButton;
    [SerializeField] private TextMeshProUGUI _statPontText;

    [SerializeField] private GameObject _enhanceOutline;
    [SerializeField] private GameObject _growthOutline;
    [SerializeField] private TextMeshProUGUI _enhanceText;
    [SerializeField] private TextMeshProUGUI _growthText;
    
    private Color _defaultColor =  new Color(0.7f, 0.7f, 0.7f, 1f);
    private void Awake()
    {
        enhancePanelButton.onClick.AddListener(EnhancePanelButtonClick);
        growthPanelButton.onClick.AddListener(GrowthPanelButtonClick);
    }
    private void OnEnable()
    {
        if (growthPanelUI.gameObject.activeInHierarchy == true)
        {
            StatPointRefresh();
        }
    }
    private void Start()
    {
        Managers.PlayerManager.OnGrowthChanged += StatPointRefresh;
        Managers.PlayerManager.OnLevelUp += StatPointRefresh;
        _enhanceOutline.SetActive(true);
        _growthOutline.SetActive(false);
        _enhanceText.color = Color.white;
        _growthText.color = _defaultColor;
    }
    private void EnhancePanelButtonClick()
    {
        enhancePanelUI.gameObject.SetActive(true);
        growthPanelUI.gameObject.SetActive(false);
        _statPontText.gameObject.SetActive(false);
        _enhanceOutline.SetActive(true);
        _growthOutline.SetActive(false);
        _enhanceText.color = Color.white;
        _growthText.color = _defaultColor;
    }
    private void GrowthPanelButtonClick()
    {
        enhancePanelUI.gameObject.SetActive(false);
        growthPanelUI.gameObject.SetActive(true);
        _statPontText.gameObject.SetActive(true);
        _growthOutline.SetActive(true);
        _enhanceOutline.SetActive(false);
        _enhanceText.color = _defaultColor;
        _growthText.color = Color.white;
        StatPointRefresh();
    }
    private void StatPointRefresh()
    {
        _statPontText.text = $"스탯 포인트 : {Managers.PlayerManager.playerData.StatPoint}";
    }
    private void OnDestroy()
    {
        Managers.PlayerManager.OnGrowthChanged -= StatPointRefresh;
        Managers.PlayerManager.OnLevelUp -= StatPointRefresh;
    }
}
