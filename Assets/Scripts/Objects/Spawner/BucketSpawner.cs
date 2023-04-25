using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;

public class BucketSpawner : ObjectPool
{
    public Color FillColor => _fillColor;

    [SerializeField] private GameObject _bucketPrefub;
    [SerializeField] private StoragePoint _spawnPoint;
    [SerializeField] private float _fillingTime;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private Color _fillColor;
    [SerializeField] private ParticleSystem _paintParticles;
    [SerializeField] private Sorter _sorter;

    private float _secondsBetweenSpawn;
    private float _elapsedTime;

    public event UnityAction<PaintBucket> BucketSpawned;

    private void Awake()
    {
        var mainModule = _paintParticles.main;
        mainModule.startColor = _fillColor;
    }

    private void Start()
    {
        Initialize(_bucketPrefub);
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

        if (_sorter.TryAddBucket(paintBucket))
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
