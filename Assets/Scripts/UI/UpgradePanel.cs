using Lean.Localization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private List<UpgradeView> _upgradeView;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _label;
    [SerializeField] private Image _image;

    private UpgradeStation _upgradeStation;

    private void OnEnable()
    {
        foreach(UpgradeView upgradeView in _upgradeView)
        {
            upgradeView.UpgradeButtonClick += OnUpgradeButtonClick;
        }
    }

    private void OnDisable()
    {
        foreach (UpgradeView upgradeView in _upgradeView)
        {
            upgradeView.UpgradeButtonClick -= OnUpgradeButtonClick;
        }
    }

    public void Render()
    {
        for (int i = 0; i < _upgradeView.Count; i++)
        {
            _upgradeView[i].Renderer(_upgradeStation.Upgradables[i]);
        }
    }

    public void Initialize(UpgradeStation upgradeStation)
    {
        if(upgradeStation != null)
        {
            _upgradeStation = upgradeStation;
            _label.TranslationName = upgradeStation.Label;
            _image.sprite = upgradeStation.Image;

            Render();
        }
    }

    private void OnUpgradeButtonClick(Upgradable upgradable)
    {
        _upgradeStation.TryUpgrade(upgradable);
    }
}
