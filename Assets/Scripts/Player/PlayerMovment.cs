using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]
public class PlayerMovment : MonoBehaviour, IUpgradable
{
    private const string PlayerSpeed = "PlayerSpeed";

    [SerializeField] private Joystick _joystick;
    [SerializeField] private float _defaultSpeed;

    private Rigidbody _rigidbody;
    private float _speed;
    private Vector3 _input;
    private Vector3 _velocity;

    public bool Moving { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        SetUpgrades();
    }

    private void OnDisable()
    {
        Moving = false;
    }

    private void Update()
    {
        _input = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

        if(_input == Vector3.zero)
        {
            _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }

        _velocity = _input.normalized * _speed;

        if (_input != Vector3.zero)
        {
            transform.forward = _input;
            Moving = true;
        }
        else
        {
            Moving = false;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(_velocity * _speed);
    }

    public void SetUpgrades()
    {
        _speed = PlayerPrefs.GetFloat(PlayerSpeed, _defaultSpeed);
    }
}
