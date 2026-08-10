using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AcquireInfoSlot : MonoBehaviour
{
    [SerializeField] private GameObject _expImage;
    [SerializeField] private GameObject _goldImage;
    [SerializeField] private GameObject _enhanceStoneImage;
    [SerializeField]  private TextMeshProUGUI _valueText;
    public ItemType itemType;
    private readonly float _fadeTime = 0.3f;
    private float _endTime;
    public bool isActive;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void SetInfo(string text, float duration, ItemType itemType)
    {
        this.itemType = itemType;
        SetImage(itemType);
        _valueText.text = text;
        _endTime = duration + Time.time;
        isActive = true;
    }
    public (string text,float endTime) GetInfo()
    {
        return (_valueText.text, _endTime);
    }

    public void PushInfo(string text, float endTime, ItemType itemType)
    {
        this.itemType = itemType;
        SetImage(itemType);
        _valueText.text = text;
        _endTime = endTime;
        isActive = true;
    }
    private void SetImage(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Exp:
                _expImage.SetActive(true);
                _goldImage.SetActive(false);
                _enhanceStoneImage.SetActive(false);
                break;
            case ItemType.Gold:
                _expImage.SetActive(false);
                _goldImage.SetActive(true);
                _enhanceStoneImage.SetActive(false);
                break;
            case ItemType.EnhanceStone:
                _expImage.SetActive(false);
                _goldImage.SetActive(false);
                _enhanceStoneImage.SetActive(true);
                break;
        }
    }
    
    private void Update()
    {
        if(isActive == false) return;
        float remainingTime = _endTime - Time.time;
        float t = Mathf.Clamp01(remainingTime / _fadeTime);
        _canvasGroup.alpha = t;
        if (remainingTime <= 0)
        {
            _canvasGroup.alpha = 0;
            isActive = false;
            GameContainer.Instance.AcquireInfoUI.EndFadeTime();
        }
    }
}
 