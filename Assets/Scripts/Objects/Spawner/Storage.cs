using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private BucketSpawner _spawner;
    [SerializeField] private StoragePoint[] _pointsSource;
    [SerializeField] private Color _color;

    private int _placesInStorageRow = 3;

    private StoragePoint[,] _storagePoints;

    public Color PaintColor => _color;

    private void Awake()
    {
        Initialization();
    }

    public List<Vector3> GetBucketsPosition()
    {
        List<Vector3> positions = new List<Vector3>();
        
        for (int i = 0; i < _storagePoints.GetLength(0); i++)
        {
            for (int j = 0; j < _storagePoints.GetLength(1); j++)
            {
                if (_storagePoints[i, j].IsFree == false)
                {
                    positions.Add(_storagePoints[i, j].transform.position);
                }
            }
        }
        return positions;
    }

    public StoragePoint TryGetFreePoint()
    {
        for (int i = 0; i < _storagePoints.GetLength(0); i++)
        {
            for (int j = 0; j < _storagePoints.GetLength(1); j++)
            {
                if (_storagePoints[i, j].IsFree == true)
                {
                    return _storagePoints[i, j];
                }
            }
        }
        return null;
    }

    public StoragePoint[] GetPointsRow(int rowNumber)
    {
        return new StoragePoint[] { _storagePoints[0, rowNumber], _storagePoints[1, rowNumber], _storagePoints[2, rowNumber] };
    }

    public int GetFreePointsNumber()
    {
        int freePointsNumber = 0;

        foreach(StoragePoint point in _pointsSource)
        {
            if(point.IsFree == true)
            {
                freePointsNumber++;
            }
        }
        return freePointsNumber;
    }

    private void Initialization()
    {
        _storagePoints = new StoragePoint[_placesInStorageRow, _placesInStorageRow];
        _color = _spawner.FillColor;
        int iterationNumber = 0;
        for (int i = 0; i < _storagePoints.GetLength(0); i++)
        {
            for (int j = 0; j < _storagePoints.GetLength(1); j++)
            {
                _storagePoints[i, j] = _pointsSource[iterationNumber];
                _storagePoints[i, j].SetRow(i);
                _storagePoints[i, j].SetColumn(j);
                iterationNumber++;
            }
        }
    }
}
