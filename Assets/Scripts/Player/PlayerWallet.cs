using System;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    private const string MoneyCount = "MoneyCount";

    public int Money { get; private set; }

    public event Action<int> MoneyCountChanged;

    private void Awake()
    {
        Money = PlayerPrefs.GetInt(MoneyCount);
    }

    private void OnEnable()
    {
        MoneyCountChanged?.Invoke(Money);
    }

    private void OnDisable()
    {
        Save();
    }

    public void AddMoney(int money)
    {
        if (money > 0)
        {
            Money += money;
            MoneyCountChanged?.Invoke(money);
        }
        Save();
    }

    public bool SpendMoney(int price)
    {
        if(Money >= price)
        {
            Money -= price;
            MoneyCountChanged?.Invoke(-price);
            Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Save()
    {
        PlayerPrefs.SetInt(MoneyCount, Money);
        PlayerPrefs.Save();
    }
}
