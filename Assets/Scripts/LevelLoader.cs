using GameAnalyticsSDK;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private const string PlayerProgress = "PlayerProgress";

    [SerializeField] private ProgressTracker _progressTracker;

    private float _delayTime = 0.5f;
    private int _scensBeforeLevels = 2;
    private int _numberOfLoopedLevels = 5;

    private void OnEnable()
    {
        if (_progressTracker != null)
        {
            _progressTracker.LevelEnd += SaveProgress;
        }
    }

    private void OnDisable()
    {
        if (_progressTracker != null)
        {
            _progressTracker.LevelEnd -= SaveProgress;
        }
    }

    public void LoadHub()
    {
        StartCoroutine(LoadSceneWithDelay(1));
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = PlayerPrefs.GetInt(PlayerProgress, 0) + _scensBeforeLevels;

        if(SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            StartCoroutine(LoadSceneWithDelay(nextSceneIndex));
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, nextSceneIndex.ToString());
        }
        else
        {
            nextSceneIndex = Random.Range(SceneManager.sceneCountInBuildSettings - _numberOfLoopedLevels, SceneManager.sceneCountInBuildSettings);
            StartCoroutine(LoadSceneWithDelay(nextSceneIndex));
        }
    }

    private void SaveProgress()
    {
        int currentLevelNumber = SceneManager.GetActiveScene().buildIndex - 1;

        if(currentLevelNumber >= PlayerPrefs.GetInt(PlayerProgress, 0))
        {
            PlayerPrefs.SetInt(PlayerProgress, currentLevelNumber);
            PlayerPrefs.Save();
        }
#if (UNITY_WEBGL && !UNITY_EDITOR)
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, currentLevelNumber.ToString());
#endif
    }

    private IEnumerator LoadSceneWithDelay(int sceneNumber)
    {
        WaitForSeconds loadDelay = new WaitForSeconds(_delayTime);
        for (int i = 0; i <= 1; i++)
        {
            yield return loadDelay;
        }
        SceneManager.LoadScene(sceneNumber);
    }
}
