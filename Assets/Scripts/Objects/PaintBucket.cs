using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PaintBucket : MonoBehaviour
{
    public Color PaintColor => _paintColor;

    [SerializeField] private float _maxPaintAmount; 
    [SerializeField] private float _fillingStep;
    [SerializeField] private Renderer _paintRenderer;
    [SerializeField] private ParticleSystem _pickUpEffect;
    [SerializeField] private GameObject _bucketRenderer;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Color _paintColor;

    private float _timeFillingStep;
    private float _currentPaintAmount = 0;
    private float _startMaterialFill = -0.7f;
    private float _endMaterialFill = 0.6f;
    private const string _fill = "Fill";
    private const string _liquidColor = "LiquidColor";
    private StoragePoint _storagePoint;

    public event UnityAction BucketFulled;
    public event UnityAction BucketCollected;

    public void SetStoragePoint(StoragePoint newStoragePoint)
    {
        _storagePoint = newStoragePoint;
    }

    public void StartFilling(float fillingTime, Color color)
    {
        SetColor(color);
        StartCoroutine(FillBucket(fillingTime));
    }

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
        _storagePoint = null;
        _paintRenderer.material.SetFloat(_fill, _startMaterialFill);
    }

    private IEnumerator FillBucket(float fillingTime)
    {
        _timeFillingStep = fillingTime / (_maxPaintAmount / _fillingStep);
        WaitForSeconds StepBetweenLifts = new WaitForSeconds(_timeFillingStep);
        float paintLevel;

        while (_currentPaintAmount < _maxPaintAmount)
        {
            _currentPaintAmount += _fillingStep;

            paintLevel = Mathf.Lerp(_startMaterialFill, _endMaterialFill, _currentPaintAmount);

            _paintRenderer.material.SetFloat(_fill, paintLevel);

            yield return StepBetweenLifts;
        }
        BucketFulled?.Invoke();
    }

    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
    }

    private void SetColor(Color color)
    {
        _paintColor = color;
        _paintRenderer.material.SetColor(_liquidColor, _paintColor);

        ParticleSystem.MainModule mainModule = _pickUpEffect.main;
        mainModule.startColor = _paintColor;
    }
}
