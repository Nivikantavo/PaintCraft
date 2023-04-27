using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private ProgressTracker _progressTracker;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private int _levelRevard;
    [SerializeField] private float _revardDistance;

    private Transform _playerTransform;
    private bool _workDone;

    public int LevelRevard => _levelRevard;

    public event UnityAction MovmentStarted;
    public event UnityAction PlayerAttained;

    private void Awake()
    {
        _workDone = false;
        _playerTransform = _player.transform;
    }

    private void Update()
    {
        if (_workDone)
        {
            if(Vector3.Distance(transform.position, _playerTransform.position) > _revardDistance)
            {
                ApproachPlayer();
            }
            else
            {
                GiveRevard();
            }
        }
    }

    private void OnEnable()
    {
        _progressTracker.AllWallsPainted += OnAllWallsPainted;
    }

    private void OnDisable()
    {
        _progressTracker.AllWallsPainted -= OnAllWallsPainted;
    }

    private void OnAllWallsPainted()
    {
        _workDone = true;
        MovmentStarted?.Invoke();
    }

    private void ApproachPlayer()
    {
        _agent.SetDestination(_playerTransform.position);    
    }

    private void GiveRevard()
    {
        _player.PlayerWallet.AddMoney(_levelRevard);
        PlayerAttained?.Invoke();
        enabled = false;
    }
}
