using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    [SerializeField] private Boss _boss;
    [SerializeField] private Animator _animator;

    private const string Move = "Move";
    private const string GiveRevard = "GiveRevard";

    private void OnEnable()
    {
        _boss.MovmentStarted += OnMovmentStarted;
        _boss.PlayerAttained += OnPlayerAtteined;
    }

    private void OnMovmentStarted()
    {
        _animator.SetBool(Move, true);
    }

    private void OnPlayerAtteined()
    {
        _animator.SetBool(Move, false);
        _animator.SetTrigger(GiveRevard);
    }
}
