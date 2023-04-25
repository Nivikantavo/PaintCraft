using UnityEngine;

public class Player : Painter, IUpgradable
{
    public float MovmentSpeed { get; private set; }

    [SerializeField] private float _defaultCapacity;

    private const string _capacity = "PlayerCapacity";

    protected override void Awake()
    {
        base.Awake();
        PaintAmount = _startPaintAmount;
        //PlayerPrefs.DeleteAll();
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
        _maxPaintAmount = PlayerPrefs.GetFloat(_capacity, _defaultCapacity);
    }
}
