using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideQuestPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private Button _button;
    [SerializeField] private Image _completeImage;
    private GuideQuestSystem _guideQuestSystem;
    private Tween _completeTween;
    private void Awake()
    {
        GameManager.Instance.OnGameStart += Init;
        _button.onClick.AddListener(CompleteButtonClick);
    }
    private void Start()
    {
        Managers.PlayerManager.OnGuideQuestValueChanged += Refresh;
    }
    private void Init()
    {
        _guideQuestSystem = Managers.PlayerManager.GuideQuestSystem;
        Refresh( _guideQuestSystem.GetGuideQuestInfo().GuideQuestType);
    }
    private void Refresh(GuideQuestType guideQuestType)
    {
        GuideQuestUIInfo info = _guideQuestSystem.GetGuideQuestInfo();
        if (info.GuideQuestType != guideQuestType) return;
        _descriptionText.text = info.Description;
        _progressText.text = info.Progress;
        _rewardText.text = $"보상 :      {info.Reward}개";

        _button.interactable = info.Completed;
        _completeImage.gameObject.SetActive(info.Completed);
        if (info.Completed == true)
        {
            if (_completeTween != null) return;
            _completeImage.gameObject.SetActive(true);
            Color color = _completeImage.color;
            color.a = 0f;
            _completeImage.color = color;
            _completeTween = _completeImage.DOFade(0.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            if (_completeTween != null)
            {
                _completeTween.Kill();
                _completeTween = null;
                _completeImage.gameObject.SetActive(false);
            }
        }
    }
    private void CompleteButtonClick()
    {
        GameContainer.Instance.UIFeedbackManager.Play(_completeImage.transform);
        _guideQuestSystem.CompleteQuest();
        Refresh( _guideQuestSystem.GetGuideQuestInfo().GuideQuestType);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= Init;
        Managers.PlayerManager.OnGuideQuestValueChanged -= Refresh;
    }
}
