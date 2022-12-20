using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TransportationState : MonoBehaviour
{
    [SerializeField] private Storage[] _roomsStorages;

    private PaintBucket[] _currentBuckets;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        //_agent.SetDestination()
    }
}
