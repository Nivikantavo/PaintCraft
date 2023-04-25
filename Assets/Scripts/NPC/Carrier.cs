using UnityEngine;

public class Carrier : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] private int _capacity;
    [SerializeField] private Storage[] _spawnerStorages;
    [SerializeField] private Storage[] _roomsStorages;

    private bool _isByed;
    private PaintBucket[] _currentBuckets;


    private void Update()
    {
        if (_isByed)
        {

        }
    }

    public void ApplayPay()
    {

    }

    private void FindeBuckets()
    {
        foreach (var bucket in _currentBuckets)
        {

        }
    }

    private void TakeBuckets()
    {

    }
}
