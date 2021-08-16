using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Drolegames.UI
{
    [RequireComponent(typeof(Canvas))]
    public class HideCanvas : MonoBehaviour
    {
        private Canvas canvas;
        private GraphicRaycaster raycaster;
        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();
        }

        private void OnBecameVisible()
        {
            SetVisible(true);
        }
        private void OnBecameInvisible()
        {
            SetVisible(false);
        }
        private void SetVisible(bool value)
        {
            canvas.enabled = value;
            if (raycaster)
                raycaster.enabled = value;
        }
    }

}
