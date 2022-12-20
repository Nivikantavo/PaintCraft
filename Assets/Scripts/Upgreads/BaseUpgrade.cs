using System.Collections.Generic;
using UnityEngine;

public class BaseUpgrade : Upgradable
{
    [SerializeField] private List<GameObject> _baseRooms;

    public override void Upgrade()
    {
        base.Upgrade();
        TurnOnRooms();
    }

    protected override void Awake()
    {
        base.Awake();
        TurnOnRooms();
    }

    private void TurnOnRooms()
    {
        for (int i = 0; i < _baseRooms.Count; i++)
        {
            if(i < CurrentLevel)
            {
                _baseRooms[i].SetActive(true);
            }
            else
            {
                _baseRooms[i].SetActive(false);
            }
        }
    }
}
