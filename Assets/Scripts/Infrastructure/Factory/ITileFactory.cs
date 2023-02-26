using GameData;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace XonixFactory
{
    public interface ITileFactory
    {
        public void Load();
        public TileBase Get(TileType tileType);
    }
}