using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Room : MonoBehaviour
{
    [SerializeField] private List<Wall> _walls;
    [SerializeField] private Color _roomColor;

    private int _wallsPaintedCount;

    public Color Color => _roomColor;
    public float DonePercentage { get; private set; }

    public event UnityAction<Room> PaintedComplited;
    public event UnityAction DonePercentageChanged;
    public event UnityAction<Room> PlayerEnterRoom;

    private void Awake()
    {
        SetWallsColor();

        foreach (var wall in _walls)
        {
            wall.WallPainted += CalculateDonePercentage;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            PlayerEnterRoom?.Invoke(this);
        }
    }

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

    private void SetWallsColor()
    {
        foreach(var wall in _walls)
        {
            wall.SetColor(_roomColor);
        }
    }

    private void CalculateDonePercentage(Wall wall)
    {
        _wallsPaintedCount++;
        DonePercentage = (float)_wallsPaintedCount / (float)_walls.Count;
        DonePercentageChanged?.Invoke();
        if(DonePercentage == 1)
        {
            PaintedComplited?.Invoke(this);
        }
    }
}
