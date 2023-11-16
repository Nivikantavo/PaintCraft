using Agava.YandexGames;
using System.Collections;
using UnityEngine;

public class RecordHandler : MonoBehaviour
{
    private const string TotalEarned = "TotalEarned";

    [SerializeField] private PlayerWallet _wallet;
    [SerializeField] private ProgressTracker _progressTracker;

    private LeaderboardEntryResponse _playerEntry = null;
    private int _currentScore;

#if (UNITY_WEBGL && !UNITY_EDITOR)
    private IEnumerator Start()
    {
        _currentScore = 0;

        if (YandexGamesSdk.IsInitialized == false)
        {
            yield return YandexGamesSdk.Initialize();
        }
        if (PlayerAccount.IsAuthorized)
        {
            Leaderboard.GetPlayerEntry(TotalEarned, OnGetPlayerEntrySuccess);
        }
    }

    private void OnEnable()
    {
        _wallet.MoneyCountChanged += OnMoneyCountChange;
        _progressTracker.LevelEnd += OnLevelEnd;
        _progressTracker.RewardReceived += OnLevelEnd;
    }

    private void OnDisable()
    {
        _wallet.MoneyCountChanged -= OnMoneyCountChange;
        _progressTracker.LevelEnd -= OnLevelEnd;
        _progressTracker.RewardReceived -= OnLevelEnd;
    }

    private void OnLevelEnd()
    {
        if (_playerEntry.score < _currentScore)
        {
            SetNewRecord();
        }
    }

    private void OnMoneyCountChange(int difference)
    {
        if(difference > 0)
        {
            _currentScore += difference;
        }
    }

    private void OnGetPlayerEntrySuccess(LeaderboardEntryResponse playerEntry)
    {
        if(playerEntry == null)
        {
            Leaderboard.SetScore(TotalEarned, _currentScore);
        }
        else
        {
            _playerEntry = playerEntry;
        }
        _currentScore = _playerEntry.score;
    }

    private void SetNewRecord()
    {
        Leaderboard.SetScore(TotalEarned, _currentScore);
        Leaderboard.GetPlayerEntry(TotalEarned, OnGetPlayerEntrySuccess);
    }
#endif
}
