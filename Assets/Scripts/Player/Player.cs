using UnityEngine;
using UnityEngine.Events;

public class Player : Painter, IUpgradable
{
    public float MovmentSpeed { get; private set; }

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _defaultCapacity;

    private const string _capacity = "PlayerCapacity";

    protected override void Awake()
    {
        PlayerPrefs.DeleteAll();
        base.Awake();
    }

    protected override void OnEnable()
    {
        _rayScan.WallSelected += TryPainting;
        _rayScan.WallDeselected += TurnOffSprayer;
    }

    protected override void OnDisable()
    {
        _rayScan.WallSelected -= TryPainting;
        _rayScan.WallDeselected -= TurnOffSprayer;
        
    }

    public override void SetUpgrades()
    {
        _maxPaintAmount = PlayerPrefs.GetFloat(_capacity, _defaultCapacity);
    }

    protected override void TryPainting(Wall wall)
    {
        if(PaintAmount >= _paint—onsumption)
        {
            wall.Select(this);
            if (_audioSource.isPlaying == false)
            {
                _audioSource.Play();
            }
        }
    }

    private void TurnOffSprayer()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
