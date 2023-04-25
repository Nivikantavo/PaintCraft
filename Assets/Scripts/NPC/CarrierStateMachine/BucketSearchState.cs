using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BucketSearchState : MonoBehaviour
{
    private int _capacity;
    private PaintBucket[] _currentBuckets;
    private Room[] _rooms;
    private Storage _currentRoomStorage;
    private Storage[] _spawnerStorages;
    private Storage[] _roomsStotages;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(_currentRoomStorage == null)
        {
            foreach(var room in _rooms)
            {
                if(room.DonePercentage < 1)
                {
                    //_currentRoomStorage = room.Storage;
                }
            }
        }
        //Проверить наличие свободных мест в хранилищах
        if ( _currentRoomStorage == null)
        {
            _currentRoomStorage = CheckFreeStorage();
            if (_currentRoomStorage == null)
                return;
        }
        else
        {
            //Пойти к хранилищу нужного цвета
            foreach (var storage in _spawnerStorages)
            {
            }
        }
        //Взять свободные ведра
        //отнести к выбранному хранилищу
    }

    private Storage CheckFreeStorage()
    {
        Storage freeStorage = null;

        foreach (var storage in _roomsStotages)
        {
            if (storage.GetFreePointsNumber() > 0) 
            { 
                freeStorage = storage;
            }
        }

        return freeStorage;
    }
}
