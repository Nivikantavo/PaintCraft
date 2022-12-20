using UnityEngine;
using UnityEngine.Events;

public class BucketSpawner : ObjectPool
{
    public Color FillColor => _fillColor;

    [SerializeField] private GameObject _backetPrefub;
    [SerializeField] private StoragePoint _spawnPoint;
    [SerializeField] private float _fillingTime;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private Color _fillColor;
    [SerializeField] private ParticleSystem _particleSystem;
    
    [SerializeField] private Sorter _sorter;

    private float _secondsBetweenSpawn;

    private float _elapsedTime;

    public event UnityAction<PaintBucket> BucketSpawned;

    private void Awake()
    {
        ParticleSystem.MainModule mainModule = _particleSystem.main;
        mainModule.startColor = _fillColor;
    }

    private void Start()
    {
        Initialize(_backetPrefub);
        _secondsBetweenSpawn = _fillingTime + _spawnDelay;
        _elapsedTime = _secondsBetweenSpawn;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if(_elapsedTime >= _secondsBetweenSpawn)
        {
            if(TryGetObject(out GameObject bucket) && _spawnPoint.IsFree == true)
            {
                _elapsedTime = 0;
                SpawnBucket(bucket, _spawnPoint.transform.position);
            }
        }
    }

    private void SpawnBucket(GameObject backet, Vector3 spawnPoint)
    {
        PaintBucket paintBacket = backet.GetComponent<PaintBucket>();

        if (_sorter.TryAddBucket(paintBacket))
        {
            backet.SetActive(true);
            backet.transform.position = spawnPoint;

            _spawnPoint.SetBucket(paintBacket);
            paintBacket.SetStoragePoint(_spawnPoint);

            BucketSpawned?.Invoke(paintBacket);

            paintBacket.StartFilling(_fillingTime, _fillColor);
        }
    }

    private void OnEnable()
    {
        _sorter.SpawnPointFreed += OnSpawnPointFreed;
    }

    private void OnDisable()
    {
        _sorter.SpawnPointFreed -= OnSpawnPointFreed;
    }

    private void OnSpawnPointFreed()
    {
        _spawnPoint.SetBucket(null);
    }
}
