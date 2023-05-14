using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class Audio : MonoBehaviour
{
    [SerializeField] private string _soundType;
    [SerializeField] private string _audioEnabling;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private Sprite _onSprite;

    private Button _soundButton;
    private Image _image;
    private bool _soundEnabled;
    private int _maxVolume = 0;
    private float _minVolume = -80;


    private void Awake()
    {
        _soundButton = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        CheckSoundOn();
    }

    private void OnEnable()
    {
        _soundButton.onClick.AddListener(OnSoundButtonClick);
    }

    private void OnDisable()
    {
        _soundButton.onClick.RemoveListener(OnSoundButtonClick);

        if (_soundEnabled)
        {
            PlayerPrefs.SetInt(_audioEnabling, 1);
        }
        else
        {
            PlayerPrefs.SetInt(_audioEnabling, 0);
        }
    }

    private void OnSoundButtonClick()
    {
        if (_soundEnabled)
        {
            SoundOff();
        }
        else
        {
            SoundOn();
        }
    }

    private void CheckSoundOn()
    {
        if (PlayerPrefs.GetInt(_audioEnabling, 1) == 1)
        {
            SoundOn();
        }
        else
        {
            SoundOff();
        }
    }
    private void SoundOn()
    {
        _audioMixer.SetFloat(_soundType, _maxVolume);
        _image.sprite = _onSprite;
        _soundEnabled = true;
    }

    private void SoundOff()
    {
        _audioMixer.SetFloat(_soundType, _minVolume);
        _image.sprite = _offSprite;
        _soundEnabled = false;
    }
}
