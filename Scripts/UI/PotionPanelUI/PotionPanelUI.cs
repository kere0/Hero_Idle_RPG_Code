using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionPanelUI : MonoBehaviour
{
    public const float PotionBuffTime = 1800;
    public const float PotionBuffRate = 200f;
    [SerializeField] private GameObject _potionPanel;
    [SerializeField] private Button _closeButton;
    [SerializeField] private PotionSlot[] _potionSlot = new PotionSlot[3];
    [SerializeField] private Button _backgroundPanel;

    private void Awake()
    {
        _closeButton.onClick.AddListener(() =>  _potionPanel.SetActive(false));
        _backgroundPanel.onClick.AddListener(()=> _potionPanel.SetActive(false));

    }
    private void OnEnable()
    {
        for (int i = 0; i < _potionSlot.Length; i++)
        {
            _potionSlot[i].Init(i);
        }
    }
}
