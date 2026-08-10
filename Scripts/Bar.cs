using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    private static readonly int FillAmountID = Shader.PropertyToID("_FillAmount");
    private Renderer hpBarRenderer;
    
    private Material mp;
    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
        TryGetComponent(out hpBarRenderer);
    }
    public void SetFillAmount(float fillAmount)
    {
        mpb.SetFloat(FillAmountID, fillAmount);
        hpBarRenderer.SetPropertyBlock(mpb);
    }
}
