using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AutoButtonUI : MonoBehaviour
{
    private Button _autoButton;
    [SerializeField] private Image _autoButtonImage;
    [SerializeField] private TextMeshProUGUI _autoText;
    private bool _isAuto = false;
    private readonly Color _disabledColor = new Color(0.55f, 0.55f, 0.55f, 1);
    private void Awake()
    {
        TryGetComponent(out _autoButton);
        _autoButton.onClick.AddListener(AutoButtonClick);
        _autoButtonImage.color = _disabledColor;
        _autoText.color = _disabledColor;
    }
    private void Start()
    {
        _autoButton.onClick.AddListener(GameContainer.Instance.Player.ToggleAutoMode);
    }

    private void AutoButtonClick()
    {
        _isAuto = !_isAuto;
        if (_isAuto == true)
        {
            _autoButtonImage.color = Color.white;
            _autoText.color = Color.white;
        }
        else
        {
            _autoButtonImage.color = _disabledColor;
            _autoText.color = _disabledColor;
        }
    }
    private void Update()
    {
        if (_isAuto == true)
        {
            _autoButtonImage.transform.Rotate(0f, 0f, 100f * Time.deltaTime);
        }
    }
}
