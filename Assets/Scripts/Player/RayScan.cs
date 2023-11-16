using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Painter))]
public class RayScan : MonoBehaviour
{
    [SerializeField] private int _rays;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _angle;
    [SerializeField] private Vector3 _offset;
    [Range(0, 0.99f)] [SerializeField] private float _distanceMultiplier;

    private Wall _currentWall;
    private List<Wall> _detectedWalls;
    private Painter _painter;

    public float CurrentWallPainted => _currentWall.Painted;

    public event Action<Wall> WallSelected;
    public event Action WallDeselected;

    private void Awake()
    {
        _painter = GetComponent<Painter>();
    }

    private void FixedUpdate()
    {
        if (_painter.PaintAmount >= _painter.PaintCost)
        {
            _detectedWalls = RayToScan();

            if (_detectedWalls != null && _detectedWalls.Any() != false)
            {
                if (_currentWall == null)
                {
                    if (ColorComparator.CompareColor(_detectedWalls[0].Color, _painter.CurrentColor))
                    {
                        _currentWall = _detectedWalls[0];
                        WallSelected?.Invoke(_currentWall);
                    }
                }
                if (_currentWall && _currentWall.Painted > 1)
                {
                    RemoveWallFromList();
                }
            }
            else
            {
                RemoveWallFromList();
            }
        }
        else
        {
            RemoveWallFromList();
        }
    }

    private List<Wall> RayToScan()
    {
        List<Wall> result = new List<Wall>();

        float rayStep = 0;
        float distanceMultiplier;
        Wall detectedWall = null;

        for (int i = 0; i < _rays; i++)
        {
            var x = Mathf.Sin(rayStep);
            var y = Mathf.Cos(rayStep);
            distanceMultiplier = Mathf.Pow(_distanceMultiplier, i);

            rayStep += _angle * Mathf.Deg2Rad / _rays;

            Vector3 dir = transform.TransformDirection(new Vector3(x, 0, y));

            detectedWall = GetRaycast(dir, _maxDistance * distanceMultiplier);

            AddDetectedWall(detectedWall, result);

            if (x != 0)
            {
                dir = transform.TransformDirection(new Vector3(-x, 0, y));
                detectedWall = GetRaycast(dir, _maxDistance * distanceMultiplier);

                AddDetectedWall(detectedWall, result);
            }
        }
        return result;
    }

    private void AddDetectedWall(Wall detectedWall, List<Wall> walls)
    {
        if (detectedWall != null && walls.Contains(detectedWall) == false)
        {
            walls.Add(detectedWall);
        }
    }

    private Wall GetRaycast(Vector3 direction, float distance)
    {
        Wall result = null;
        RaycastHit hit = new RaycastHit();
        Vector3 position = transform.position + _offset;

        if (Physics.Raycast(position, direction, out hit, distance))
        {
            if (hit.transform.gameObject.TryGetComponent<Wall>(out Wall wall))
            {
                if(wall.Painted < 1)
                {
                    result = wall;
                }
            }
        }
        return result;
    }

    private void RemoveWallFromList()
    {
        if(_currentWall != null)
        {
            _detectedWalls.Remove(_currentWall);
            _currentWall.Deselect();
            _currentWall = null;
            WallDeselected?.Invoke();
        }
    }
}
