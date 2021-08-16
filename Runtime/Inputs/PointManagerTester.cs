using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drolegames.Inputs
{
    public class PointManagerTester : MonoBehaviour
    {
        [Header("Manager")]
        [SerializeField] private PointManager manager;
        private void OnEnable()
        {
            manager.Pressed += PointManager_Pressed;
            manager.Dragged += PointManager_Dragged;
            manager.Released += PointManager_Released;
            manager.LongPressed += PointManager_LongPressed;
            manager.ScrolledOrPinched += PointManager_Zoomed;
        }

        private void PointManager_Zoomed(float arg1, double arg2)
        {
            Debug.Log("PointManagerTester PointManager_Zoomed " + arg1);
        }

        private void PointManager_LongPressed(Vector2 arg1, double arg2)
        {
            Debug.Log("PointManagerTester PointManager_LongPressed " + arg1);
        }

        private void PointManager_Released(Vector2 arg1, double arg2)
        {
            Debug.Log("PointManagerTester PointManager_Released " + arg1);
        }

        private void PointManager_Dragged(DraggedArgs arg1)
        {
            Debug.Log("PointManagerTester PointManager_Dragged " + arg1);
        }

        private void PointManager_Pressed(Vector2 arg1, double arg2)
        {
            Debug.Log("PointManagerTester PointManager_Pressed " + arg1);
        }

        private void OnDisable()
        {
            manager.Pressed -= PointManager_Pressed;
            manager.Dragged -= PointManager_Dragged;
            manager.Released -= PointManager_Released;
            manager.LongPressed -= PointManager_LongPressed;
            manager.ScrolledOrPinched -= PointManager_Zoomed;
        }
    }
}