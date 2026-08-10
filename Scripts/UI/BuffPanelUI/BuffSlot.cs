using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuffSlot : MonoBehaviour
{
    private BuffPanelUI _buffUIPanelUI;
    [SerializeField] private Image _skillImage;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private TextMeshProUGUI _valueText; // cooldown or count
    [SerializeField] private GameObject _activeBackground;
    [SerializeField] public GameObject _passiveBackground;
    public SkillTableSO.SkillInfo currentSkillInfo;
    private float _duration;
    public Coroutine passiveCoroutine;
    public void SetInfo(SkillTableSO.SkillInfo skillInfo, BuffPanelUI buffPanelUI)
    {
        _buffUIPanelUI = buffPanelUI;
        currentSkillInfo = skillInfo;
        _skillImage.sprite = skillInfo.sprite;
        if (currentSkillInfo.skillCategory == SKillCategory.Active)
        {
            _duration = currentSkillInfo.duration;
            _activeBackground.SetActive(true);
            _passiveBackground.SetActive(false);
            _cooldownImage.gameObject.SetActive(true);

        }
        else if (currentSkillInfo.skillCategory == SKillCategory.Passive)
        {
            _duration = currentSkillInfo.cooldown;
            _passiveBackground.SetActive(true);
            _activeBackground.SetActive(false);
            passiveCoroutine = StartCoroutine(PassiveCoroutine());
            _cooldownImage.gameObject.SetActive(false);
        }
    }
    public void Reset()
    {
        _duration = 0;
        _buffUIPanelUI.buffSlots.Remove(this); 
        Managers.Pool.ObjPush(gameObject);
    }
    private void Update()
    {
        if (_buffUIPanelUI == null) return;
        if (currentSkillInfo.skillCategory == SKillCategory.Active)
        {
            if (_duration > 0)
            {
                _duration -= Time.deltaTime;
                float cooldown = _duration / currentSkillInfo.duration;
                _cooldownImage.fillAmount = cooldown;
                _valueText.text = Mathf.CeilToInt(_duration).ToString();
            }
            else
            {
                _buffUIPanelUI.buffSlots.Remove(this); 
                Managers.Pool.ObjPush(gameObject);
            }
        }
    }
    private IEnumerator PassiveCoroutine()
    {
        int useCount = 0;
        while (useCount < PassiveSkillInstance.MaxPassiveStack)
        {
            useCount++;
            _valueText.text = useCount.ToString();
            yield return new WaitForSeconds(_duration);
        }
    }

    private void OnDisable()
    {
        if (passiveCoroutine != null)
        {
            StopCoroutine(passiveCoroutine);
        }
        passiveCoroutine = null;
    }
}
