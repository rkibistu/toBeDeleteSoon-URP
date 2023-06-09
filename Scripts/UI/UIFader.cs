using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(CanvasGroup))]
public class UIFader : UIBehaviour {
    // PUBLIC MEMBERS

    public bool IsFinished => _isFinished;

    public float StartDelay;
    public EFadeDirection Direction = EFadeDirection.FadeIn;
    public float Duration = 0.5f;
    public Ease Ease = Ease.OutQuad;
    public EPlayBehavior Behaviour = EPlayBehavior.Once;
    public bool ResetOnDisable = true;
    public float FadeOutValue = 0f;

    // PRIVATE MEMBERS

    private float _resetValue;
    private float _startValue;
    private float _targetValue;
    private float _time;
    private bool _isFinished;

    private CanvasGroup _canvasGroup;

    // MONOBEHAVIOR

    protected override void Awake() {

        _canvasGroup = GetComponent<CanvasGroup>();
    }

    protected override void OnEnable() {

        if (_isFinished == false) {
            _resetValue = _canvasGroup.alpha;
            _startValue = Direction == EFadeDirection.FadeIn ? FadeOutValue : _resetValue;
            _targetValue = Direction == EFadeDirection.FadeIn ? _resetValue : FadeOutValue;
            _time = -StartDelay;

            _canvasGroup.alpha = _startValue;
        }
    }

    protected void Update() {
        if (_isFinished == true) {
            if (Behaviour == EPlayBehavior.PingPong) {
                float previousStart = _startValue;

                _startValue = _targetValue;
                _targetValue = previousStart;
                _time = 0f;
                _isFinished = false;
            }
            else if (Behaviour == EPlayBehavior.Restart) {
                _time = 0f;
                _isFinished = false;
            }
            else {
                return;
            }
        }

        _time += Time.deltaTime;

        if (_time >= Duration) {
            _time = Duration;
            _isFinished = true;
        }

        float progress = _time > 0f ? DOVirtual.EasedValue(0f, 1f, _time / Duration, Ease) : 0f;
        _canvasGroup.alpha = Mathf.Lerp(_startValue, _targetValue, progress);
    }

    protected override void OnDisable() {
        if (ResetOnDisable == true) {
            _canvasGroup.alpha = _resetValue;
            _isFinished = false;
        }
    }

    // HELPERS

    public enum EFadeDirection {
        FadeIn,
        FadeOut,
    }

    public enum EPlayBehavior {
        //None,
        Once = 1,
        Restart = 2,
        PingPong = 3,
    }
}
