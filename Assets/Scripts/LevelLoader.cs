using GameAnalyticsSDK;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private ProgressTracker _progressTracker;

    private float _delayTime = 0.5f;

    private const string _playerProgress = "PlayerProgress";

    public void LoadHub()
    {
        StartCoroutine(LoadSceneWithDelay(1));
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = PlayerPrefs.GetInt(_playerProgress, 0) + 2;

        if(SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            StartCoroutine(LoadSceneWithDelay(nextSceneIndex));
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, nextSceneIndex.ToString());
        }
        else
        {
            nextSceneIndex = Random.Range(SceneManager.sceneCountInBuildSettings - 5, SceneManager.sceneCountInBuildSettings - 1);
            //nextSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
            StartCoroutine(LoadSceneWithDelay(nextSceneIndex));
        }
    }

    private void OnEnable()
    {
        if(_progressTracker != null)
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

    private void SaveProgress()
    {
        int currentLevelNumber = SceneManager.GetActiveScene().buildIndex - 1;

        if(currentLevelNumber >= PlayerPrefs.GetInt(_playerProgress, 0))
        {
            PlayerPrefs.SetInt(_playerProgress, currentLevelNumber);
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
