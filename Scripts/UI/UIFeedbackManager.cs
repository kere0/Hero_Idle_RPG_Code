using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIFeedbackManager : MonoBehaviour
{
    [SerializeField] private Transform _diamondTarget;
    [SerializeField] private Transform _goldTarget;
    
    [SerializeField] private GameObject _missionRedDot;
    [SerializeField] private GameObject _dailyMissionRedDot;
    [SerializeField] private GameObject _achievementRedDot;

    public void Play(Transform start)
    {
        float radius = 100f;
        for (int i = 0; i < 15; i++)
        {
            GameObject diamond  = Managers.Resource.Instantiate("Diamond", transform, true);
            diamond .transform.position = start.position;
            Vector2 offset = Random.insideUnitCircle * radius;
            Vector3 randomPos = start.position + (Vector3)offset;
            Sequence s = DOTween.Sequence();
            s.Append(diamond.transform.DOMove(randomPos,0.35f));
            s.Append(diamond.transform.DOMove(_diamondTarget.position, 1f).OnComplete(() =>
            {
                Managers.Pool.ObjPush(diamond);
            }));
        }
    }
    // 미션
    public void SetMissionRedDot(bool active)
    {
        _missionRedDot.SetActive(active);
    }
    public void SetDailyMissionRedDot(bool active)
    {
        _dailyMissionRedDot.SetActive(active);
    }public void SetAchievementRedDot(bool active)
    {
        _achievementRedDot.SetActive(active);
    }
}
