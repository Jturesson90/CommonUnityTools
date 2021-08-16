using System;
using UnityEngine;

namespace Drolegames.BoardGame
{
    [Serializable]
    public class TileBase
    {
        public readonly Vector2Int position;

        public TileBase(Vector2Int position)
        {
            this.position = position;
        }
    }
}
