using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drolegames.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaDrole : MonoBehaviour
    {
        [SerializeField] private RectTransform _panel;
        private Rect _lastSafeArea = new Rect(0, 0, 0, 0);


        private void Awake()
        {
            _panel = GetComponent<RectTransform>();
            Refresh();
        }

        private void Refresh()
        {
            Rect safeArea = GetSafeArea();
            if (safeArea != _lastSafeArea)
                ApplySafeArea(safeArea);
        }

        private void ApplySafeArea(Rect r)
        {
            _lastSafeArea = r;
            Vector2 anchorMin = r.position;
            Vector2 anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            _panel.anchorMin = anchorMin;
            _panel.anchorMax = anchorMax;
        }

        private Rect GetSafeArea() => Screen.safeArea;
    }
}
