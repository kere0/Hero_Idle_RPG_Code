using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevelViewPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _unlockLevelText;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _backgroundPanel;

    private void Awake()
    {
        _closeButton.onClick.AddListener(()=> gameObject.SetActive(false));
        _backgroundPanel.onClick.AddListener(()=> gameObject.SetActive(false));
    }
    public void SetUnlockLevelText(string unlockLevelText) 
    {
        _unlockLevelText.text =$"레벨 {unlockLevelText}에 잠금 해제";
    }
}
