using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    private RewardedAd rewardedAd;
    private string rewardedAdId = "ca-app-pub-3940256099942544/5224354917";

    public bool IsRewardReady { get; private set; }

    public event Action OnRewardReadyChanged;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        MobileAds.Initialize(_ => LoadRewardedAd());
    }
    private void LoadRewardedAd()
    {
        RewardedAd.Load(rewardedAdId, new AdRequest(), (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogWarning("리워드 광고 로드 실패");
                IsRewardReady = false;
                OnRewardReadyChanged?.Invoke();
                return;
            }

            rewardedAd = ad;
            IsRewardReady = true;
            OnRewardReadyChanged?.Invoke();
        });
    }
    public void ShowRewardAd(Action onSuccess)
    {
        if (IsRewardReady == false || rewardedAd == null)
        {
            Debug.Log("광고 준비 안됨 로드시작");
            LoadRewardedAd();
            return;
        }
        SoundManager.Instance.PauseBGM();
        FadeManager.Instance.FadeIn(0f, FadeType.Boss);
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            // 광고가 닫혔을 때
            SoundManager.Instance.ResumeBGM();
            FadeManager.Instance.FadeOut(0f);

            IsRewardReady = false;
            OnRewardReadyChanged?.Invoke();
            LoadRewardedAd();
        };
       rewardedAd.Show(reward => 
       {
            onSuccess?.Invoke();
       }); 
    }
}