using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Sorter : MonoBehaviour
{
    public bool HasFreePoint => TryFindFreePoint();

    [SerializeField] private Storage[] _storages;
    [SerializeField] private BucketSpawner _spawner;

    private PaintBucket _currentBucket;
    private StoragePoint _targetPoint;
    private Storage _curretntStorage;

    public event UnityAction SpawnPointFreed;

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
        if (TryFindFreePoint())
        {
            StoragePoint[] currentColumn = _curretntStorage.GetPointsRow(_targetPoint.Column);
            PaintBucket[] buckets = new PaintBucket[] {_currentBucket, currentColumn[0].GetBucket(), currentColumn[1].GetBucket()};

            buckets = buckets.Where(b => b != null).ToArray();

            MoveBucketsToPoints(buckets, currentColumn);

            SpawnPointFreed?.Invoke();
        }
    }

    private bool TryFindFreePoint()
    {
        foreach (var storage in _storages)
        {
            _targetPoint = storage.TryGetFreePoint();

            if (_targetPoint != null)
            {
                _curretntStorage = storage;
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
            buckets[i].SetStoragePoint(points[i]);
            points[i].SetBucket(buckets[i]);

            StartCoroutine(MoveToPoint(buckets[i], points[i].transform));
        }
    }

    public bool TryAddBucket(PaintBucket bucket)
    {
        return TryFindFreePoint();
    }

    private IEnumerator MoveToPoint(PaintBucket backet, Transform point)
    {
        Vector3 startBacketPosition = backet.transform.position;
        float movingProgress = 0;

        while (backet.transform.position != point.position)
        {
            backet.transform.position = Vector3.Lerp(startBacketPosition, point.position, movingProgress);
            movingProgress += Time.deltaTime;
            yield return Time.deltaTime;
        }
    }
}
