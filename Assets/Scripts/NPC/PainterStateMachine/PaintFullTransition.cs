using UnityEngine;

[RequireComponent(typeof(Painter))]
public class PaintFullTransition : Transition
{
    private Painter _painter;

    private void Awake()
    {
        _painter = GetComponent<Painter>();
    }

    private void Update()
    {
        if(_painter.PaintAmount >= _painter.MaxPaintAmount)
        {
            NeedTransit = true;
        }
    }
}
