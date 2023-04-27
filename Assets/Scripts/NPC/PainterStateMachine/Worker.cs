using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(FindingPaintState))]
public class Worker : Painter
{
    private const string Speed = "WorkerSpeed";
    private const string Capacity = "WorkerCapacity";

    public Room CurrentRoom { get; private set; }

    [SerializeField] private float _defaultCapacity;
    [SerializeField] private float _defaultSpeed;

    private List<Room> _rooms;
    private FindingPaintState _findingPaintState;
    private NavMeshAgent _agent;

    protected override void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _findingPaintState = GetComponent<FindingPaintState>();
        base.Awake();
        _startPaintAmount = _maxPaintAmount;
        PaintAmount = _startPaintAmount;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (Room room in _rooms)
        {
            room.PaintedComplited -= OnPaintedComplited;
        }
    }

    public bool ChooseRoom()
    {
        if (_rooms.Count > 0)
        {
            int randomNumber = Random.Range(0, _rooms.Count);

            CurrentRoom = _rooms[randomNumber];
            this.TryTakePaint(0, CurrentRoom.Color);
            return true;
        }
        else
        {
            _agent.isStopped = true;
            _findingPaintState.enabled = false;
            return false;
        }
    }

    public override void SetUpgrades()
    {
        _maxPaintAmount = PlayerPrefs.GetFloat(Capacity, _defaultCapacity);
        _agent.speed = PlayerPrefs.GetFloat(Speed, _defaultSpeed);
    }

    public void Initialize(PlayerWallet wallet, List<Room> rooms, List<Storage> storages)
    {
        Wallet = wallet;
        _rooms = rooms;

        foreach (Room room in _rooms)
        {
            room.PaintedComplited += OnPaintedComplited;
        }

        _findingPaintState.Initialize(storages);
        ChooseRoom();
    }

    private void OnPaintedComplited(Room room)
    {
        if (_rooms.Contains(room))
        {
            _rooms.Remove(room);
        }
        if(CurrentRoom == room)
        {
            ChooseRoom();
        }
    }
}
