using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : MonoBehaviour
{
    [SerializeField] private List<GameObject> _interiorWalls;

    private void OnEnable()
    {
        foreach (GameObject wall in _interiorWalls)
        {
            wall.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject wall in _interiorWalls)
        {
            wall.SetActive(true);
        }
    }
}
