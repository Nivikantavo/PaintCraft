using UnityEngine;

public class Wobble : MonoBehaviour
{
    [SerializeField] private Painter _painter;

    [SerializeField] private float _maxWobble = 0.03f;
    [SerializeField] private float _wobbleSpeed = 1f;
    [SerializeField] private float _recovery = 1f;

    private Renderer _renderer;
    private Vector3 _lastPosition;
    private Vector3 _velocity;
    private Vector3 _lastRotation;
    private Vector3 _angularVelocity;
    private float _wobbleAmountX;
    private float _wobbleAmountZ;
    private float _wobbleAmountToAddX;
    private float _wobbleAmountToAddZ;
    private float _pulse;
    private float _time = 0.5f;

    private const string _wobbleX = "WobbleX";
    private const string _wobbleZ = "WobbleZ";

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(_painter.PaintAmount > 0)
        {
            _time += Time.deltaTime;

            _wobbleAmountToAddX = Mathf.Lerp(_wobbleAmountToAddX, 0, Time.deltaTime * _recovery);
            _wobbleAmountToAddZ = Mathf.Lerp(_wobbleAmountToAddZ, 0, Time.deltaTime * _recovery);

            _pulse = 2 * Mathf.PI * _wobbleSpeed;
            _wobbleAmountX = _wobbleAmountToAddX * Mathf.Sin(_pulse * _time);
            _wobbleAmountZ = _wobbleAmountToAddZ * Mathf.Sin(_pulse * _time);

            _renderer.material.SetFloat(_wobbleX, _wobbleAmountX);
            _renderer.material.SetFloat(_wobbleZ, _wobbleAmountZ);

            _velocity = (_lastPosition - transform.position) / Time.deltaTime;
            _angularVelocity = transform.rotation.eulerAngles - _lastRotation;


            _wobbleAmountToAddX += Mathf.Clamp((_velocity.x + (_angularVelocity.z * 0.2f)) * _maxWobble, -_maxWobble, _maxWobble);
            _wobbleAmountToAddZ += Mathf.Clamp((_velocity.z + (_angularVelocity.x * 0.2f)) * _maxWobble, -_maxWobble, _maxWobble);

            _lastPosition = transform.position;
            _lastRotation = transform.rotation.eulerAngles;
        }
    }
}