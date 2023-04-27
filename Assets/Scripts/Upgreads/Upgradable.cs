using GameAnalyticsSDK;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Upgradable : MonoBehaviour
{
    [SerializeField] protected float UpgradeStep;
    [SerializeField] private int _maxLevel;
    [SerializeField] private int _startPrice;
    [SerializeField] private int _priceStep;
    [SerializeField] private float _startParemeter;
    [SerializeField] private string _parameterName;
    [SerializeField] private string _lable;

    protected int CurrentLevel;
    private float _currentParemeter;

    public int Price => _startPrice + CurrentLevel * _priceStep;
    public int MaxLevel => _maxLevel;
    public int Level => CurrentLevel;
    public string ParameterName => _parameterName;
    public string Label => _lable;
    public float StartParameter => _startParemeter;

    protected virtual void Awake()
    {
        CurrentLevel = PlayerPrefs.GetInt(_parameterName + "Level", 0);
        _currentParemeter = PlayerPrefs.GetFloat(_parameterName, _startParemeter);
    }

    public virtual void Upgrade()
    {
        CurrentLevel++;
        _currentParemeter = _startParemeter + (UpgradeStep * CurrentLevel);
        SaveUpgrade(_currentParemeter);
#if (UNITY_WEBGL && !UNITY_EDITOR)
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "gold", Price, "UpgradeStation", _parameterName);
#endif
    }

    private void SaveUpgrade(float parameter)
    {
        PlayerPrefs.SetFloat(_parameterName, parameter);
        PlayerPrefs.SetInt(_parameterName + "Level", CurrentLevel);
        Debug.Log(_parameterName + " / " + PlayerPrefs.GetFloat(_parameterName));
        PlayerPrefs.Save();
    }
}
