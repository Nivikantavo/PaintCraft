using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _startGameText;
    [SerializeField] private LeanPhrase _loading;
    [SerializeField] private LeanPhrase _start;

    private void Awake()
    {
        ChangeButtonInteractable(false);
    }

    private IEnumerator Start()
    {
#if UNITY_EDITOR
        ChangeButtonInteractable(true);
        yield return null;
#endif
#if !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize();
        ChangeButtonInteractable(true);
#endif
    }

    private void ChangeButtonInteractable(bool gameReady)
    {
        _startGameButton.interactable = gameReady;
        _startGameText.TranslationName = gameReady ? _start.name : _loading.name;
    }
}
