using System;
using System.Linq;
using UnityEngine;

public class Sorter : MonoBehaviour
{
    [SerializeField] private Storage[] _storages;
    [SerializeField] private BucketSpawner _spawner;

    private PaintBucket _currentBucket;
    private StoragePoint _targetPoint;
    private Storage _curretntStorage;

    public bool HasFreePoint => HasFreePointInStorages();

    public event Action SpawnPointFreed;

    private void OnEnable()
    {
        _spawner.BucketSpawned += OnBucketSpawned;
    }

    private void OnDisable()
    {
        _spawner.BucketSpawned -= OnBucketSpawned;
    }

    private void OnBucketSpawned(PaintBucket bucket)
    {
        if(_currentBucket != null)
        {
            _currentBucket.BucketFulled -= SortBucket;
        }
        _currentBucket = bucket;
        _currentBucket.BucketFulled += SortBucket;
    }

    private void SortBucket()
    {
        if (TrySetCurrentStorage())
        {
            StoragePoint[] currentColumn = _curretntStorage.GetPointsRow(_targetPoint.Column);
            PaintBucket[] buckets = new PaintBucket[] {_currentBucket, currentColumn[0].GetBucket(), currentColumn[1].GetBucket()};

            buckets = buckets.Where(b => b != null).ToArray();

            MoveBucketsToPoints(buckets, currentColumn);

            SpawnPointFreed?.Invoke();
        }
    }

    private bool TrySetCurrentStorage()
    {
        if (HasFreePointInStorages() == false)
            return false;

        foreach (var storage in _storages)
        {
            _targetPoint = storage.TryGetFreePoint();
            if (_targetPoint != null)
            {
                _curretntStorage = storage;
            }
        }
        return true;
    }

    private bool HasFreePointInStorages()
    {
        foreach (var storage in _storages)
        {
            if (storage.TryGetFreePoint() != null)
            {
                return true;
            }
        }
        return false;
    }

    

    private void MoveBucketsToPoints(PaintBucket[] buckets, StoragePoint[] points)
    {
        if (buckets == null)
            return;

        for (int i = 0; i < buckets.Length; i++)
        {
            if(buckets[i].enabled == true)
            {
                points[i].SetBucket(buckets[i]);
                buckets[i].StartMoving(points[i].transform);
            }
        }
    }
}
