using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class NextLevelStarter : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player player))
        {
            player.GetComponent<PlayerMovment>().enabled = false;
            _levelLoader.LoadNextLevel();
        }
    }
}
