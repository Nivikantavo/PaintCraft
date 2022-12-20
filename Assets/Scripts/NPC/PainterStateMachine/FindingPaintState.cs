using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FindingPaintState : State
{
    private List<Room> _rooms;
    private List<Storage> _storages;
    private NavMeshAgent _agent;
    private Room _currentRoom;
    private Storage _currentStorage;
    private CapsuleCollider _collider;

    public void Initialize(List<Storage> storages, List<Room> rooms)
    {
        _storages = storages;
        _rooms = rooms;
    }

    private void Awake()
    {
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
        if( _currentRoom == null)
        {
            ChooseRoom();
        }

        _currentStorage = FindStorage();

        TakePaint();
    }

    private void ChooseRoom()
    {
        int randomNumber = UnityEngine.Random.Range(0, _rooms.Count);

        _currentRoom = _rooms[randomNumber];
    }

    private Storage FindStorage()
    {
        foreach (Storage storage in _storages)
        {
            if (ColorComparator.CompareColor(storage.PaintColor, _currentRoom.Color))
            {
                return storage;
            }
        }
        Debug.Log("Не найдено нужного Storage");
        throw new InvalidOperationException();
    }

    private void TakePaint()
    {
        List<Vector3> route = _currentStorage.GetBucketsPosition();

        foreach(Vector3 routePoint in route)
        {
            _agent.SetDestination(routePoint);
        }
    }
}
