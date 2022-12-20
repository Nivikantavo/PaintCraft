using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Painter))]
[RequireComponent(typeof(NavMeshAgent))]
public class PaintingState : State
{
    [SerializeField] private float _stoppingDistance;

    private List<Room> _rooms;
    private List<Wall> _currentWalls;
    private NavMeshAgent _agent;
    private Room _currentRoom;
    private Wall _currentWall;
    private Painter _painter;
    private Coroutine _paintingCorutine;

    public void Initialize(List<Room> rooms)
    {
        _rooms = rooms;
    }

    private void Awake()
    {
        _painter = GetComponent<Painter>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _agent.stoppingDistance = _stoppingDistance;

        if (_currentRoom == null)
        {
            ChooseRoom();
        }

        FindeUnpaintedWall();

        foreach(Room room in _rooms)
        {
            room.PaintedComplited += OnPaintedComplited;
        }
    }
    private void OnDisable()
    {
        if(_paintingCorutine != null)
        {
            StopCoroutine(_paintingCorutine);
        }

        _agent.stoppingDistance = 0;

        foreach (Room room in _rooms)
        {
            room.PaintedComplited -= OnPaintedComplited;
        }
        if(_paintingCorutine != null)
        {
            StopCoroutine(_paintingCorutine);
            _paintingCorutine = null;
        }
    }

    private void Update()
    {
        if (_currentWalls != null && _currentWalls.Count > 0)
        {
            PaintingWalls();
        }
        else
        {
            FindeUnpaintedWall();
        }
    }

    private void FindeUnpaintedWall()
    {
        if(_rooms != null && _rooms.Count > 0)
        {
            int randomNumber;

            foreach (Room room in _rooms)
            {
                if (ColorComparator.CompareColor(room.Color, _painter.CurrentColor))
                {
                    _currentRoom = room;
                }
            }

            if (_currentRoom.DonePercentage >= 100)
            {
                ChooseRoom();
            }

            _currentWalls = _currentRoom.GetUnpaitedWalls();

            randomNumber = Random.Range(0, _currentWalls.Count);
            _currentWall = _currentWalls[randomNumber];

            _paintingCorutine = null;
        }
        else
        {
            _agent.isStopped = true;
        }
    }

    private void PaintingWalls()
    {
        _agent.SetDestination(_currentWall.transform.position);
        Debug.DrawLine(_currentWall.transform.position, transform.position, Color.red);

        if (Vector3.Distance(_currentWall.transform.position, transform.position) <= _stoppingDistance + 1 && _currentWall.Painted >= 1)
        {
            if (_currentWalls.IndexOf(_currentWall) < _currentWalls.Count - 1)
            {
                _currentWall = _currentWalls[_currentWalls.IndexOf(_currentWall) + 1];
            }
            else
            {
                FindeUnpaintedWall();
            }
        }
    }

    private void ChooseRoom()
    {
        if(_rooms.Count > 0)
        {
            int randomNumber = Random.Range(0, _rooms.Count);

            _currentRoom = _rooms[randomNumber];
            _painter.TryTakePaint(0, _currentRoom.Color);
        }
    }

    private void OnPaintedComplited(Room room)
    {
        _rooms.Remove(room);
        _currentWalls = null;
    }
}
