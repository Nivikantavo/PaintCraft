using UnityEngine;

[RequireComponent(typeof(Painter))]
public class AmountPaintViewer : MonoBehaviour
{
    [SerializeField] private GameObject _paint;

    private Painter _painter;

    private float _maxScale = 1;
    private float _minScale = 0;

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
        float filling = _minScale;

        if (_painter.PaintAmount >= _painter.PaintCost)
        {
            filling = Mathf.InverseLerp(_minScale, _painter.MaxPaintAmount, _painter.PaintAmount);
            filling = Mathf.Clamp(filling, _minScale, _maxScale);
        }

        _paint.transform.localScale = new Vector3(_maxScale, filling, _maxScale);
    }
}
