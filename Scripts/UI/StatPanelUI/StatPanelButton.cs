using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPanelButton : MonoBehaviour
{
    private Button _button;
    [SerializeField] private GameObject _statPanel;
    private void Awake()
    {
        TryGetComponent(out _button);
        _button.onClick.AddListener(StatButtonClick);
    }
    private void StatButtonClick()
    {
        _statPanel.SetActive(!_statPanel.activeSelf);
    }
}
