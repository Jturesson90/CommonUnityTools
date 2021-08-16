using UnityEngine;

namespace Drolegames.BoardGame
{
    public abstract class BoardBase<T> where T : TileBase
    {
        public readonly Vector2Int size;
        public abstract T[,] Tiles { get; protected set; }

        protected BoardBase(Vector2Int size)
        {
            this.size = size;
        }

        public abstract T GetTile(Vector2Int position);
        public abstract T GetTile(int x, int y);
        protected abstract void CreateTiles();
        //TODO Remove abstract
        protected abstract bool IsInsideGrid(Vector2Int position);

        public readonly Vector2Int[] directDirections = {
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
        };

        public readonly Vector2Int[] diagonalDirections = {
            new Vector2Int(1, 1),
            new Vector2Int(-1,1),
            new Vector2Int(-1, -1),
            new Vector2Int(1, -1)
        };

        public readonly Vector2Int[] allDirections = {
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),

            new Vector2Int(1, 1),
            new Vector2Int(-1,1),
            new Vector2Int(-1, -1),
            new Vector2Int(1, -1)
        };
    }
}