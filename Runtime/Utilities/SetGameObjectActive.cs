using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drolegames.Utils
{
    public class SetGameObjectActive : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;

        public void TriggerObject()
        {
            if (!_gameObject) return;
            _gameObject.SetActive(!_gameObject.activeSelf);
        }
    }
}