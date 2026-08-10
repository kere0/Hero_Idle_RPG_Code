using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ViewPotionSlot : MonoBehaviour
{
    public PotionType potionType;
    private ViewPotionBuffPanel _viewPotionBuffPanel;
    private bool _isBuffStart = false;
    private float _leftTime = 0;
    [SerializeField] private TextMeshProUGUI _leftTimeText;
    public void Init(PotionType potionType, ViewPotionBuffPanel viewPotionBuffPanel)
    {
        _viewPotionBuffPanel = viewPotionBuffPanel;
        this.potionType = potionType;
    }
    public void StartBuff(float leftTime)
    {
        if (_isBuffStart == false)
        {
            _isBuffStart = true;
            _leftTime = leftTime;
        }
    }
    private void Update()
    {
        if (_isBuffStart == false) return;
        if (_leftTime > 0)
        {
            _leftTime -= Time.deltaTime;
            _leftTime = Mathf.Max(0f, _leftTime - Time.deltaTime);
            _leftTimeText.text = $"{Mathf.CeilToInt(_leftTime / 60f)}m";
        }
        else
        {
            _isBuffStart = false;
            _viewPotionBuffPanel.EndAdBuff(this);
        }
    }
}
