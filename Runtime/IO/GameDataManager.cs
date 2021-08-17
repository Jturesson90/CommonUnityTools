namespace Drolegames.IO
{
    using Drolegames.Utils;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameDataManager : Singleton<GameDataManager>
    {
        [SerializeField] private SaveDataBase data; // SHOULD BE A SCRIPTABLEOBJECT
        public SaveDataBase GameData => data;
    }
}