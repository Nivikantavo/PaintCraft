using Agava.YandexGames;
using System.Collections;
using UnityEngine;

public class PlayersLeaderboard : MonoBehaviour
{
    [SerializeField] private ScoreView _scoreViewPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private int _leaderboardsLenth;

    private const string _leaderboardName = "TotalEarned";

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize();
        InitializePlayerEntries();
    }

    private void FillLeaderboard(LeaderboardEntryResponse entry)
    {
        var view = Instantiate(_scoreViewPrefab, _content);

        view.Initialize(entry.rank, entry.score, entry.player.publicName);
        Debug.Log(entry.player.publicName);
    }

    private void InitializePlayerEntries()
    {
        Leaderboard.GetEntries(_leaderboardName, OnGetEntriesSuccess, null, _leaderboardsLenth);
    }

    private void OnGetEntriesSuccess(LeaderboardGetEntriesResponse result)
    {
        foreach (var entry in result.entries)
        {
            FillLeaderboard(entry);
        }
    }
}
