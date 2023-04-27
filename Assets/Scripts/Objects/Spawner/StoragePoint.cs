using UnityEngine;

public class StoragePoint : MonoBehaviour
{
    private PaintBucket _bucket;

    public int Row { get; private set; }
    public int Column { get; private set; }
    public bool IsFree
    {
        get
        {
            return !_bucket;
        }
        private set
        {
            value = !_bucket;
        }
    }

    private void OnDisable()
    {
        if (_bucket != null)
        {
            _bucket.BucketCollected -= OnBucketCollected;
        }
    }

    public void SetRow(int row)
    {
        Row = row;
    }

    public void SetColumn(int column)
    {
        Column = column;
    }

    public void SetBucket(PaintBucket newBucket)
    {
        if (_bucket != null)
        {
            _bucket.BucketCollected -= OnBucketCollected;
        }
        _bucket = newBucket;
        if(_bucket != null)
        {
            _bucket.BucketCollected += OnBucketCollected;
        }
    }

    public PaintBucket GetBucket()
    {
        return _bucket;
    }

    private void OnBucketCollected()
    {
        _bucket.BucketCollected -= OnBucketCollected;
        _bucket = null;
    }
}
