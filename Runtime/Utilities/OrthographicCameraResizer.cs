using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drolegames.Utils
{
    [RequireComponent(typeof(Camera))]
    public class OrthographicCameraResizer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The default orthographic size in unity units")]
        private float _defaultSize = 1f;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            UpdateFromWidthUnits(_defaultSize);
        }

        public void UpdateFromWidthUnits(float unityUnits)
        {
            _camera.orthographicSize = unityUnits * Screen.height / Screen.width * 0.5f;
        }
        public void UpdateFromHeightUnits(float unityUnits)
        {
            _camera.orthographicSize = unityUnits * 0.5f;
        }
    }
}