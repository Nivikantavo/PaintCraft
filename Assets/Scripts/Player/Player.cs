using UnityEngine;

public class Player : Painter, IUpgradable
{
    private const string Capacity = "PlayerCapacity";

    [SerializeField] private float _defaultCapacity;

    public float MovmentSpeed { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        PaintAmount = _startPaintAmount;
    }

    protected override void OnEnable()
    {
        _rayScan.WallSelected += TryPainting;
    }

    protected override void OnDisable()
    {
        _rayScan.WallSelected -= TryPainting;
    }

    public override void SetUpgrades()
    {
        _maxPaintAmount = PlayerPrefs.GetFloat(Capacity, _defaultCapacity);
    }
}
