using System.Collections;
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
                _paidCorutine = StartCoroutine(PaidCorutine(player));
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
        }
    }

    private IEnumerator PaidCorutine(Player player)
    {
        WaitForSeconds delay = new(0.01f);
        Vector3 particlesOffset = new(0, 1);
        int contribution = _price / 100;

        while (_price > _paid)
        {
            if(_price - _paid < contribution)
            {
                contribution = _price - _paid;
            }
            else if(player.PlayerWallet.Money < contribution && player.PlayerWallet.Money > 0)
            {
                contribution = player.PlayerWallet.Money;
            }

            if (player.PlayerWallet.SpendMoney(contribution))
            {
                if(_paidParticles.isPlaying == false)
                {
                    _paidParticles.Play();
                }
                
                _paidParticles.transform.position = player.transform.position + particlesOffset;
                _paid += contribution;
                _paidText.text = (_price - _paid).ToString();
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
}
