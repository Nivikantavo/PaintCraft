using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Painter))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Worker))]
public class PaintingState : State
{
    [SerializeField] private float _stoppingDistance;

    private List<Wall> _currentWalls;
    private NavMeshAgent _agent;
    private Worker _worker;
    private Wall _currentWall;
    private Coroutine _paintingCorutine;

    private float _distanceOffset = 0.5f;
    private int _straightAngle = 90;

    private void Awake()
    {
        _worker = GetComponent<Worker>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _agent.stoppingDistance = _stoppingDistance;

        if (_worker.CurrentRoom == null)
        {
            _worker.ChooseRoom();
        }

        FindeUnpaintedWall();
    }

    private void OnDisable()
    {
        if(_paintingCorutine != null)
        {
            StopCoroutine(_paintingCorutine);
        }

        _agent.stoppingDistance = 0;

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
        int randomNumber;

        if (_worker.CurrentRoom.DonePercentage >= 1)
        {
            if (_worker.ChooseRoom() == false)
            {
                enabled = false;
                return;
            }
        }

        _currentWalls = _worker.CurrentRoom.GetUnpaitedWalls();

        if(_currentWalls.Count == 0)
        {
            return;
        }

        randomNumber = Random.Range(0, _currentWalls.Count);
        _currentWall = _currentWalls[randomNumber];

        _paintingCorutine = null;
    }

    private void PaintingWalls()
    {
        _agent.SetDestination(_currentWall.PaintigPoint);

        if (Vector3.Distance(_currentWall.PaintigPoint, transform.position) <= _stoppingDistance + _distanceOffset)
        {
            if (_currentWall.Painted >= 1)
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
            else
            {
                if(ColorComparator.CompareColor(_worker.CurrentColor, _currentWall.Color) == false)
                {
                    _worker.TryTakePaint(0, _currentWall.Color);
                }
                transform.rotation = Quaternion.Euler(new Vector3(_currentWall.transform.eulerAngles.x, _currentWall.transform.eulerAngles.y + _straightAngle, _currentWall.transform.eulerAngles.z));
            }
        }
    }
}
