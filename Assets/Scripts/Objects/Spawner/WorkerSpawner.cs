using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _workerPrefab;
    [SerializeField] private Transform _workerContainer;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<Room> _rooms;
    [SerializeField] private List<Storage> _storages;
    [SerializeField] private PlayerWallet _playerWallet;

    private const string _workersCount = "WorkersCount";

    private void Awake()
    {
        int workersCount = PlayerPrefs.GetInt(_workersCount, 5);

        for (int i = 0; i < workersCount; i++)
        {
            var spawned = Instantiate(_workerPrefab, _spawnPoints[i].position, _spawnPoints[i].rotation, _workerContainer);
            Worker worker = spawned.GetComponent<Worker>();
            worker.Initialize(_playerWallet, _rooms, _storages);
            if(worker.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                agent.avoidancePriority = i;
            }
        }
    }
}
