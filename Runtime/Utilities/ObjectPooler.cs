using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;

namespace Drolegames.Utils
{
    public class ObjectPooler<T> where T : Behaviour
    {
        private T _tilePrefab;
        private List<T> pool;
        private Transform _parent;

        public ObjectPooler(T tilePrefab, int startSize, Transform parent)
        {
            _parent = parent;
            pool = new List<T>();
            _tilePrefab = tilePrefab;
            for (int i = 0; i < startSize; i++)
            {
                var obj = Object.Instantiate(_tilePrefab, parent);
                obj.gameObject.SetActive(false);
                pool.Add(obj);
            }
        }

        public T Instantiate(Vector3 localPosition, Quaternion rotation, Transform parent = null)
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].isActiveAndEnabled)
                {
                    var obj = pool[i];
                    obj.transform.localPosition = localPosition;
                    obj.transform.rotation = rotation;
                    obj.transform.parent = parent == _parent ? _parent : parent;
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
            var objs = Object.Instantiate(_tilePrefab, localPosition, rotation, parent);
            objs.transform.localPosition = localPosition;
            pool.Add(objs);
            return objs;
        }
    }
}