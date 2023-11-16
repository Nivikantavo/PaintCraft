using System.Collections;
using GameAnalyticsSDK;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BuyArea : MonoBehaviour
{
    [SerializeField] private GameObject _purchasedObject;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private TMP_Text _paidText;
    [SerializeField] private ParticleSystem _paidParticles;
    [SerializeField] private int _price;
    [SerializeField] private string _areaName;

    private int _paid;
    private int _payStep = 100;
    private float _payDelay = 0.01f;
    private Vector3 _particlesOffset = new(0, 1);
    private Coroutine _paidCorutine;

    private void Awake()
    {
        _paid = PlayerPrefs.GetInt(_areaName, 0);
        if(_paid >= _price)
        {
            UnlockObject(false);
        }
        else
        {
            _purchasedObject.SetActive(false);
            _paidText.text = (_price - _paid).ToString();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player player))
        {
            if(_paidCorutine == null && _paid != _price)
            {
                _paidCorutine = StartCoroutine(PaidCorutine(player.PlayerWallet));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            if(_paidCorutine != null)
            {
                StopCoroutine(_paidCorutine);
                _paidParticles.Stop();
                _paidCorutine = null;
            }

            PlayerPrefs.SetInt(_areaName, _paid);
            #if (UNITY_WEBGL && !UNITY_EDITOR)
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, "gold", _paid, "buyArea", "Open " + _purchasedObject.name);
            #endif
        }
    }

    private IEnumerator PaidCorutine(PlayerWallet playerWallet)
    {
        WaitForSeconds delay = new(_payDelay);
        int contribution = _price / _payStep;

        while (_price > _paid)
        {
            CalculateContribution(ref contribution, playerWallet.Money);

            if (playerWallet.SpendMoney(contribution))
            {
                _paid += contribution;
                PayDisplaying(playerWallet.transform);
                yield return delay;
            }
            else
            {
                break;
            }
        }

        if (_paid >= _price)
        {
            UnlockObject(true);
        }
        _paidParticles.Stop();
    }

    protected virtual void UnlockObject(bool firstUnlock)
    {
        _purchasedObject.SetActive(true);
        if (firstUnlock)
        {
            PlayerPrefs.SetInt(_areaName, _paid);
        }
        gameObject.SetActive(false);
    }

    private void PayDisplaying(Transform payer)
    {
        if (_paidParticles.isPlaying == false)
        {
            _paidParticles.Play();
        }

        _paidParticles.transform.position = payer.position + _particlesOffset;
        string paidText = (_price - _paid).ToString();
        _paidText.text = paidText;
    }

    private void CalculateContribution(ref int contribution, int playerMoney)
    {
        if (_price - _paid < contribution)
        {
            contribution = _price - _paid;
        }
        else if (playerMoney < contribution && playerMoney > 0)
        {
            contribution = playerMoney;
        }
    }
}
