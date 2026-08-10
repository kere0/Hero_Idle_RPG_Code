using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _damageText;
    private readonly Color _normalAttackColor = new Color(1, 0.3695f,0,1);
    private readonly Color _criticalAttackColor = new Color(0.8196f, 0,1,1);
    public void SetText(Vector3 pos, int damage, bool isCritical, CreatureType creatureType)
    {
        transform.position = pos;
        if (creatureType == CreatureType.Monster)
        {
            if (isCritical == true)
            {
                _damageText.color = _criticalAttackColor;
            }
            else
            {
                _damageText.color = _normalAttackColor;
            }
        }
        else if(creatureType == CreatureType.Player)
        {
            _damageText.color = Color.red;
        }
        _damageText.text = damage.ToString("N0");
        _damageText.transform.localScale = new Vector3(1, 1, 1);
        Sequence seq = DOTween.Sequence();
        seq.Append(_damageText.transform.DOScale(1.35f, 0.15f))
            .Append(_damageText.transform.DOScale(1f, 0.15f))
            .Join(transform.DOMoveY(transform.position.y + 1f, 1f))
            .Join(_damageText.DOFade(0f, 1f))
            .OnComplete(() => Managers.Pool.ObjPush(gameObject));
    }
}
