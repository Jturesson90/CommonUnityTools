using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drolegames.Utils
{
    public static class CameraExtensions
    {
        public static Vector2 BoundsMin(this Camera camera)
        {
            return (Vector2)camera.transform.position - camera.Extents();
        }

        public static Vector2 BoundsMax(this Camera camera)
        {
            return (Vector2)camera.transform.position + camera.Extents();
        }
        public static Vector3 Center(this Camera camera)
        {
            return camera.transform.position;
        }
        public static Vector2 Extents(this Camera camera)
        {
            if (camera.orthographic)
                return new Vector2(camera.orthographicSize * Screen.width / Screen.height, camera.orthographicSize);
            else
            {
                Debug.LogError("Camera is not orthographic!", camera);
                return new Vector2();
            }
        }
    }
}
