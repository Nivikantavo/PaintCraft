using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private List<UpgradeView> _upgradeView;

    [SerializeField] private TMP_Text _label;
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
            _label.text = upgradeStation.Label;
            _image.sprite = upgradeStation.Image;

            Render();
        }
        else
        {
            throw new NullReferenceException();
        }
    }

    private void OnUpgradeButtonClick(Upgradable upgradable)
    {
        _upgradeStation.TryUpgrade(upgradable);
    }
}
