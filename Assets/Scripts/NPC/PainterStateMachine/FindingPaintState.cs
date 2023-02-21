using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Worker))]
public class FindingPaintState : State
{
    private List<Storage> _storages;
    private NavMeshAgent _agent;
    private Worker _worker;
    private Storage _currentStorage;
    private CapsuleCollider _collider;

    public void Initialize(List<Storage> storages)
    {
        _storages = storages;
    }

    private void Awake()
    {
        _worker = GetComponent<Worker>();
        _agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        _collider.enabled = true;
    }

    private void OnDisable()
    {
        _collider.enabled = false;
    }

    private void Update()
    {
        if( _worker.CurrentRoom == null)
        {
            _worker.ChooseRoom();
        }

        if(_currentStorage != null && ColorComparator.CompareColor(_currentStorage.PaintColor, _worker.CurrentRoom.Color) )
        {
            GoForPaint();
        }
        else
        {
            _currentStorage = FindStorage();
        }
    }

    private Storage FindStorage()
    {
        foreach (Storage storage in _storages)
        {
            if (ColorComparator.CompareColor(storage.PaintColor, _worker.CurrentRoom.Color))
            {
                return storage;
            }
        }
        _worker.ChooseRoom();
        return FindStorage();
    }

    private void GoForPaint()
    {
        List<Vector3> route = _currentStorage.GetBucketsPosition();
        
        foreach (Vector3 routePoint in route)
        {
            _agent.SetDestination(routePoint);
        }
    }
}
