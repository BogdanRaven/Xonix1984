using UnityEngine;

namespace GameData
{
    public interface IField
    {
        public int Height { get; }
        public int Width { get; }
        public void SetTile(Vector2Int position, TileType tileType);
        public TileType GetTile(Vector2Int position);
    }
}