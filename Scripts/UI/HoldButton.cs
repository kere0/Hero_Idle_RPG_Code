using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : Button
{
    public bool isHolding;

    public float HoldTime { get; private set; }
    
    public event Action OnHoldStart;
    public event Action OnHoldEnd;
    
    private const float LongPressTime = 0.3f;

    public bool IsLongPressing => isHolding && HoldTime >= LongPressTime;

    private void Update()
    {
        if (isHolding == false) return;
        HoldTime += Time.unscaledDeltaTime;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (interactable == false) return;
        isHolding = true;
        HoldTime = 0f;
        OnHoldStart?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        isHolding = false;
        HoldTime = 0f;
        OnHoldEnd?.Invoke();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        isHolding = false;
        HoldTime = 0f;
    }
}