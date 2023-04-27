using UnityEngine;
using UnityEngine.Events;

public class Painter : MonoBehaviour, IUpgradable
{
    protected const string LiquidColor = "LiquidColor";

    [SerializeField] protected float _paintCost;
    [SerializeField] protected float _startPaintAmount;
    [SerializeField] protected RayScan _rayScan;
    [SerializeField] protected ParticleSystem _paintParticle;
    [SerializeField] protected Renderer _paintRenderer;
    [SerializeField] protected PlayerWallet Wallet;

    protected float _maxPaintAmount;

    public float PaintAmount { get; protected set; }
    public Color CurrentColor { get; private set; }
    public float MaxPaintAmount => _maxPaintAmount;
    public float PaintCost => _paintCost;
    public PlayerWallet PlayerWallet => Wallet;

    public event UnityAction PaintAmountChanged;

    protected virtual void Awake()
    {
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
        if (PaintAmount >= _paintCost)
        {
            PaintAmount -= _paintCost;
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
        if (PaintAmount >= _paintCost)
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
