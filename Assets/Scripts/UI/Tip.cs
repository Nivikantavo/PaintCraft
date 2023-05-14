using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    [SerializeField] private List<GameObject> _tips;
    [SerializeField] private GameObject _tipPanel;
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.PaintAmountChanged += OnPaintAmountChanged;
    }

    private void OnPaintAmountChanged()
    {
        _tipPanel.SetActive(true);
        _tips[0].gameObject.SetActive(false);
        _tips[1].gameObject.SetActive(true);
        _player.PaintAmountChanged -= OnPaintAmountChanged;
    }
}
