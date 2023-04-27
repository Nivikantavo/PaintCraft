using UnityEngine;

public class BuyWorkerArea : BuyArea
{
    private const string WorkersCount = "WorkersCount";

    protected override void UnlockObject(bool firstUnlock)
    {
        base.UnlockObject(firstUnlock);

        if (firstUnlock)
        {
            PlayerPrefs.SetInt(WorkersCount, PlayerPrefs.GetInt(WorkersCount) + 1);
        }
    }
}
