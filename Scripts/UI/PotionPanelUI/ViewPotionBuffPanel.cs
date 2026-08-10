using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ViewPotionBuffPanel : MonoBehaviour
{
    [SerializeField] private ViewPotionSlot[] _viewPotionSlot = new ViewPotionSlot[3];
    [SerializeField] private Transform _buffSlotPool;
    private void Start()
    {
        for (int i = 0; i < _viewPotionSlot.Length; i++)
        {
            _viewPotionSlot[i].Init((PotionType)i, this);
        }
    }
    public void ApplyAdBuff(PotionType potionType, float time)
    {
        foreach (ViewPotionSlot viewPotionSlot in _viewPotionSlot)
        {
            if (viewPotionSlot.potionType == potionType)
            {
                viewPotionSlot.gameObject.SetActive(true);
                viewPotionSlot.transform.SetParent(transform);
                viewPotionSlot.StartBuff(time);
            }
        }
    }
    public void EndAdBuff(ViewPotionSlot viewPotionSlot)
    {
        viewPotionSlot.transform.SetParent(_buffSlotPool);
        viewPotionSlot.gameObject.SetActive(false);
        Managers.PlayerManager.AdBuffSystem.EndAdBuff(viewPotionSlot.potionType);
    }
}
