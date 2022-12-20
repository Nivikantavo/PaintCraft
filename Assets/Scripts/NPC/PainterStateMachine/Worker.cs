using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PaintingState))]
[RequireComponent(typeof(FindingPaintState))]
public class Worker : Painter
{
    [SerializeField] private float _defaultCapacity;
    [SerializeField] private float _defaultSpeed;

    private PaintingState _paintingState;
    private FindingPaintState _findingPaintState;
    private NavMeshAgent _agent;

    private const string _speed = "WorkerSpeed";
    private const string _capacity = "WorkerCapacity";

    protected override void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _paintingState = GetComponent<PaintingState>();
        _findingPaintState = GetComponent<FindingPaintState>();
        base.Awake();
    }

    public override void SetUpgrades()
    {
        _maxPaintAmount = PlayerPrefs.GetFloat(_capacity, _defaultCapacity);
        _agent.speed = PlayerPrefs.GetFloat(_speed, _defaultSpeed);
    }

    public void Initialize(PlayerWallet wallet, List<Room> rooms, List<Storage> storages)
    {
        Wallet = wallet;
        _paintingState.Initialize(rooms);
        _findingPaintState.Initialize(storages, rooms);
    }
}
