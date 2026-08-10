using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private TextMeshProUGUI _tapToStartText;
    [SerializeField] private GameObject _loadingBarPanel;
    [SerializeField] private Image _loadingBar;
    private bool _isResourceLoaded = false;
    private void Awake()
    {
        _button.onClick.AddListener(StartButtonClick);
        _fadeImage.color = Color.clear;
        _fadeImage.gameObject.SetActive(false);
    }
    private void Start()
    {
        _tapToStartText.DOFade(0f, 0.85f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        GameManager.Instance.OnResourceLoaded += OnResourceLoaded;
        Managers.Data.StartSetResourceData();
    }
    private void StartButtonClick()
    {
        if (_isResourceLoaded == false) return;
        _fadeImage.gameObject.SetActive(true);
        StartCoroutine(LoadScene());
    }
    private void OnResourceLoaded()
    {
        _isResourceLoaded = true;
    }
    private void FadeIn(float duration, Action callback)
    {
        Sequence _sequence = DOTween.Sequence();
        _sequence.Append(_fadeImage.DOFade(1f, duration));
        _sequence.AppendCallback(callback.Invoke);
    }
    IEnumerator LoadScene()
    {
        _loadingBarPanel.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync("GameScene");

        op.allowSceneActivation = false;
        float fakeProgress = 0f;
        bool isFading = false;
        while (op.isDone == false)
        {
            // 씬 로딩 진행도 (0 ~ 0.9)
            float realProgress = Mathf.Clamp01(op.progress / 0.9f);
            fakeProgress = Mathf.MoveTowards(fakeProgress, 1f, Time.deltaTime);
            // UI 업데이트
            float finalProgress = Mathf.Min(fakeProgress, realProgress);
            _loadingBar.fillAmount = finalProgress;
            // 100% 되면 씬 전환
            if (finalProgress >= 1f && isFading == false)
            {
                isFading = true;
                FadeIn(1.5f, () => op.allowSceneActivation = true);
            }
            yield return null;
        }
    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnResourceLoaded -= OnResourceLoaded;
        }
    }
}
