using Lean.Localization;
using System;
using TMPro;
using UnityEngine;
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

    public event Action<Upgradable> UpgradeButtonClick;

    private void OnEnable()
    {
        TryLockButton();
        _upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
    }

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClick);
    }

    public void Renderer(Upgradable upgradable)
    {
        _upgradable = upgradable;
        _description.TranslationName = upgradable.Label;
        _price.text = upgradable.Price.ToString();
        _currentLevel.text = upgradable.Level.ToString();

        TryLockButton();
    }

    private void OnUpgradeButtonClick()
    {
        UpgradeButtonClick?.Invoke(_upgradable);
        TryLockButton();
    }

    private void TryLockButton()
    {
        bool needLock = _upgradable.Level == _upgradable.MaxLevel;

        _upgradeButton.interactable = !needLock;
            
        _maxLevel.SetActive(needLock);
        _description.gameObject.SetActive(!needLock);
        _priceGroup.SetActive(!needLock);
        _levelGroup.SetActive(!needLock);
    }
}
