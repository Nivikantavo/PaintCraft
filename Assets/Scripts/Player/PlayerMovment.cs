using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]
public class PlayerMovment : MonoBehaviour, IUpgradable
{
    public bool Moving { get; private set; }

    [SerializeField] private Joystick _joystick;
    [SerializeField] private float _defaultSpeed;

    private Rigidbody _rigidbody;
    private float _speed;

    private Vector3 _input;
    private Vector3 _velocity;

    private const string _playerSpeed = "PlayerSpeed";

    public void SetUpgrades()
    {
        _speed = PlayerPrefs.GetFloat(_playerSpeed, _defaultSpeed);
    }

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
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.deltaTime);
    }
}
