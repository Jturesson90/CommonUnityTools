using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drolegames.Utils
{
    [RequireComponent(typeof(Image))]
    public class ImageChanger : MonoBehaviour
    {
        [SerializeField] private Sprite _trueImage = null;
        [SerializeField] private Sprite _falseImage = null;
        [Space()]
        [SerializeField] private float _trueAlpha = 1f;
        [SerializeField] private float _falseAlpha = 0f;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void SetImage(bool condition)
        {
            if (condition)
            {
                _image.sprite = _trueImage;
            }
            else
            {
                _image.sprite = _falseImage;
            }
        }
        public void ChangeAlpha(bool condition)
        {
            if (condition)
            {
                SetAlpha(_trueAlpha);
            }
            else
            {
                SetAlpha(_falseAlpha);
            }
        }
        private void SetAlpha(float alpha)
        {
            var color = _image.color;
            color.a = alpha;
            _image.color = color;
        }
    }
}
