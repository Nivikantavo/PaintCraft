using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider))]
public class Upgradable : MonoBehaviour
{
    public int Price => _startPrice + CurrentLevel * _priceStep;
    public int MaxLevel => _maxLevel;
    public int Level => CurrentLevel;
    public string ParameterName => _parameterName;
    public string Label => _lable;
    public float StartParameter => _startParemeter;
    
    [SerializeField] protected float UpgradeStep;
    [SerializeField] private int _maxLevel;
    [SerializeField] private int _startPrice;
    [SerializeField] private int _priceStep;
    [SerializeField] private float _startParemeter;
    [SerializeField] private string _parameterName;
    [SerializeField] private string _lable;

    protected int CurrentLevel;
    private float _currentParemeter;

    protected virtual void Awake()
    {
        CurrentLevel = PlayerPrefs.GetInt(_parameterName + "Level", 0);
        _currentParemeter = PlayerPrefs.GetFloat(_parameterName, 0);
    }

    public virtual void Upgrade()
    {
        CurrentLevel++;
        _currentParemeter = _startParemeter + (UpgradeStep * CurrentLevel);
        SaveUpgrade(_currentParemeter);
    }

    private void SaveUpgrade(float parameter)
    {
        PlayerPrefs.SetFloat(_parameterName, parameter);
        PlayerPrefs.SetInt(_parameterName + "Level", CurrentLevel);
    }
}
