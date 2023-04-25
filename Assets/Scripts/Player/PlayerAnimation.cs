using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovment))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovment _playerMovment;

    private const string Moving = "Moving";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovment = GetComponent<PlayerMovment>();
    }

    private void Update()
    {
        if(_playerMovment.Moving != _animator.GetBool(Moving))
        {
            _animator.SetBool(Moving, _playerMovment.Moving);
        }
    }

    public void Cut()
    {
        _animator.SetLayerWeight(1, 1);
    }
}
