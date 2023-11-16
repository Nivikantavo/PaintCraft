using System;
using UnityEngine;

public class BucketSpawner : ObjectPool
{
    [SerializeField] private GameObject _bucketTemplate;
    [SerializeField] private StoragePoint _spawnPoint;
    [SerializeField] private float _fillingTime;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private Color _fillColor;
    [SerializeField] private ParticleSystem _paintStream;
    [SerializeField] private Sorter _sorter;

    private float _secondsBetweenSpawn;
    private float _elapsedTime;

    public Color FillColor => _fillColor;

    public event Action<PaintBucket> BucketSpawned;

    private void Awake()
    {
        var mainModule = _paintStream.main;
        mainModule.startColor = _fillColor;
    }

    private void Start()
    {
        Initialize(_bucketTemplate);
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

    private void SpawnBucket(GameObject bucket, Vector3 spawnPoint)
    {
        PaintBucket paintBucket = bucket.GetComponent<PaintBucket>();

        if (_sorter.HasFreePoint)
        {
            bucket.SetActive(true);
            bucket.transform.position = spawnPoint;

            _spawnPoint.SetBucket(paintBucket);

            BucketSpawned?.Invoke(paintBucket);

            paintBucket.StartFilling(_fillingTime, _fillColor);
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
