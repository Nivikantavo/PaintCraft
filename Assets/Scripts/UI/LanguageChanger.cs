using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using UnityEngine;


public class LanguageChanger : MonoBehaviour
{
    [SerializeField] private LeanLocalization _leanLocalization;

    private const string En = "English";
    private const string Ru = "Russian";
    private const string Tr = "Turkish";


    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif
        yield return YandexGamesSdk.Initialize();
        string language = YandexGamesSdk.Environment.i18n.lang;

        if(language == "ru")
        {
            _leanLocalization.SetCurrentLanguage(Ru);
        }
        else if(language == "en")
        {
            _leanLocalization.SetCurrentLanguage(En);
        }
        else if (language == "tr")
        {
            _leanLocalization.SetCurrentLanguage(Tr);
        }
    }
}
