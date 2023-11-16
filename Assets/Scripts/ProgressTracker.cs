using GameAnalyticsSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    [SerializeField] private List<GameObject> _rooms;
    [SerializeField] private PlayerWallet _playerWallet;
    [SerializeField] private Boss _boss;
    [SerializeField] private GameObject _endLevelPanel;
    [SerializeField] private AdStarter _adStarter;

    private List<Wall> _walls = new List<Wall>();
    private int _paintedWallsCount;
    private float _delayBeforeEnd = 3f;
    private bool _rewardAdSuccess;

    public int MoneyEarnedPerLevel { get; private set; }

    public event Action AllWallsPainted;
    public event Action LevelEnd;
    public event Action RewardReceived;

    private void Awake()
    {
        List<Wall> walls = new List<Wall>();
        _rewardAdSuccess = false;

        foreach (var wallSet in _rooms)
        {
            wallSet.GetComponentsInChildren<Wall>(false, walls);
            foreach (var wall in walls)
            {
                _walls.Add(wall);
            }
        }
    }

    private void OnEnable()
    {
        MoneyEarnedPerLevel = 0;
        foreach (var wall in _walls)
        {
            wall.WallPainted += OnWallPainted;
        }

        _playerWallet.MoneyCountChanged += OnPlayerMoneyChanged;
        _boss.Paying += OnLevelEnd;
        _adStarter.Reward += OnRewardVideoSuccess;
        _adStarter.AdClose += OnRewardVideoClose;
    }

    private void OnDisable()
    {
        foreach (var wall in _walls)
        {
            wall.WallPainted -= OnWallPainted;
        }

        _playerWallet.MoneyCountChanged -= OnPlayerMoneyChanged;
        _boss.Paying -= OnLevelEnd;
        _adStarter.Reward -= OnRewardVideoSuccess;
        _adStarter.AdClose -= OnRewardVideoClose;
    }

    private void OnRewardVideoSuccess()
    {
        _rewardAdSuccess = true;
    }

    private void OnRewardVideoClose()
    {
        if (_rewardAdSuccess)
        {
            int rewarded = MoneyEarnedPerLevel * _adStarter.RewardMultiplayer - MoneyEarnedPerLevel;
            _playerWallet.AddMoney(rewarded);
            RewardReceived?.Invoke();

            _endLevelPanel.GetComponent<EndLevelPanel>().ViewReward();
        }
    }

    private void OnWallPainted(Wall wall)
    {
        _paintedWallsCount++;

        if (_walls.Count == _paintedWallsCount)
        {
            AllWallsPainted?.Invoke();
        }
    }

    private void OnPlayerMoneyChanged(int money)
    {
        MoneyEarnedPerLevel += money;
    }

    private void OnLevelEnd()
    {
        StartCoroutine(DelayBeforeEnd());
        LevelEnd?.Invoke();
        #if (UNITY_WEBGL && !UNITY_EDITOR)
        SetGoldInfo();
        #endif
    }

    private IEnumerator DelayBeforeEnd()
    {
        WaitForSeconds delayTime = new WaitForSeconds(_delayBeforeEnd);
        yield return delayTime;
        _endLevelPanel.SetActive(true);
    }

    private void SetGoldInfo()
    {
        int earnedByWalls = MoneyEarnedPerLevel - _boss.LevelRevard;
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "gold", earnedByWalls, "earned by walls", "walls rewards");
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "gold", _boss.LevelRevard, "earned by level complited", "level reward");
    }
}
