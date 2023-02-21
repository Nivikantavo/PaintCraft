using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeView : MonoBehaviour
{
    [SerializeField] private LeanLocalizedTextMeshProUGUI _description;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _currentLevel;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private GameObject _maxLevel;
    [SerializeField] private GameObject _priceGroup;
    [SerializeField] private GameObject _levelGroup;

    private Upgradable _upgradable;

    public event UnityAction<Upgradable> UpgradeButtonClick;


    private void OnEnable()
    {
        TryLockButton();
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
        _description.TranslationName = upgradable.Label;
        _price.text = upgradable.Price.ToString();
        _currentLevel.text = upgradable.Level.ToString();

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
            _maxLevel.SetActive(true);
            _priceGroup.SetActive(false);
            _levelGroup.SetActive(false);
        }
    }
}
