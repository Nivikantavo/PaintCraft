using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PaintBucket : MonoBehaviour
{
    [SerializeField] private float _maxPaintAmount;
    [SerializeField] private float _fillingStep;
    [SerializeField] private Renderer _paintRenderer;
    [SerializeField] private GameObject _paint;
    [SerializeField] private ParticleSystem _pickUpEffect;
    [SerializeField] private GameObject _bucketRenderer;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Color _paintColor;

    private Coroutine _filling;
    private Coroutine _moving;

    private float _timeFillingStep;
    private float _currentPaintAmount = 0;
    private float _minScale = 0;
    private float _maxScale = 1;

    public Color PaintColor => _paintColor;

    public event UnityAction BucketFulled;
    public event UnityAction BucketCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Painter>(out Painter painter))
        {
            if (painter.TryTakePaint(_currentPaintAmount, _paintColor))
            {
                Collected();
            }
        }
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        if(_filling != null)
        {
            StopCoroutine(_filling);
        }
        if (_moving != null)
        {
            StopCoroutine(_moving);
        }
    }

    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
    }

    public void StartFilling(float fillingTime, Color color)
    {
        SetColor(color);
        _filling = StartCoroutine(FillBucket(fillingTime));
    }

    public void StartMoving(Transform targetPoint)
    {
        if (gameObject.activeInHierarchy)
            _filling = StartCoroutine(MoveToPoint(targetPoint));
    }

    private void Collected()
    {
        BucketCollected?.Invoke();
        _pickUpEffect.Play();
        _audioSource.PlayOneShot(_audioSource.clip);
        _bucketRenderer.SetActive(false);
        _capsuleCollider.enabled = false;
    }

    private void Initialize()
    {
        _bucketRenderer.SetActive(true);
        _capsuleCollider.enabled = true;
        _currentPaintAmount = 0;
        _paint.transform.localScale = new Vector3(_maxScale, _minScale, _maxScale);
    }

    private IEnumerator FillBucket(float fillingTime)
    {
        _timeFillingStep = fillingTime / (_maxPaintAmount / _fillingStep);
        WaitForSeconds StepBetweenLifts = new WaitForSeconds(_timeFillingStep);

        while (_currentPaintAmount < _maxPaintAmount)
        {
            _currentPaintAmount += _fillingStep;
            _currentPaintAmount = Mathf.Clamp(_currentPaintAmount, 0, _maxPaintAmount);
            _paint.transform.localScale = new Vector3(_maxScale, _currentPaintAmount, _maxScale);

            yield return StepBetweenLifts;
        }
        BucketFulled?.Invoke();
    }

    private IEnumerator MoveToPoint(Transform point)
    {
        Vector3 startBucketPosition = transform.position;
        float movingProgress = 0;

        while (transform.position != point.position)
        {
            transform.position = Vector3.Lerp(startBucketPosition, point.position, movingProgress);
            movingProgress += Time.deltaTime;
            yield return Time.deltaTime;
        }
    }

    private void SetColor(Color color)
    {
        _paintColor = color;
        _paintRenderer.material.color = _paintColor;

        ParticleSystem.MainModule mainModule = _pickUpEffect.main;
        mainModule.startColor = _paintColor;
    }
}
