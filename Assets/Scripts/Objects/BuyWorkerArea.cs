using UnityEngine;

public class BuyWorkerArea : BuyArea
{
    private const string WorkersCount = "WorkersCount";

    protected override void UnlockObject(bool firstUnlock)
    {
        base.UnlockObject(firstUnlock);

        if (firstUnlock)
        {
            int currentWorkersCount = PlayerPrefs.GetInt(WorkersCount);
            currentWorkersCount++;
            PlayerPrefs.SetInt(WorkersCount, currentWorkersCount);
        }
    }
}
