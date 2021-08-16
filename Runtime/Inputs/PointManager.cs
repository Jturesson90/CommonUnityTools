using System;
using UnityEngine;

namespace Drolegames.Inputs
{
    public class PointManager : MonoBehaviour
    {
        /// <summary>
        /// Event fired when the user presses on the screen.
        /// </summary>
        public event Action<Vector2, double> Pressed;

        /// <summary>
        /// Event fired when the user presses on the screen.
        /// </summary>
        public event Action<Vector2, double> LongPressed;

        /// <summary>
        /// Event fired as the user drags along the screen.
        /// </summary>
        public event Action<DraggedArgs> Dragged;

        /// <summary>
        /// Event fired when the user releases a press.
        /// </summary>
        public event Action<Vector2, double> Released;


        /// <summary>
        /// Event fired when the user releases a press.
        /// </summary>
        public event Action<float, double> ScrolledOrPinched;

        [Header("Standalone Input")]
        public float draggedThreshold = 0.1f;
        [Tooltip("Seconds when the click is considered longpress")]
        public float longPressThreshold = 1f;

        private Camera _mainCamera;
        public Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        void Update()
        {
            if (MainCamera == null) return;
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
            StandaloneInput();
#elif UNITY_ANDROID || UNITY_IOS
        TouchInput();
#endif
        }
        float pressedTime;
        Vector2 pressedLocation;
        bool isDragging, isLongPress;

        Vector3 lastPosition;
        private void StandaloneInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Began(Input.mousePosition);
                lastPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Holding(Input.mousePosition, Input.mousePosition - lastPosition);
                lastPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Ended(Input.mousePosition);
            }

            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > float.Epsilon)
            {
                ScrolledOrPinched?.Invoke(scroll, Time.timeSinceLevelLoad);
            }
        }

        private void Began(Vector3 position)
        {
            pressedLocation = MainCamera.ScreenToViewportPoint(position);
            pressedTime = Time.timeSinceLevelLoad;
            Pressed?.Invoke(position, pressedTime);
        }
        private void Holding(Vector3 position, Vector3 deltaPosition)
        {
            if (!isDragging && !isLongPress)
            {
                if (pressedTime + longPressThreshold < Time.timeSinceLevelLoad)
                {
                    isLongPress = true;
                    LongPressed?.Invoke(position, Time.timeSinceLevelLoad);
                }
            }
            // Dragging
            if (!isDragging && !isLongPress)
            {
                if (Vector2.Distance(pressedLocation, MainCamera.ScreenToViewportPoint(position)) > draggedThreshold)
                {
                    isDragging = true;
                }
            }
            if (isDragging && !isLongPress)
            {
                Dragged?.Invoke(
                    new DraggedArgs()
                    {
                        TimeAt = Time.timeSinceLevelLoad,
                        Position = position,
                        DeltaPosition = deltaPosition
                    }
                ); ;
            }
        }
        private void Ended(Vector3 position)
        {
            if (!isLongPress && !isDragging)
            {
                Released?.Invoke(position, Time.timeSinceLevelLoad);
            }

            isDragging = false;
            isLongPress = false;
        }

        bool isDoubleTouch = false;
        private void TouchInput()
        {
            var touchCount = Input.touchCount;
            if (touchCount == 0)
            {
                isDoubleTouch = false;
                isLongPress = false;
            }
            else if (touchCount == 1)
            {
                if (!isDoubleTouch)
                {
                    var touch = Input.GetTouch(0);
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            Began(touch.position);
                            lastPosition = touch.position;
                            break;
                        case TouchPhase.Moved:
                        case TouchPhase.Stationary:
                            Vector2 s = lastPosition;
                            Holding(touch.position, touch.position - s);
                            lastPosition = touch.position;
                            break;
                        case TouchPhase.Ended:
                            Ended(touch.position);
                            break;
                    }
                }
            }
            else if (touchCount == 2)
            {
                isDoubleTouch = true;
                TouchZoom();
            }
        }
        private void TouchZoom()
        {
            Touch first = Input.GetTouch(0);
            Touch second = Input.GetTouch(1);

            Vector2 pos1 = MainCamera.ScreenToWorldPoint(first.position);
            Vector2 pos2 = MainCamera.ScreenToWorldPoint(second.position);
            Vector2 pos1b = MainCamera.ScreenToWorldPoint(first.position - first.deltaPosition);
            Vector2 pos2b = MainCamera.ScreenToWorldPoint(second.position - second.deltaPosition);

            float prevTouchDeltaMag = (pos1b - pos2b).magnitude;
            float touchDeltaMag = (pos1 - pos2).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            ScrolledOrPinched?.Invoke(deltaMagnitudeDiff, Time.timeSinceLevelLoad);
        }
    }
    public class DraggedArgs : EventArgs
    {
        public Vector2 Position { get; set; }
        public Vector2 DeltaPosition { get; set; }
        public float TimeAt { get; set; }
    }
}
