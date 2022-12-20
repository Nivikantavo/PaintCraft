using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    public Color Color => _roomColor;
    public float DonePercentage {get; private set;}

    [SerializeField] private List<Wall> _walls;
    [SerializeField] private Color _roomColor;

    private int _wallsPaintedCount;

    public event UnityAction<Room> PaintedComplited;

    public List<Wall> GetUnpaitedWalls()
    {
        List<Wall> unpaintedWalls = new List<Wall>();

        foreach (var wall in _walls)
        {
            if(wall.Painted < 1)
            {
                unpaintedWalls.Add(wall);
            }
        }

        return unpaintedWalls;
    }

    private void Awake()
    {
        SetWallsColor();

        foreach (var wall in _walls)
        {
            wall.WallPainted += CalculateDonePercentage;
        }
    }

    private void SetWallsColor()
    {
        foreach(var wall in _walls)
        {
            wall.SetColor(_roomColor);
        }
    }

    private void CalculateDonePercentage()
    {
        _wallsPaintedCount++;
        DonePercentage = (float)_wallsPaintedCount / (float)_walls.Count * 100;
        if(DonePercentage == 100)
        {
            PaintedComplited?.Invoke(this);
        }
    }
}
