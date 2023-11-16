using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;

public class AdStarter : MonoBehaviour
{
    [SerializeField] private int _rewardMultiplayer;

    public int RewardMultiplayer => _rewardMultiplayer;

    public event Action Reward;
    public event Action AdClose;

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        #if (!UNITY_WEBGL || UNITY_EDITOR)
        yield break;
        #endif
        yield return YandexGamesSdk.Initialize();
    }

    public void ShowInterstitialAd()
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        InterstitialAd.Show(OnAdOpen, OnAdClose);
#endif
    }

    public void ShowVideoAd()
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        VideoAd.Show(OnAdOpen, RewardPlayer, OnRewardAdClose, null);
#endif
    }

    private void RewardPlayer()
    {
        Reward?.Invoke();
    }

    private void OnAdOpen()
    {
        AudioListener.pause = true;
        AudioListener.volume = 0f;
    }

    private void OnRewardAdClose()
    {
        AdClose?.Invoke();
        OnAdClose();
    }

    private void OnAdClose(bool wasShown = true)
    {
        AudioListener.pause = false;
        AudioListener.volume = 1f;
    }
}
