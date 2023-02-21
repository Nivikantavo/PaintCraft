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
        StartCoroutine(LoadSceneWithDelay(0));
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = PlayerPrefs.GetInt(_playerProgress, 0) + 1;
        Debug.Log(PlayerPrefs.GetInt(_playerProgress));

        if(SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            StartCoroutine(LoadSceneWithDelay(nextSceneIndex));
        }
        else
        {
            Debug.LogError("Next scene did`t exist");
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
        int currentLevelNumber = SceneManager.GetActiveScene().buildIndex;

        PlayerPrefs.SetInt(_playerProgress, currentLevelNumber);
        Debug.Log("save progress: "+ currentLevelNumber);
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
