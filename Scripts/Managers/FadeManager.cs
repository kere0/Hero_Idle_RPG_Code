using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType
{
    Skill = 0,
    StageClear = 2,
    Boss = 7
}
public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private SpriteRenderer _fadeSprite;
    private Sequence _spriteFadeSequence;
    private void Awake()
    {
        Instance = this;
        _fadeImage.gameObject.SetActive(false);
        _fadeImage.color = Color.black;
    }
    public Tween FadeIn(float duration, FadeType fadeType)
    {
        if (_fadeImage.gameObject.activeSelf == false)
        {
            _fadeImage.gameObject.SetActive(true);
        }
        Sequence _sequence = DOTween.Sequence();
        _sequence.AppendCallback(() =>
        {
            _canvas.sortingOrder = (int)fadeType;
            _fadeImage.color = Color.clear;
      
        });
        _sequence.Append(_fadeImage.DOFade(1f, duration));
        return _sequence;
    }
    public Tween FadeOut(float duration) 
    {
        if (_fadeImage.gameObject.activeSelf == false)
        {
            _fadeImage.gameObject.SetActive(true);
        }
        Sequence _sequence = DOTween.Sequence();
        _sequence.AppendCallback(() => _fadeImage.color = Color.black);
        _sequence.Append(_fadeImage.DOFade(0f, duration).OnComplete(() => _fadeImage.gameObject.SetActive(false)));
        return _sequence; 
    }
    public void SpriteFadeInOut(float fadeInDuration, float holdTime, float fadeOutDuration, FadeType fadeType)
    {
        _spriteFadeSequence?.Kill();
        _fadeSprite.DOKill();
        _fadeSprite.color = Color.black;
        if (_fadeSprite.gameObject.activeSelf == false)
        {
            _fadeSprite.gameObject.SetActive(true);
        } 
        _spriteFadeSequence = DOTween.Sequence(); 
        _spriteFadeSequence.Append(_fadeSprite.DOFade(1f, fadeInDuration)); 
        _spriteFadeSequence.AppendInterval(holdTime); 
        _spriteFadeSequence.Append(_fadeSprite.DOFade(0f, fadeOutDuration)); 
        _spriteFadeSequence.OnComplete(() =>
        {
            _fadeSprite.gameObject.SetActive(false);
            _spriteFadeSequence = null;
        }); 
    }
}