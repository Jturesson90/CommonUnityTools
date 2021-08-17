namespace Drolegames.IO
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SaveDataBase
    {
        public double totalPlayingTime;


        public static SaveDataBase FromBytes(byte[] data) => data != null ? SaveDataBase.FromString(System.Text.ASCIIEncoding.Default.GetString(data)) : null;
        public static SaveDataBase FromString(string s) => JsonUtility.FromJson<SaveDataBase>(s);
        public byte[] ToBytes() => System.Text.ASCIIEncoding.Default.GetBytes(ToString());
        public override string ToString() => JsonUtility.ToJson(this, false);
    }
}
