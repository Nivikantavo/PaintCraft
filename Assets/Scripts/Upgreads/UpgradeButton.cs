using TMPro;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _price;

    public void Rerenderer(Upgradable upgradable)
    {
        _level.text = upgradable.Level.ToString();
        _price.text = upgradable.Price.ToString();
    }
}
