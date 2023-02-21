using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PaintBucket : MonoBehaviour
{
    public Color PaintColor => _paintColor;

    [SerializeField] private float _maxPaintAmount;
    [SerializeField] private float _fillingStep;
    [SerializeField] private Renderer _paintRenderer;
    [SerializeField] private GameObject _paint;
    [SerializeField] private ParticleSystem _pickUpEffect;
    [SerializeField] private GameObject _bucketRenderer;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Color _paintColor;

    private float _timeFillingStep;
    private float _currentPaintAmount = 0;

    public event UnityAction BucketFulled;
    public event UnityAction BucketCollected;

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
        _paint.transform.localScale = new Vector3(1, 0, 1);
    }

    private IEnumerator FillBucket(float fillingTime)
    {
        _timeFillingStep = fillingTime / (_maxPaintAmount / _fillingStep);
        WaitForSeconds StepBetweenLifts = new WaitForSeconds(_timeFillingStep);

        while (_currentPaintAmount < _maxPaintAmount)
        {
            _currentPaintAmount += _fillingStep;
            _currentPaintAmount = Mathf.Clamp(_currentPaintAmount, 0, _maxPaintAmount);
            _paint.transform.localScale = new Vector3(1, _currentPaintAmount, 1);

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
        _paintRenderer.material.color = _paintColor;

        ParticleSystem.MainModule mainModule = _pickUpEffect.main;
        mainModule.startColor = _paintColor;
    }
}
