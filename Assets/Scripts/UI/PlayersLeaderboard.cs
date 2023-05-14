using Agava.YandexGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersLeaderboard : MonoBehaviour
{
    private const string LeaderboardName = "TotalEarned";

    [SerializeField] private ScoreView _scoreViewPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private int _leaderboardsLenth;
    [SerializeField] private GameObject _leaderboard;

    private bool _leaderboardInitialized = false;

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize();
    }

    public void OpenLeaderboard()
    {
        if (_leaderboardInitialized)
        {
            _leaderboard.gameObject.SetActive(true);
        }
        else
        {
            CheckAuthorized();
        }
    }

    private void CheckAuthorized()
    {
        if (PlayerAccount.IsAuthorized == false)
        {
            PlayerAccount.Authorize(InitializePlayerEntries, null);
        }
        else
        {
            InitializePlayerEntries();
        }
    }

    private void InitializePlayerEntries()
    {
        _leaderboard.gameObject.SetActive(true);
        Leaderboard.GetEntries(LeaderboardName, OnGetEntriesSuccess, null, _leaderboardsLenth);
    }

    private void OnGetEntriesSuccess(LeaderboardGetEntriesResponse result)
    {
        foreach (var entry in result.entries)
        {
            FillLeaderboard(entry);
        }
        _leaderboardInitialized = true;
    }

    private void FillLeaderboard(LeaderboardEntryResponse entry)
    {
        var view = Instantiate(_scoreViewPrefab, _content);
        view.Initialize(entry.rank, entry.score, entry.player.publicName);
    }
}
