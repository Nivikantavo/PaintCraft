using UnityEngine;

public class HubBoss : MonoBehaviour
{
    [SerializeField] private GameObject _nextLevelPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            _nextLevelPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            _nextLevelPanel.SetActive(false);
        }
    }
}
