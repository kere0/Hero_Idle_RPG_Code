using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    private Tween _scaleTween;
    private void OnEnable()
    {
        transform.localScale = Vector3.one;

        _scaleTween = transform.DOScale(1.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void OnDisable()
    {
        _scaleTween?.Kill();
        _scaleTween = null;
        transform.localScale = Vector3.one;
    }
}
