using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Wall : Interactable, IUpgradable
{
    public float Painted => _painted;
    public Color Color => _color;

    [SerializeField] private ParticleSystem _coinEffect;
    [SerializeField] private int _defaultReward;
    [SerializeField] private float _secondsBetweenPaint;
    [SerializeField] private float _paintingStep;
    [SerializeField] private SpriteRenderer _paintTexture;
    [SerializeField] private SpriteRenderer _stripe;
    [SerializeField] private Color _color;
    [SerializeField] private Vector2 _endSize;
    [SerializeField] private Vector3 _particlesOffset;

    [Range(0, 1)]
    private float _painted;
    private int _reward;
    private Vector2 _startSize;
    private Coroutine _painting;

    private const string _paintingReward = "Reward";

    public event UnityAction WallPainted;

    public void SetUpgrades()
    {
        _reward = PlayerPrefs.GetInt(_paintingReward, _defaultReward);
    }

    public void SetColor(Color color)
    {
        _color = color;

        _paintTexture.color = _color;
        _stripe.color = _color;
    }

    private void Awake()
    {
        _startSize = _paintTexture.size;
        SetUpgrades();
    }

    private IEnumerator Paintig()
    {
        WaitForSeconds waitingTime = new WaitForSeconds(_secondsBetweenPaint);
        if (_paintTexture.gameObject.activeSelf == false)
        {
            _paintTexture.gameObject.SetActive(true);
        }

        for (float i = _painted; i <= 1; i += _paintingStep)
        {
            if (Painter.TrySpendPaint() == false)
            {
                break;
            }
            else
            {
                _painted += _paintingStep;
                _paintTexture.size = Vector2.Lerp(_startSize, _endSize, Painted);
            }
            yield return waitingTime;
        }
    }

    protected override void Interact()
    {
        if(_painting == null)
        {
            _painting = StartCoroutine(Paintig());
        }
    }

    protected override void StopInteract()
    {
        if (_painting != null)
        {
            if (_painted >= 1)
            {
                _stripe.enabled = false;
                WallPainted?.Invoke();
                _coinEffect.Play();
                Painter.PlayerWallet.AddMoney(_reward);
            }

            StopCoroutine(_painting);
            _startSize = _paintTexture.size;
            _painting = null;
        }
    }
}
