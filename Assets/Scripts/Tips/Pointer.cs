using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;

    private Transform _target;
    public Transform Target => _target;

    private void Update()
    {
        if(_target != null && _target.gameObject.activeSelf == true)
        {
            transform.position = _target.position + _offset;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Initialize(Transform target)
    {
        _target = target;
    }
}
