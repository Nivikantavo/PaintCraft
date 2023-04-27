using System.Collections.Generic;
using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    [SerializeField] private List<Upgradable> _upgradebles;
    [SerializeField] private UpgradePanel _upgradPanel;
    [SerializeField] private string _label;
    [SerializeField] private Sprite _image;

    private Player _player;

    public IReadOnlyList<Upgradable> Upgradables => _upgradebles;
    public string Label => _label;
    public Sprite Image => _image;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            _player = player;
            _upgradPanel.Initialize(this);
            _upgradPanel.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            _upgradPanel.gameObject.SetActive(false);
        }
    }

    public void TryUpgrade(Upgradable upgradable)
    {
        if (_player != null)
        {
            if (_player.PlayerWallet.Money >= upgradable.Price && upgradable.Level < upgradable.MaxLevel)
            {
                _player.PlayerWallet.SpendMoney(upgradable.Price);
                upgradable.Upgrade();
                _upgradPanel.Render();
            }
        }
    }
}
