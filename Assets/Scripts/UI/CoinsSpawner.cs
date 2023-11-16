using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CoinsSpawner : ObjectPool
{
    public event System.Action CoinDelivered;

    [SerializeField] private GameObject _spawnPrefab;
    [SerializeField] private AdStarter _adStarter;
    [SerializeField] private Transform _moneyView;
    [SerializeField] private float _moveTime;
    [SerializeField] private int _burstVolume;

    private int _spawnSpace = 150;

    private void Start()
    {
        Initialize(_spawnPrefab);
    }

    private void OnEnable()
    {
        _adStarter.AdClose += OnRewardReceived;
    }

    private void OnDisable()
    {
        _adStarter.AdClose -= OnRewardReceived;
    }

    private void OnRewardReceived()
    {
        SpawnCoin(_burstVolume);
    }

    private void SpawnCoin(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-_spawnSpace, _spawnSpace), Random.Range(-_spawnSpace, _spawnSpace));
            if (TryGetObject(out GameObject coin))
            {
                coin.transform.position = transform.position + offset;
                coin.SetActive(true);
                StartCoroutine(MoveCoin(coin, _moneyView.position));
            }
        }
    }

    private IEnumerator MoveCoin(GameObject coin, Vector3 position)
    {
        WaitForSeconds delay = new WaitForSeconds(_moveTime);
        
        coin.transform.DOMove(position, _moveTime);
        yield return delay;
        coin.SetActive(false);
        CoinDelivered?.Invoke();
    }
}
