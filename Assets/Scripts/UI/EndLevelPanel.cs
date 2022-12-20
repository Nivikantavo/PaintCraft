using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelPanel : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private ProgressTracker _progressTracker;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _hubButton;
    [SerializeField] private TMP_Text _moneyEarned;
    [SerializeField] private GameObject _joystick;

    private void OnEnable()
    {
        _moneyEarned.text = _progressTracker.MoneyEarnedPerLevel.ToString();
        _joystick.SetActive(false);
        _hubButton.onClick.AddListener(_levelLoader.LoadHub);
        _nextLevelButton.onClick.AddListener(_levelLoader.LoadNextLevel);
    }

    private void OnDisable()
    {
        _joystick.SetActive(true);
        _hubButton.onClick.RemoveListener(_levelLoader.LoadHub);
        _nextLevelButton.onClick.RemoveListener(_levelLoader.LoadNextLevel);
    }
}
