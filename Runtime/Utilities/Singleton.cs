using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drolegames.Utils
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Current { get; private set; }
        public static bool IsInitialized => Current != null;

        protected virtual void Awake()
        {
            if (Current != null)
            {
                Debug.LogWarning("Destroying Multiple Singletons of same Type " + gameObject.name);
                Destroy(gameObject);
            }
            else
            {
                Current = (T)this;
            }
        }
        protected virtual void OnDestroy()
        {
            if (Current == this)
            {
                Current = null;
            }
        }
    }
}