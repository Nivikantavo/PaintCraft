using UnityEngine;
using UnityEngine.Events;

public class PlayerWallet : MonoBehaviour
{
    public int Money { get; private set; }

    public event UnityAction<int> MoneyCountChanged;

    private const string _moneyCount = "MoneyCount";

    public void AddMoney(int money)
    {
        if (money > 0)
        {
            Money += money;
            MoneyCountChanged?.Invoke(money);
        }
    }

    public bool SpendMoney(int price)
    {
        if(Money >= price)
        {
            Money -= price;
            MoneyCountChanged?.Invoke(-price);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnEnable()
    {
        Money = PlayerPrefs.GetInt(_moneyCount);
        MoneyCountChanged?.Invoke(Money);
        AddMoney(10000);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(_moneyCount, Money);
    }
}
