using System.Collections.Generic;
using UnityEngine;

public class GameMechanicTeacher : ObjectPool
{
    [SerializeField] private BucketSpawner _spawner;
    [SerializeField] private Player _player;
    [SerializeField] private List<GameObject> _walls;
    [SerializeField] private GameObject _pointerPrefab;

    private List<GameObject> _buckets = new List<GameObject>();
    private List<Pointer> _pointers = new List<Pointer>();

    private bool _isCurrentTargetWalls;

    private void Start()
    {
        _isCurrentTargetWalls = true;
        Initialize(_pointerPrefab);
    }

    private void Update()
    {
        if(_player.PaintAmount < _player.PaintCost && _isCurrentTargetWalls == true)
        {
            ChangeTargets(_buckets);
        }
        else if(_player.PaintAmount >= _player.PaintCost && _isCurrentTargetWalls == false)
        {
            ChangeTargets(_walls);
        }
    }

    private void OnEnable()
    {
        _spawner.BucketSpawned += OnBucketSpawned;

        for (int i = 0; i < _walls.Count; i++)
        {
            _walls[i].GetComponent<Wall>().WallPainted += RemoveWallFromList;
        }
    }

    private void OnDisable()
    {
        _spawner.BucketSpawned -= OnBucketSpawned;

        for (int i = 0; i < _walls.Count; i++)
        {
            _walls[i].GetComponent<Wall>().WallPainted -= RemoveWallFromList;
        }
    }

    private void OnBucketSpawned(PaintBucket bucket)
    {
        _buckets.Add(bucket.gameObject);

        if(_isCurrentTargetWalls == false)
        {
            if (TryGetObject(out GameObject pointer))
            {
                SpawnPointer(pointer, bucket.transform);
                _pointers.Add(pointer.GetComponent<Pointer>());
            }
        }
    }

    private void ChangeTargets(List<GameObject> newTarget)
    {
        foreach(Pointer pointer in _pointers)
        {
            pointer.gameObject.SetActive(false);
        }
        _pointers.Clear();

        foreach (GameObject target in newTarget)
        {
            if (TryGetObject(out GameObject pointer))
            {
                SpawnPointer(pointer, target.transform);
                _pointers.Add(pointer.GetComponent<Pointer>());
            }
        }

        _isCurrentTargetWalls = !_isCurrentTargetWalls;
    }

    private void RemoveWallFromList(Wall wall)
    {
        _walls.Remove(wall.gameObject);

        foreach(Pointer pointer in _pointers)
        {
            if(pointer.Target == wall.transform)
            {
                pointer.gameObject.SetActive(false);
            }
        }
    }

    private void SpawnPointer(GameObject pointer, Transform target)
    {
        pointer.gameObject.SetActive(true);
        pointer.GetComponent<Pointer>().Initialize(target);
    }
}
