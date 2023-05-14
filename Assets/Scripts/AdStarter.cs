using Agava.YandexGames;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class AdStarter : MonoBehaviour
{
    [SerializeField] private int _rewardMultiplayer;

    public int RewardMultiplayer => _rewardMultiplayer;

    public event UnityAction Reward;
    public event UnityAction AdClose;

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
//#if (UNITY_WEBGL && !UNITY_EDITOR)
        InterstitialAd.Show(OnAdOpen, OnInterstitialAdClose);
//#endif
    }

    public void ShowVideoAd()
    {
//#if (UNITY_WEBGL && !UNITY_EDITOR)
        VideoAd.Show(OnAdOpen, RewardPlayer, OnRewardAdClose, null);
//#endif
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

        AudioListener.pause = false;
        AudioListener.volume = 1f;
    }

    private void OnInterstitialAdClose(bool wasShown)
    {
        AudioListener.pause = false;
        AudioListener.volume = 1f;
    }
}
