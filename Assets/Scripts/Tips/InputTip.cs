using Agava.YandexGames;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DeviceType = Agava.YandexGames.DeviceType;

public class InputTip : MonoBehaviour
{
    private const string PlayerProgress = "PlayerProgress";
    private const string GiveTip = "GiveTip";

    [SerializeField] private Image _cursorImage;
    [SerializeField] private Animator _animator;
    [SerializeField] private Sprite _keyboard;
    [SerializeField] private Sprite _hand;

    private bool _stopped = false;

    private void Awake()
    {
        _cursorImage.enabled = false;
        int nextSceneIndex = PlayerPrefs.GetInt(PlayerProgress, 0) + 1;

        _cursorImage.sprite = _hand;

        if (nextSceneIndex == 1)
        {
            GiveAdvice();
        }
    }

    private IEnumerator Start()
    {
#if(!UNITY_WEBGL || UNITY_EDITOR)
        yield break;
#endif
        yield return YandexGamesSdk.Initialize();

        if (Device.Type == DeviceType.Desktop)
        {
            _cursorImage.sprite = _keyboard;
            _animator.enabled = false;
        }
    }

    private void Update()
    {
        if(_stopped)
            enabled = false;

        if (Input.GetMouseButton(0) || Input.anyKeyDown)
        {
            StopAdvice();
        }
    }

    private void GiveAdvice()
    {
        _cursorImage.enabled = true;
        _animator.SetBool(GiveTip, true);
    }

    private void StopAdvice()
    {
        _cursorImage.enabled = false;
        _animator.SetBool(GiveTip, false);
        _stopped = true;
    }
}
