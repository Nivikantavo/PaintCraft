using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyCountViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsCountView;
    [SerializeField] private Player _player;
    [SerializeField] private float _addingDelay;
    [SerializeField] private int _maxAddingDelta;

    private Coroutine _addingCoroutine;
    private float _currentValue;

    private void Start()
    {
        _currentValue = _player.PlayerWallet.Money;
        _coinsCountView.text = _currentValue.ToString();
    }

    private void OnEnable()
    {
        _player.PlayerWallet.MoneyCountChanged += OnBalanceChanged;
    }

    private void OnDisable()
    {
        _player.PlayerWallet.MoneyCountChanged -= OnBalanceChanged;
    }

    private void OnBalanceChanged(int money)
    {
        if (_addingCoroutine != null)
        {
            StopCoroutine(_addingCoroutine);
        }

        _addingCoroutine = StartCoroutine(ViewBalance());
    }

    private IEnumerator ViewBalance()
    {
        WaitForSeconds delay = new WaitForSeconds(_addingDelay);

        while(_currentValue != _player.PlayerWallet.Money)
        {
            _currentValue = Mathf.MoveTowards(_currentValue, _player.PlayerWallet.Money, _maxAddingDelta);
            _coinsCountView.text = _currentValue.ToString();
            yield return delay;
        }
    }
}
