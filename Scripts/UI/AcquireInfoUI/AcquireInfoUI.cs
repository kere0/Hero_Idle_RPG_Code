using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquireInfoUI : MonoBehaviour
{
    [SerializeField] private AcquireInfoSlot[] _acquireInfoSlots = new AcquireInfoSlot[5];
    
    private int _currentViewSlot = 0;
    private readonly float _fadeTime = 0.2f;
    public Action OnFadeEnd;
    public void PushInfoSlot(float value, ItemType itemType)
    {
        int maxCount = Mathf.Min(_currentViewSlot, _acquireInfoSlots.Length - 1);
        for (int i = maxCount; i > 0; i--)
        {
            AcquireInfoSlot acquireInfoSlot = _acquireInfoSlots[i - 1];
            _acquireInfoSlots[i].PushInfo(acquireInfoSlot.GetInfo().text, acquireInfoSlot.GetInfo().endTime, acquireInfoSlot.itemType);
        }
        _acquireInfoSlots[0].SetInfo(Mathf.RoundToInt(value).ToString(), 0.8f, itemType);
        _currentViewSlot++;
    }
    public void EndFadeTime()
    {
        _currentViewSlot--;
    }
}
