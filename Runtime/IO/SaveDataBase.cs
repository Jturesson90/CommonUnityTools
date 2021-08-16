using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drolegames.IO
{
    [Serializable]
    public abstract class SaveDataBase<T> where T : class
    {
        public T data;
        public static T FromBytes(byte[] bits) => bits != null ? FromString(System.Text.ASCIIEncoding.Default.GetString(bits)) : null;
        public static T FromString(string s) => JsonUtility.FromJson<T>(s);
        public byte[] ToBytes() => System.Text.ASCIIEncoding.Default.GetBytes(ToString());
        public override string ToString() => JsonUtility.ToJson(this, false);
        public readonly string key = string.Empty;
        public SaveDataBase(T data, string key = "")
        {
            this.data = data;
            this.key = key.Equals(string.Empty) ? key : new Guid().ToString();
        }
        protected static bool HasData(string key)
        {
            var s = PlayerPrefs.GetString(key, string.Empty);
            if (s == null || s.Trim().Length == 0)
            {
                return false;
            }
            return true;
        }
        protected void LoadFromDisk()
        {
            var s = PlayerPrefs.GetString(key, string.Empty);
            if (s != null && s.Trim().Length > 0)
            {
                data = FromString(s);
            }
        }

        protected void SaveToDisk()
        {
            var json = data.ToString();
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }
    }
}
