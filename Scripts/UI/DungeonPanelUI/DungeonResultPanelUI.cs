using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonResultPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private GameObject _goldImage;
    [SerializeField] private GameObject _expImage;
    [SerializeField] private GameObject _enhanceStoneImage;
    [SerializeField] private TextMeshProUGUI _exitCountdownText;
    [SerializeField] private Button _exitButton;
    private readonly int _exitCountdown = 2;
    private Coroutine _coroutine;

    private void Awake()
    {
        _exitButton.onClick.AddListener(ExitButtonClick);
    }

    public void SetResultPanel(MonsterType monsterType, int rewardValue)
    {
        switch (monsterType)
        {
            case MonsterType.GoldDungeon:
                _titleText.text = "획득 골드";
                _goldImage.SetActive(true);
                _expImage.SetActive(false);
                _enhanceStoneImage.SetActive(false);
                break;
            case MonsterType.ExpDungeon:
                _titleText.text = "획득 경험치";
                _goldImage.SetActive(false);
                _expImage.SetActive(true);
                _enhanceStoneImage.SetActive(false);
                break;
            case MonsterType.EnhanceStoneDungeon:
                _titleText.text = "획득 강화석";
                _goldImage.SetActive(false);
                _expImage.SetActive(false);
                _enhanceStoneImage.SetActive(true);
                break;
        }
        _valueText.text = rewardValue.ToString("N0");
        _exitCountdownText.text = $"{_exitCountdown}초 뒤 나가집니다.";

        _coroutine = StartCoroutine(ExitCountCoroutine());
    }

    private void ExitButtonClick()
    {
        RemoveCoroution();
        GameContainer.Instance.BattleManager.ReStart(0f, 0f, ()=>
        {
            GameContainer.Instance.MenuPanelUI.EndBossBattle();
            GameContainer.Instance.HUD.EndBossBattle();
        });
        gameObject.SetActive(false);
    }
    private IEnumerator ExitCountCoroutine()
    {
        int count = _exitCountdown;
        while (count >= 0)
        {
            _exitCountdownText.text = $"{count}초 뒤 나가집니다.";
            yield return new WaitForSeconds(1f);
            count--;
        }
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        RemoveCoroution();
    }
    private void RemoveCoroution()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}
