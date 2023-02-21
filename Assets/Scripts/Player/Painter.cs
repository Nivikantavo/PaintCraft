using UnityEngine;
using UnityEngine.Events;

public class Painter : MonoBehaviour, IUpgradable
{
    public float PaintAmount { get; private set; }
    public Color CurrentColor { get; private set; }
    public float MaxPaintAmount => _maxPaintAmount;
    public float Paint—onsumption => _paint—onsumption;
    public PlayerWallet PlayerWallet => Wallet;

    [SerializeField] protected float _paint—onsumption;
    [SerializeField] protected float _startPaintAmount;
    [SerializeField] protected RayScan _rayScan;
    [SerializeField] protected ParticleSystem _paintParticle;
    [SerializeField] protected Renderer _paintRenderer;
    [SerializeField] protected PlayerWallet Wallet;

    protected float _maxPaintAmount;

    protected const string LiquidColor = "LiquidColor";

    public event UnityAction PaintAmountChanged;

    public bool TryTakePaint(float fullnessProcentage, Color color)
    {
        float fillMultiplier = 0.33f;

        if (PaintAmount < _maxPaintAmount || color != CurrentColor)
        {
            PaintAmount += _maxPaintAmount * fillMultiplier * fullnessProcentage;

            Mathf.Clamp(PaintAmount, 0, _maxPaintAmount);

            if (color != CurrentColor)
            {
                ChangeColor(color);
            }
            PaintAmountChanged?.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TrySpendPaint()
    {
        if (PaintAmount >= _paint—onsumption)
        {
            PaintAmount -= _paint—onsumption;
            PaintAmountChanged?.Invoke();

            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void Awake()
    {
        PaintAmount = _startPaintAmount;
        CurrentColor = _paintRenderer.material.color;
        SetUpgrades();
    }

    protected virtual void OnEnable()
    {
        _rayScan.WallSelected += TryPainting;    
    }

    protected virtual void OnDisable()
    {
        _rayScan.WallSelected -= TryPainting;
    }

    protected virtual void TryPainting(Wall wall)
    {
        if (PaintAmount >= _paint—onsumption)
        {
            wall.Select(this);
        }
    }

    protected void ChangeColor(Color color)
    {
        CurrentColor = color;
        _paintRenderer.material.color = CurrentColor;

        ParticleSystem.MainModule mainModule = _paintParticle.main;
        mainModule.startColor = CurrentColor;
    }

    public virtual void SetUpgrades(){}
}
