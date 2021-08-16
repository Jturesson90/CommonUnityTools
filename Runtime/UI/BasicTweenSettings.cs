using DG.Tweening;
using System;
using UnityEngine;

namespace Drolegames.UI
{
    [Serializable]
    public struct BasicTweenSettings
    {
        public float delay;
        public float duration;
        public Ease ease;
        public Vector2 relativeStartPosition;
    }
}