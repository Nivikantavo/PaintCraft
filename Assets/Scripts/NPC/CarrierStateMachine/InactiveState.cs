using System.Collections;
using UnityEngine;

public class InactiveState : MonoBehaviour
{
    public int Paid { get; private set; }
    public int Price => _price;

    [SerializeField] private int _price;
    [SerializeField] private BoxCollider _paymentArea;

    private void OnTriggerEnter(Collider other)
    {
        if(TryGetComponent<Player>(out Player player))
        {
            if(player.PlayerWallet.Money > 0)
            {
                StartCoroutine(ApplyPayment(player));
            }
        }
    }

    private IEnumerator ApplyPayment(Player player)
    {
        for(int i = 0; i <= _price; i++)
        {
            if(player.PlayerWallet.Money > 0)
            {
                player.PlayerWallet.SpendMoney(1);
                Paid++;
                yield return null;
            }
            else
            {
                break;
            }
        }
    }
}
