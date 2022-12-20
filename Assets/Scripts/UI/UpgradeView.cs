using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private Button _upgradeButton;

    private Upgradable _upgradable;

    public event UnityAction<Upgradable> UpgradeButtonClick;

    private void OnEnable()
    {
        _upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
        _upgradeButton.onClick.AddListener(TryLockButton);
    }

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClick);
        _upgradeButton.onClick.RemoveListener(TryLockButton);
    }

    public void Renderer(Upgradable upgradable)
    {
        _upgradable = upgradable;
        _label.text = upgradable.Label;
        _price.text = upgradable.Price.ToString();
        _level.text = upgradable.Level.ToString();

        if (_upgradable.Level == _upgradable.MaxLevel)
        {
            _upgradeButton.interactable = false;
        }
        else
        {
            _upgradeButton.interactable = true;
        }
    }

    private void OnUpgradeButtonClick()
    {
        UpgradeButtonClick?.Invoke(_upgradable);
    }

    private void TryLockButton()
    {
        if(_upgradable.Level == _upgradable.MaxLevel)
        {
            _upgradeButton.interactable = false;
        }
    }
}
