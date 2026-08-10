using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Canvas_HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerLevelText;
    [SerializeField] private TextMeshProUGUI _expText;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _diamondText;
    [SerializeField] private Image _expBar;
    
    [SerializeField] private GameObject _progressBarUI;
    [SerializeField] private Image _progressBar;

    [SerializeField] private GameObject bossBattleUI;
    [SerializeField] private Image _timeProgressBar;
    [SerializeField] private Image _bossHpProgressBar;
    
    [SerializeField] private TextMeshProUGUI _bossHpRateText;
    [SerializeField] private TextMeshProUGUI _bossBattleTimeText;
    
    [SerializeField] private Button _bossBattleButton;
    [SerializeField] private Button _runBossBattleButton;
    // 미션
    [SerializeField] private Button _missionButton;
    [SerializeField] private GameObject _missionUI;
    // 포션
    [SerializeField] private Button _potionButton;
    [SerializeField] private GameObject _potionUI;

    [SerializeField] private Button _levelUp;
    [SerializeField] private GameObject _StagelevelUI;
    [SerializeField] private TextMeshProUGUI _stageLevelText;
    
    [SerializeField] private DungeonResultPanelUI _dungeonResultPanelUI;
    [SerializeField] private Button _sleepModeButton;
    private void Awake()
    {
        _bossBattleButton.onClick.AddListener(BossBattleClick);
        _runBossBattleButton.onClick.AddListener(RunBossBattleClick);
        _missionButton.onClick.AddListener(()=> _missionUI.SetActive(true));
        _potionButton.onClick.AddListener(() => _potionUI.SetActive(true));
        _sleepModeButton.onClick.AddListener(()=> GameContainer.Instance.SleepModeManager.EnterSleepMode());
    }
    private void Start()
    {
        RefreshExp();
        RefreshGold();
        RefreshDiamond();
        RefreshProgressBar(0);
        Managers.PlayerManager.OnLevelUp += RefreshLevel;
        Managers.PlayerManager.OnExpChanged += RefreshExp;
        Managers.PlayerManager.OnGoldChanged += RefreshGold;
        Managers.PlayerManager.OnDiamondChanged += RefreshDiamond;
        GameContainer.Instance.BattleManager.OnMonsterGroupClear += RefreshProgressBar;
        _levelUp.onClick.AddListener(Managers.PlayerManager.PlayerInfoSystem.LevelUp);
    }
    private void RefreshLevel()
    {
        _playerLevelText.text = Managers.PlayerManager.playerData.Level.ToString();
    }
    private void RefreshExp()
    {
        float percent = (float)Managers.PlayerManager.playerData.Exp / Managers.PlayerManager.PlayerInfoSystem.MaxExp;
        _expText.text = $"{percent * 100f:0.##}%";
        _expBar.fillAmount = percent;
    }
    private void RefreshGold()
    {
        _goldText.text = Mathf.RoundToInt(Managers.PlayerManager.playerData.Gold).ToString();
    }
    private void RefreshDiamond()
    {
        _diamondText.text = Mathf.RoundToInt(Managers.PlayerManager.playerData.Diamond).ToString();
    }
    private void RefreshProgressBar(int amount)
    {
        _progressBar.DOKill();
        _progressBar.DOFillAmount(amount / 100f, 0.2f).SetEase(Ease.OutQuad);
    }
    public void RefreshStageLevel()
    {
        _stageLevelText.text = $"STAGE {Managers.PlayerManager.playerData.CurrentStage}";
    }
    public void StartBossBattle(bool isDungeonBoss)
    {
        if (isDungeonBoss == true)
        {
            _StagelevelUI.gameObject.SetActive(false);
        }
        _bossBattleButton.gameObject.SetActive(false);
        _progressBar.DOKill();
        _progressBarUI.SetActive(false);
        bossBattleUI.SetActive(true);
        _bossHpProgressBar.fillAmount = 1;
        _timeProgressBar.fillAmount = 1;
    }
    public void RefreshBossHpProgressBar(float amount)
    {
        _bossHpProgressBar.DOKill();
        _bossHpProgressBar.DOFillAmount(amount, 0.2f).SetEase(Ease.OutQuad);
        _bossHpRateText.text = $"{Mathf.CeilToInt(amount * 100)}%";
    }
    private void BossBattleClick()
    {
        GameContainer.Instance.BattleManager.StartBossBattle(MonsterType.StageBoss);
    }
    public void RefreshTimeProgressBar(float currentTime, int maxTime)
    {
        _timeProgressBar.DOKill();
        float maxCurrentTime = Mathf.Max(0,currentTime);
        _timeProgressBar.DOFillAmount(maxCurrentTime / maxTime, 0.2f).SetEase(Ease.OutQuad);
        _bossBattleTimeText.text = Utils.ToTimeString(maxCurrentTime);
    }
    public void EndBossBattle()
    {
        if (_StagelevelUI.gameObject.activeSelf == false)
        {
            _StagelevelUI.gameObject.SetActive(true);
        }
        _bossBattleButton.gameObject.SetActive(true);
        _progressBar.fillAmount = 0;
        bossBattleUI.SetActive(false);
        _progressBarUI.SetActive(true);
    }

    public void ViewDungeonRewardPanel(MonsterType monsterType, int rewardValue)
    {
        if (_dungeonResultPanelUI.gameObject.activeSelf == false)
        {
            DOVirtual.DelayedCall(2f, () =>
            {
                _dungeonResultPanelUI.gameObject.SetActive(true);
                _dungeonResultPanelUI.SetResultPanel(monsterType, rewardValue);
            });
        }
    }
    private void RunBossBattleClick()
    {
        GameContainer.Instance.BattleManager.RunBossBattle();
    }
    private void OnDestroy()
    {
        Managers.PlayerManager.OnLevelUp -= RefreshLevel;
        Managers.PlayerManager.OnExpChanged -= RefreshExp;
        Managers.PlayerManager.OnGoldChanged -= RefreshGold;
        Managers.PlayerManager.OnDiamondChanged -= RefreshDiamond;
    }
}
