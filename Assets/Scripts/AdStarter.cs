using Agava.YandexGames;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AdStarter : MonoBehaviour
{
    public event UnityAction Reward;
    public event UnityAction AdClose;
    public int RewardMultiplayer => _rewardMultiplayer;
    [SerializeField] private int _rewardMultiplayer;

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
        InterstitialAd.Show();
#endif

    }

    public void ShowVideoAd()
    {
#if (UNITY_WEBGL && !UNITY_EDITOR)
        VideoAd.Show(null, RewardPlayer, OnAdClose, null);
#endif

    }

    private void RewardPlayer()
    {
        Reward?.Invoke();
    }

    private void OnAdClose()
    {
        AdClose?.Invoke();
    }
}