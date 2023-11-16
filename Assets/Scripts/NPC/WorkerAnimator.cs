using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class WorkerAnimator : MonoBehaviour
{
    [SerializeField] private float _minAnimatedVelocity;
    [SerializeField] private float _maxAnimatedVelocity;

    private Animator _animator;
    private NavMeshAgent _agent;
    
    private const string Moving = "Moving";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if ((_minAnimatedVelocity < _agent.velocity.x &&
            _agent.velocity.x < _maxAnimatedVelocity) &&
            (_minAnimatedVelocity < _agent.velocity.z &&
            _agent.velocity.z < _maxAnimatedVelocity))
        {
            _animator.SetBool(Moving, false);
        }
        else
        {
            _animator.SetBool(Moving, true);
        }
    }
}
