using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RayScan))]
public class PaintingAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem _paintingEffect;

    private RayScan _rayScan;
    private Animator _animator;

    private bool _ikActive = false;
    private Transform _currentWall = null;

    private Vector3 _offset = Vector3.zero;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rayScan = GetComponent<RayScan>();
        _paintingEffect.Stop();
    }

    private void OnEnable()
    {
        _rayScan.WallSelected += OnWallSelected;
        _rayScan.WallDeselected += OnWallDeselected;
    }

    private void OnDisable()
    {
        _rayScan.WallSelected -= OnWallSelected;
        _rayScan.WallDeselected -= OnWallDeselected;
    }

    private void OnAnimatorIK()
    {
        if (_animator)
        {
            if (_ikActive)
            {
                if (_currentWall != null)
                {
                    _offset = new Vector3(0, _rayScan.CurrentWallPainted, 0);

                    _animator.SetLookAtWeight(1);
                    _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    _animator.SetIKPosition(AvatarIKGoal.RightHand, _currentWall.position - _offset);
                }
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                _animator.SetLookAtWeight(0);
            }
        }
    }

    private void OnWallSelected(Wall wall)
    {
        _paintingEffect.Play();
        _currentWall = wall.transform;
        _ikActive = true;
    }

    private void OnWallDeselected()
    {
        _paintingEffect.Stop();
        _offset = Vector3.zero;
        _currentWall = null;
        _ikActive = false;
    }
}
