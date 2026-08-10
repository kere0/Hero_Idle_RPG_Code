using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDropper
{
    private readonly float _groundY = 1.05f;
    public void GoldDrop(Vector3 position, int goldValue)
    {
        Gold gold = Managers.Resource.Instantiate("Coin", pooling: true).GetComponent<Gold>();
        //gold.value = goldValue;
        gold.transform.position = position;
        //gold.transform.DOJump(gold.transform.position + new Vector3(0, 0.1f, 0), 0.5f,1, 0.2f).SetEase(Ease.Linear);
        Sequence bounceSequence = DOTween.Sequence();
        
        Vector3 jump1 = gold.transform.position + new Vector3(0.7f, 0, 0);
        Vector3 jump2 = jump1 + new Vector3(0.5f, 0, 0);
        Vector3 jump3 = jump2 + new Vector3(0.3f, 0, 0);
        jump1.y = _groundY;
        jump2.y = _groundY;
        jump3.y = _groundY;
        bounceSequence.Append(gold.transform.DOJump(jump1, 1.5f,1,0.3f).SetEase(Ease.OutQuad));
        bounceSequence.Append(gold.transform.DOJump(jump2, 1f,1,0.3f).SetEase(Ease.OutQuad));
        bounceSequence.Append(gold.transform.DOJump(jump3, 0.7f,1,0.3f).SetEase(Ease.OutQuad));
        bounceSequence.AppendCallback(() =>
        {
            // 돈오르게
            GameContainer.Instance.BattleManager.RewardSystem.GainReward(ItemType.Gold,  goldValue);
            Managers.Pool.ObjPush(gold.gameObject);
        });
    }
    public void EnhanceStoneDrop(Vector3 position, int value)
    {
        EnhanceStone enhanceStone = Managers.Resource.Instantiate("EnhanceStone", pooling: true).GetComponent<EnhanceStone>();
        enhanceStone.transform.position = position;
        Vector3 jump = enhanceStone.transform.position + new Vector3(0.3f, 0, 0);
        jump.y = _groundY;
        Sequence seq = DOTween.Sequence();
        seq.Append(enhanceStone.transform.DOJump(jump, 1.5f, 1, 0.3f).SetEase(Ease.OutQuad));
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() =>
        {
            GameContainer.Instance.BattleManager.RewardSystem.GainReward(ItemType.EnhanceStone,  value);
            Managers.Pool.ObjPush(enhanceStone.gameObject);
        });
        
        
    }
} 