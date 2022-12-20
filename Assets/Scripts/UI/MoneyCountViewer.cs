using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCountViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsCountView;
    [SerializeField] private Player _player;

    private void Awake()
    {
        _coinsCountView.text = _player.PlayerWallet.Money.ToString();
    }
    private void OnEnable()
    {
        _player.PlayerWallet.MoneyCountChanged += OnMoneyCountChanged;
    }

    private void OnDisable()
    {
        _player.PlayerWallet.MoneyCountChanged -= OnMoneyCountChanged;
    }

    private void OnMoneyCountChanged(int money)
    {
        _coinsCountView.text = _player.PlayerWallet.Money.ToString();
    }
}
