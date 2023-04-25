using UnityEngine;
using UnityEngine.Events;

public class PlayerWallet : MonoBehaviour
{
    public int Money { get; private set; }

    public event UnityAction<int> MoneyCountChanged;

    private const string _moneyCount = "MoneyCount";

    private void Awake()
    {
        Money = PlayerPrefs.GetInt(_moneyCount);
        AddMoney(10000);
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

    private void OnEnable()
    {
        MoneyCountChanged?.Invoke(Money);
    }

    private void OnDisable()
    {
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetInt(_moneyCount, Money);
        PlayerPrefs.Save();
    }
}
