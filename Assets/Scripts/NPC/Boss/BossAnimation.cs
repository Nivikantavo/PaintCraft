using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    private const string Move = "Move";
    private const string GiveRevard = "GiveRevard";

    [SerializeField] private Boss _boss;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        _boss.Moving += OnMoving;
        _boss.Paying += OnPaying;
    }

    private void OnMoving()
    {
        _animator.SetBool(Move, true);
    }

    private void OnPaying()
    {
        _animator.SetBool(Move, false);
        _animator.SetTrigger(GiveRevard);
    }
}
