using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SleepModeManager : MonoBehaviour
{
    [SerializeField] private Canvas _sleepModeCanvas;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas[] _canvases;
    private const float SleepModeDelay = 180f;
    private const int NormalFrame = 60;
    private const int SleepFrame = 15;
    private const float UnlockPressedTime = 2f;
    private bool _isSleepMode;
    private float _inactiveTime;
    
    [SerializeField] private HoldButton _unlockButton;
    [SerializeField] private Image _unlockGaugeImage;
    private void Awake()
    {
        Application.targetFrameRate = NormalFrame;
        _sleepModeCanvas.gameObject.SetActive(false);
        _unlockGaugeImage.gameObject.SetActive(false);
        _unlockButton.OnHoldStart += StartUnlockGauge;
        _unlockButton.OnHoldEnd += ResetUnlockGauge;
    }
    private void Update()
    {
        if (_isSleepMode == false)
        {
            if (Input.touchCount > 0)
            {
                _inactiveTime = 0f;
                return;
            }
            _inactiveTime += Time.unscaledDeltaTime;

            if (_inactiveTime >= SleepModeDelay)
            {
                EnterSleepMode();
            }
        }
        else if (_isSleepMode == true && _unlockButton.isHolding == true)
        {
        
            _unlockGaugeImage.fillAmount = _unlockButton.HoldTime / UnlockPressedTime;
            if (_unlockGaugeImage.fillAmount >= 1f)
            {
                ExitSleepMode();
            }
        }
    }
    private void StartUnlockGauge()
    {
        if (_isSleepMode == false)
            return;

        Application.targetFrameRate = NormalFrame;

        _unlockGaugeImage.fillAmount = 0f;
        _unlockGaugeImage.gameObject.SetActive(true);
    }
    public void EnterSleepMode()
    {
        if (_isSleepMode == true) return;
        _isSleepMode = true;
        _camera.enabled = false;
        SoundManager.Instance.PauseBGM();
        SoundManager.Instance.SetSFXMuted(true);
        foreach (Canvas canvas in _canvases)
        {
            canvas.enabled = false;
        }
        _sleepModeCanvas.gameObject.SetActive(true);
        Application.targetFrameRate = SleepFrame;
    }
    private void ExitSleepMode()
    {
        if (_isSleepMode == false) return;
        _isSleepMode = false;
        _inactiveTime = 0f;
        _camera.enabled = true;
        SoundManager.Instance.ResumeBGM();
        SoundManager.Instance.SetSFXMuted(false);
        foreach (Canvas canvas in _canvases)
        {
            canvas.enabled = true;
        }
        _sleepModeCanvas.gameObject.SetActive(false);
        _unlockGaugeImage.fillAmount = 0f;
        Application.targetFrameRate = NormalFrame;
    }
    private void ResetUnlockGauge()
    {
        _unlockGaugeImage.fillAmount = 0f;
        _unlockGaugeImage.gameObject.SetActive(false);
        if (_isSleepMode == true)
        {
            Application.targetFrameRate = SleepFrame;
        }
    }
}
