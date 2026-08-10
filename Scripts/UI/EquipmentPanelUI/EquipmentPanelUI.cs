using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EquipmentPanelUI : MonoBehaviour
{
    [SerializeField] private SwordSlotPanelUI _swordSlotPanelUI;
    [SerializeField] private RingSlotPanelUI _ringSlotPanelUI;
    [SerializeField] private Button _swordPanelButton;
    [SerializeField] private Button _ringPanelButton;
    [SerializeField] private Button _MergeAllButton;
    
    [SerializeField] private GameObject _swordOutline;
    [SerializeField] private GameObject _ringOutline;
    [SerializeField] private TextMeshProUGUI _swordText;
    [SerializeField] private TextMeshProUGUI _ringText;
    
    private Color _defaultColor =  new Color(0.7f, 0.7f, 0.7f, 1f);
    private void Awake()
    {
        _swordPanelButton.onClick.AddListener(WeaponPanelButtonClick);
        _ringPanelButton.onClick.AddListener(RingPanelButtonClick);
        _MergeAllButton.onClick.AddListener(MergeAllButtonClick);
    }

    private void Start()
    {
        _swordOutline.gameObject.SetActive(true);
        _ringOutline.gameObject.SetActive(false);
        _swordText.color = Color.white;
        _ringText.color = _defaultColor;
    }

    private void WeaponPanelButtonClick()
    {
        _swordSlotPanelUI.gameObject.SetActive(true);
        _ringSlotPanelUI.gameObject.SetActive(false);
        _swordOutline.gameObject.SetActive(true);
        _ringOutline.gameObject.SetActive(false);
        _swordText.color = Color.white;
        _ringText.color = _defaultColor;
    }
    private void RingPanelButtonClick()
    {
        _swordSlotPanelUI.gameObject.SetActive(false);
        _ringSlotPanelUI.gameObject.SetActive(true);
        _swordOutline.gameObject.SetActive(false);
        _ringOutline.gameObject.SetActive(true);
        _ringText.color = Color.white;
        _swordText.color = _defaultColor;
    }
    private void MergeAllButtonClick()
    {
        if (_swordSlotPanelUI.gameObject.activeSelf)
        {
            Managers.PlayerManager.EquipmentSystem.MergeAllEquipment(EquipmentType.Sword);
            _swordSlotPanelUI.Refresh();
        }
        else if(_ringSlotPanelUI.gameObject.activeSelf)
        {
            Managers.PlayerManager.EquipmentSystem.MergeAllEquipment(EquipmentType.Ring);
            _ringSlotPanelUI.Refresh();
        }
    }
}
