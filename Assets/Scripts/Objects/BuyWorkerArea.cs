using UnityEngine;

public class BuyWorkerArea : BuyArea
{
    private const string _workersCount = "WorkersCount";

    protected override void UnlockObject(bool firstUnlock)
    {
        base.UnlockObject(firstUnlock);

        if (firstUnlock)
        {
            PlayerPrefs.SetInt(_workersCount, PlayerPrefs.GetInt(_workersCount) + 1);
        }
    }
}
