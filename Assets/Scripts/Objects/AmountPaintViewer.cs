using UnityEngine;

[RequireComponent(typeof(Painter))]
public class AmountPaintViewer : MonoBehaviour
{
    [SerializeField] private GameObject _paint;

    private Painter _painter;

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
        float filling = 0;

        if (_painter.PaintAmount >= _painter.Paint—onsumption)
        {
            filling = Mathf.InverseLerp(0, _painter.MaxPaintAmount, _painter.PaintAmount);
            Mathf.Clamp(filling, 0, 1);
        }

        _paint.transform.localScale = new Vector3(1, filling, 1);
    }
}
