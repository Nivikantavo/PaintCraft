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
            UnlockObject();
            gameObject.SetActive(false);
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
            StopCoroutine(_paidCorutine);
            _paidParticles.Stop();
            _paidCorutine = null;

            PlayerPrefs.SetInt(_areaName, _paid);
        }
    }

    private IEnumerator PaidCorutine(Player player)
    {
        ParticleSystem.VelocityOverLifetimeModule particlesVelocity = _paidParticles.velocityOverLifetime;
        Vector3 vector3 = new Vector3(0, 1);


        while (_price > _paid)
        {
            player.PlayerWallet.SpendMoney(1);
            _paidParticles.Play();
            _paidParticles.transform.position = player.transform.position + vector3;
            _paid += 1;
            _paidText.text = (_price - _paid).ToString();
            yield return null;
        }

        if (_paid == _price)
        {
            _paidParticles.Stop();
            UnlockObject();
        }
    }

    private void UnlockObject()
    {
        _purchasedObject.SetActive(true);
        _sprite.enabled = false;
        _paidText.enabled = false;
    }
}
