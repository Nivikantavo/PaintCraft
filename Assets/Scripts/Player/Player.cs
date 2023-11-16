using UnityEngine;

public class Player : Painter, IUpgradable
{
    private const string Capacity = "PlayerCapacity";

    [SerializeField] private float _defaultCapacity;

    public float MovmentSpeed { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        PaintAmount = StartPaintAmount;
    }

    protected override void OnEnable()
    {
        RayScan.WallSelected += TryPainting;
    }

    protected override void OnDisable()
    {
        RayScan.WallSelected -= TryPainting;
    }

    public override void SetUpgradeParams()
    {
        _maxPaintAmount = PlayerPrefs.GetFloat(Capacity, _defaultCapacity);
    }
}
