namespace Drolegames.UI
{
    using UnityEngine;
    using DG.Tweening;
    using UnityEngine.UI;
    using System;

    [RequireComponent(typeof(RectTransform))]
    public class CustomRectTransformAnimation : MonoBehaviour
    {
        public bool shouldMove;
        public BasicTweenSettings move;
        public bool ShouldFade;
        public BasicTweenSettings fade;
        [SerializeField] private Image _fader = null;
        private RectTransform rectTransform;

        private Vector2 startPos;
        private Vector2 targetPos;
        private Tween moveTween;
        private Tween fadeTween;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            if (shouldMove)
            {
                startPos = rectTransform.anchoredPosition - move.relativeStartPosition;
                targetPos = rectTransform.anchoredPosition;
            }

            SetStart();

        }
        public void Play()
        {
            Play(move.duration, move.delay);
        }

        public void SetStart()
        {
            if (shouldMove)
            {
                rectTransform.anchoredPosition = startPos;
            }
            if (_fader)
            {
                var c = _fader.color;
                c.a = 1f;
                _fader.color = c;
            }
        }
        private void Play(float duration, float delay)
        {
            if (shouldMove)
            {
                moveTween = rectTransform
                     .DOAnchorPos(targetPos, duration)
                     .SetDelay(delay)
                     .SetEase(move.ease);
            }
            if (ShouldFade && _fader)
            {
                fadeTween = _fader
                    .DOFade(0, duration)
                    .SetDelay(delay)
                    .SetEase(fade.ease);
            }
        }
        public void Instant()
        {
            if (moveTween != null && moveTween.IsPlaying())
            {
                moveTween.Kill();
            }
            if (fadeTween != null && fadeTween.IsPlaying())
            {
                fadeTween.Kill();
            }
            Play(0, 0);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SetStart();
                Play();
            }
        }
    }
}