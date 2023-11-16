using GameAnalyticsSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelPanel : MonoBehaviour
{
    private const string XSign = "X";

    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private ProgressTracker _progressTracker;
    [SerializeField] private Button _nextLevel;
    [SerializeField] private Button _hub;
    [SerializeField] private Button _reward;
    [SerializeField] private TMP_Text _moneyEarned;
    [SerializeField] private TMP_Text _rewardMultiplayer;
    [SerializeField] private GameObject _joystick;
    [SerializeField] private AdStarter _adStarter;

    private bool _rewardButtonClicked = false;

    private void OnEnable()
    {
        string rewardMultiplayerText = XSign + _adStarter.RewardMultiplayer.ToString();

        _moneyEarned.text = _progressTracker.MoneyEarnedPerLevel.ToString();
        _rewardMultiplayer.text = rewardMultiplayerText;
        _joystick.SetActive(false);

        _hub.onClick.AddListener(_levelLoader.LoadHub);
        _nextLevel.onClick.AddListener(_levelLoader.LoadNextLevel);
        _reward.onClick.AddListener(OnRewardButtonClick);
    }

    private void OnDisable()
    {
        if(_rewardButtonClicked == false)
        {
            _adStarter.ShowInterstitialAd();
        }

        _joystick.SetActive(true);

        _hub.onClick.RemoveListener(_levelLoader.LoadHub);
        _nextLevel.onClick.RemoveListener(_levelLoader.LoadNextLevel);
        _reward.onClick.RemoveListener(OnRewardButtonClick);
    }

    public void ViewReward()
    {
        _reward.interactable = false;
        _moneyEarned.text = _progressTracker.MoneyEarnedPerLevel.ToString();
    }

    private void OnRewardButtonClick()
    {
        GameAnalytics.NewDesignEvent("rewardtype-ad-click");
        _adStarter.ShowVideoAd();
        _rewardButtonClicked = true;
    }
}
