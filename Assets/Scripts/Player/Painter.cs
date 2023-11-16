using System;
using UnityEngine;

public class Painter : MonoBehaviour, IUpgradable
{
    protected const string LiquidColor = "LiquidColor";

    [SerializeField] protected float PaintConsumption;
    [SerializeField] protected float StartPaintAmount;
    [SerializeField] protected RayScan RayScan;
    [SerializeField] protected ParticleSystem PaintParticle;
    [SerializeField] protected Renderer PaintRenderer;
    [SerializeField] protected PlayerWallet Wallet;

    protected float _maxPaintAmount;

    public float PaintAmount { get; protected set; }
    public Color CurrentColor { get; private set; }
    public float MaxPaintAmount => _maxPaintAmount;
    public float PaintCost => PaintConsumption;
    public PlayerWallet PlayerWallet => Wallet;

    public event Action PaintAmountChanged;

    protected virtual void Awake()
    {
        CurrentColor = PaintRenderer.material.color;
        SetUpgradeParams();
    }

    protected virtual void OnEnable()
    {
        RayScan.WallSelected += TryPainting;
    }

    protected virtual void OnDisable()
    {
        RayScan.WallSelected -= TryPainting;
    }

    public bool TryTakePaint(float fullnessProcentage, Color color)
    {
        float fillMultiplier = 0.5f;

        if (PaintAmount < _maxPaintAmount || color != CurrentColor)
        {
            PaintAmount += _maxPaintAmount * fillMultiplier * fullnessProcentage;

            PaintAmount = Mathf.Clamp(PaintAmount, 0, _maxPaintAmount);

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
        if (PaintAmount >= PaintConsumption)
        {
            PaintAmount -= PaintConsumption;
            PaintAmountChanged?.Invoke();

            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void TryPainting(Wall wall)
    {
        if (PaintAmount >= PaintConsumption)
        {
            wall.Select(this);
        }
    }

    protected void ChangeColor(Color color)
    {
        CurrentColor = color;
        PaintRenderer.material.color = CurrentColor;

        ParticleSystem.MainModule mainModule = PaintParticle.main;
        mainModule.startColor = CurrentColor;
    }

    public virtual void SetUpgradeParams(){}
}
