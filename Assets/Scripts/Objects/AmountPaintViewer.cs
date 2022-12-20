using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Painter))]
public class AmountPaintViewer : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _minFillingLevel;
    [SerializeField] private float _maxFillingLevel;

    private Painter _painter;
    private const string _fill = "Fill";

    private void Awake()
    {
        _painter = gameObject.GetComponent<Painter>();
        OnPaintAmountChanged();
    }

    private void OnEnable()
    {
        _painter.PaintAmountChanged += OnPaintAmountChanged;
    }

    private void OnDisable()
    {
        _painter.PaintAmountChanged -= OnPaintAmountChanged;
    }

    private void OnPaintAmountChanged()
    {
        float temp = Mathf.InverseLerp(0, _painter.MaxPaintAmount, _painter.PaintAmount);
        float filling = Mathf.Lerp(_minFillingLevel, _maxFillingLevel, temp);

        _renderer.material.SetFloat(_fill, filling);
    }
}
