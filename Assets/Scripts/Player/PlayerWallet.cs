using UnityEngine;
using UnityEngine.Events;

public class PlayerWallet : MonoBehaviour
{
    public int Money { get; private set; }

    public event UnityAction<int> MoneyCountChanged;

    public void AddMoney(int money)
    {
        if (money > 0)
        {
            Money += money;
            MoneyCountChanged?.Invoke(money);
        }
    }

    public void SpendMoney(int price)
    {
        if(Money >= price)
        {
            Money -= price;
            MoneyCountChanged?.Invoke(-price);
        }
        else
        {
            throw new System.Exception("not enough money");
        }
    }
}
