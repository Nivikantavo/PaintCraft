using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProgressTracker : MonoBehaviour
{
    public int MoneyEarnedPerLevel { get; private set; }

    [SerializeField] private List<GameObject> _rooms;
    [SerializeField] private PlayerWallet _playerWallet;
    [SerializeField] private Boss _boss;
    [SerializeField] private GameObject _endLevelPanel;

    private List<Wall> _walls = new List<Wall>();
    private int _paintedWallsCount;
    private float _delayBeforeEnd = 3f;

    public event UnityAction AllWallsPainted;
    public event UnityAction LevelEnd;

    private void Awake()
    {
        

        List<Wall> walls = new List<Wall>();

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
        _boss.PlayerAttained += OnLevelEnd;
    }

    private void OnDisable()
    {
        foreach (var wall in _walls)
        {
            wall.WallPainted -= OnWallPainted;
        }

        _playerWallet.MoneyCountChanged -= OnPlayerMoneyChanged;
        _boss.PlayerAttained -= OnLevelEnd;
    }

    private void OnWallPainted()
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
        Debug.Log("OnLevelEnd");
    }

    private IEnumerator DelayBeforeEnd()
    {
        WaitForSeconds delayTime = new WaitForSeconds(_delayBeforeEnd);

        yield return delayTime;

        _endLevelPanel.SetActive(true);
    }

}
