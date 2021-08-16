using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Drolegames.UI
{
    public class GraphicBlink : MonoBehaviour
    {
        public Graphic graphic;
        public new SpriteRenderer renderer;
        public Ease ease = Ease.InOutQuart;

        private Tween _tween;
        private void Start()
        {
            if (graphic)
            {
                _tween = graphic.DOFade(0, 1).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
            }else if (renderer)
            {
                _tween = renderer.DOFade(0, 1).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
            }

        }
        private void OnDestroy()
        {
            _tween.Kill();
        }
    }
}
