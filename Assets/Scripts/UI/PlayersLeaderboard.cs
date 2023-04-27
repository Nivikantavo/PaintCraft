using Agava.YandexGames;
using System.Collections;
using UnityEngine;

public class PlayersLeaderboard : MonoBehaviour
{
    private const string LeaderboardName = "TotalEarned";

    [SerializeField] private ScoreView _scoreViewPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private int _leaderboardsLenth;

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize();

        if (PlayerAccount.IsAuthorized == false)
        {
            PlayerAccount.Authorize(InitializePlayerEntries, null);
        }
        else
        {
            InitializePlayerEntries();
        }
    }

    private void FillLeaderboard(LeaderboardEntryResponse entry)
    {
        var view = Instantiate(_scoreViewPrefab, _content);

        view.Initialize(entry.rank, entry.score, entry.player.publicName);
    }

    private void InitializePlayerEntries()
    {
        Leaderboard.GetEntries(LeaderboardName, OnGetEntriesSuccess, null, _leaderboardsLenth);
    }

    private void OnGetEntriesSuccess(LeaderboardGetEntriesResponse result)
    {
        foreach (var entry in result.entries)
        {
            FillLeaderboard(entry);
        }
    }
}
