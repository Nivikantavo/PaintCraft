using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjects : MonoBehaviour
{
    [SerializeField] private Player _player;

    private Wall[] _currentWalls;

    private void FixedUpdate()
    {
        RaycastHit[] hits;

        Vector3 direction = _player.transform.position - transform.position;
        Vector3 position = transform.position;

        Ray ray = new Ray(position, direction);
        Debug.DrawRay(position, direction);
        hits = Physics.RaycastAll(ray, 500f);

        
        foreach (var hit in hits)
        {
            if(hit.collider.TryGetComponent<Wall>(out Wall wall))
            {
                
            }
        }
    }
}
