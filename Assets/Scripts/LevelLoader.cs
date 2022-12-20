using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private ProgressTracker _progressTracker;

    private const string _playerProgress = "PlayerProgress";

    public void LoadHub()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = PlayerPrefs.GetInt(_playerProgress, 1) + 1;

        if(SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
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
    }
}
