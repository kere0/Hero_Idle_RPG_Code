using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseSummonSlot : MonoBehaviour
{
    [SerializeField] private GameObject _equipmentSlotImage;
    [SerializeField] private Image _frameImage;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _effectImage;
    [SerializeField] private TextMeshProUGUI _starGradeText;
    [SerializeField] private GameObject _legendEffect;
    [SerializeField] private ParticleSystem _legendEffectParticle;
    
    private Sequence _sequence;
     public void SetInfo(int itemId, int starLevelText)
    {
        // 이전 연출 제거
        _sequence?.Kill();
        _sequence = null;

        // 혹시 개별 Tween이 남아있을 경우 제거
        _effectImage.transform.DOKill();
        _effectImage.DOKill();

        // 파티클 초기화
        _legendEffectParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // 전설 이펙트 초기화
        _legendEffect.SetActive(false);

        // 기본 UI 초기화
        _equipmentSlotImage.SetActive(false);
        _effectImage.gameObject.SetActive(true);

        _effectImage.color = new Color(1f, 1f, 1f, 0.9f);
        _effectImage.transform.localScale = Vector3.one * 1.5f;

        _starGradeText.text = starLevelText.ToString();

        // 장비 등급에 따른 색상 설정
        if (itemId < 4)
        {
            _frameImage.color = EquipmentGachaManager.NormalFrameColor;
            _backgroundImage.color = EquipmentGachaManager.NormalBackgroundColor;
        }
        else if (itemId < 8)
        {
            _frameImage.color = EquipmentGachaManager.RareFrameColor;
            _backgroundImage.color = EquipmentGachaManager.RareBackgroundColor;
        }
        else if (itemId < 12)
        {
            _frameImage.color = EquipmentGachaManager.UniqueFrameColor;
            _backgroundImage.color = EquipmentGachaManager.UniqueBackgroundColor;
        }
        else if (itemId < 16)
        {
            _frameImage.color = EquipmentGachaManager.LegendFrameColor;
            _backgroundImage.color = EquipmentGachaManager.LegendBackgroundColor;
        }

        bool isLegend = itemId >= 12 && itemId < 16;

        // 소환 연출 시작
        _sequence = DOTween.Sequence();

        _sequence.Append(_effectImage.transform.DOScale(1f, 0.35f).SetEase(Ease.Linear));

        _sequence.AppendCallback(() =>
        {
            _equipmentSlotImage.SetActive(true);

            if (isLegend)
            {
                _legendEffect.SetActive(true);
                _legendEffectParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _legendEffectParticle.Play(true);
            }
        });

        _sequence.Append(_effectImage.DOFade(0f, 0.35f).SetEase(Ease.Linear));

        _sequence.OnComplete(() => { _sequence = null; });
    }

    private void OnDisable()
    {
        _sequence?.Kill();
        _sequence = null;

        _effectImage.transform.DOKill();
        _effectImage.DOKill();

        _legendEffectParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _legendEffect.SetActive(false);
    }
}

