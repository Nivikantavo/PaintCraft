using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _targetDistance;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothnessFactor;

    private Vector3 _newPosition;

    private void LateUpdate()
    {
        _newPosition = Vector3.Lerp(transform.position, _target.position + _targetDistance, _smoothnessFactor);
        transform.position = _newPosition;
        transform.LookAt(_target.position + _offset);
    }
}
