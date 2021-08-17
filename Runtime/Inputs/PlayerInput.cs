using System;
using System.Collections.Generic;
using Drolegames.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Drolegames.Inputs
{
    public class PlayerInput : Singleton<PlayerInput>
    {
        [Header("Manager")]
        [SerializeField] private PointManager manager;

        [Tooltip("Click and Secondary will be triggered by IClickable")]
        [SerializeField] private LayerMask _layerMaskToHit = -1;

        public event EventHandler<OnDraggedArgs> OnDragged;
        public event EventHandler<OnZoomedArgs> OnZoomed;
        public static event Action<OnClickedArgs> OnPressed;

        private void OnEnable()
        {
            manager.Pressed += PointManager_Pressed;
            manager.Dragged += PointManager_Dragged;
            manager.Released += PointManager_Released;
            manager.LongPressed += PointManager_LongPressed;
            manager.ScrolledOrPinched += PointManager_Zoomed;
        }

        private void OnDisable()
        {
            manager.Pressed -= PointManager_Pressed;
            manager.Dragged -= PointManager_Dragged;
            manager.Released -= PointManager_Released;
            manager.LongPressed -= PointManager_LongPressed;
            manager.ScrolledOrPinched -= PointManager_Zoomed;
        }

        private RaycastHit? GetRaycastHit(Vector2 point)
        {
            if (IsPointerOverUIObject()) return null;
            var ray = manager.MainCamera.ScreenPointToRay(point);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMaskToHit))
            {
                return hit;
            }
            return null;
        }

        private void ClickedTransform(Transform clickedTransform, Vector3 worldSpacePosition)
        {
            if (!clickedTransform) return;
            IClickable tileInterface = (IClickable)clickedTransform.GetComponent(typeof(IClickable));
            tileInterface?.OnClick(worldSpacePosition);
        }

        private void SecondaryClickedTransform(Transform clickedTransform, Vector3 worldSpacePosition)
        {
            if (!clickedTransform) return;
            IClickable tileInterface = (IClickable)clickedTransform.GetComponent(typeof(IClickable));
            tileInterface?.OnSecondaryClick(worldSpacePosition);
        }

        public static bool IsPointerOverUIObject()
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }


        private void PointManager_Zoomed(float arg1, double arg2)
        {
            OnZoomed?.Invoke(this, new OnZoomedArgs() { Amount = arg1 });
        }

        private void PointManager_LongPressed(Vector2 screenPosition, double arg2)
        {
            if (IsPointerOverUIObject()) return;
            var raycastHit = GetRaycastHit(screenPosition);
            if (raycastHit.HasValue)
            {
                Vector3 worldSpacePosition = raycastHit.Value.point;
                var clickedTransform = raycastHit.Value.transform;
                SecondaryClickedTransform(clickedTransform, worldSpacePosition);
            }
        }

        private void PointManager_Released(Vector2 screenPosition, double arg2)
        {
            if (IsPointerOverUIObject()) return;
            var raycastHit = GetRaycastHit(screenPosition);
            if (raycastHit.HasValue)
            {
                Vector3 worldSpacePosition = raycastHit.Value.point;
                var clickedTransform = raycastHit.Value.transform;
                ClickedTransform(clickedTransform, worldSpacePosition);
            }
        }

        private Vector2 deltaPressedViewportPoint;
        private void PointManager_Dragged(DraggedArgs args)
        {
            var screenPosition = args.Position;
            Vector2 draggedViewportPoint = manager.MainCamera.ScreenToWorldPoint(screenPosition);

            deltaPressedViewportPoint = manager.MainCamera.ScreenToWorldPoint(args.Position - args.DeltaPosition);

            var heading = draggedViewportPoint - deltaPressedViewportPoint;

            var distance = heading.magnitude;
            var direction = heading / distance;

            OnDragged?.Invoke(this, new OnDraggedArgs { Direction = heading });

            deltaPressedViewportPoint = draggedViewportPoint;
        }

        private void PointManager_Pressed(Vector2 screenPosition, double arg2)
        {
            deltaPressedViewportPoint = manager.MainCamera.ScreenToWorldPoint(screenPosition);
            OnPressed?.Invoke(new OnClickedArgs(screenPosition, deltaPressedViewportPoint));
        }
    }

    public class OnDraggedArgs : EventArgs
    {
        public Vector2 Direction { get; set; }
    }
    public struct OnClickedArgs
    {
        public Vector2 ScreenPosition { get; set; }
        public Vector2 WorldPosition { get; set; }
        public OnClickedArgs(Vector2 screenPosition, Vector2 worldPosition)
        {
            this.ScreenPosition = screenPosition;
            this.WorldPosition = worldPosition;
        }
    }
    public class OnZoomedArgs : EventArgs
    {
        /// <summary>
        /// Will be between [-0.1f or 0.1f]
        /// </summary>
        public float Amount { get; set; }
    }

}