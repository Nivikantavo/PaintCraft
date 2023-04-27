using UnityEngine;

[RequireComponent(typeof(Painter))]
public class NeedPaintTransition : Transition
{
    private Painter _painter;

    private void Awake()
    {
        _painter = GetComponent<Painter>();
    }

    private void Update()
    {
        if(_painter.PaintAmount <= _painter.PaintCost)
        {
            NeedTransit = true;
        }
    }
}
