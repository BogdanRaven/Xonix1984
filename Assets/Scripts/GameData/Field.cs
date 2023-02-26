using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameData
{
    public class Field : IField
    {
        private TileType[,] _field;

        private int _height;
        private int _width;
        private int _waterSpace;
        private int _maxPercentLand;

        public int Height => _height;

        public int Width => _width;

        public Field(int x, int y, int waterSpace, int maxPercentLand)
        {
            _height = y;
            _width = x;
            _waterSpace = waterSpace;
            _maxPercentLand = maxPercentLand;
            Initialize();
        }

        public void SetTile(Vector2Int position, TileType tileType)
        {
            _field[position.x, position.y] = tileType;
        }

        public TileType GetTile(Vector2Int position)
        {
            if (position.x < 0 || position.y < 0 || position.x > _width - 1 || position.y > _height - 1)
                return TileType.Border;
            return _field[position.x, position.y];
        }

        public bool MovableObjectInArea(IMovable movableObject, TileType tileArea)
        {
            return GetTile(movableObject.Position) == tileArea;
        }

        public bool HasMaxPercentLand()
        {
            return GetPercentTile(TileType.Land) >= _maxPercentLand;
        }

        public void FillTrace(TileType tile)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_field[x, y] == TileType.Trace)
                    {
                        _field[x, y] = tile;
                    }
                }
            }
        }

        public void FillTraceAre()
        {
            FillTemporaryArea();

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_field[x, y] == TileType.Trace || _field[x, y] == TileType.FloodFillSmall)
                    {
                        _field[x, y] = TileType.Land;
                    }

                    if (_field[x, y] == TileType.FloodFillBig)
                    {
                        _field[x, y] = TileType.Water;
                    }
                }
            }
        }

        private int GetPercentTile(TileType tileType)
        {
            int countLandTile = 0;
            foreach (var tile in _field)
            {
                if (tile == tileType)
                {
                    countLandTile++;
                }
            }

            return (int) Mathf.Round(countLandTile * 100 / _field.Length);
        }

        private void FillTemporaryArea()
        {
            FillWaterTemporaryArea(new List<TileType>() {TileType.Water}, TileType.FloodFillBig);
            FillWaterTemporaryArea(new List<TileType>() {TileType.Water}, TileType.FloodFillSmall);

            var allTiles = _field.Cast<TileType>().ToList();

            int countSmallArea = allTiles.Count(type => type == TileType.FloodFillSmall);
            int countBigArea = allTiles.Count(type => type == TileType.FloodFillBig);
            if (countSmallArea >= countBigArea)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        if (_field[x, y] == TileType.FloodFillSmall)
                        {
                            _field[x, y] = TileType.FloodFillBig;
                        }
                        else if (_field[x, y] == TileType.FloodFillBig)
                        {
                            _field[x, y] = TileType.FloodFillSmall;
                        }
                    }
                }
            }
        }

        private void FillWaterTemporaryArea(List<TileType> allowedTileTypes, TileType floodFillType)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_field[x, y] == TileType.Water)
                    {
                        FillTemporaryArea(x, y, allowedTileTypes, floodFillType);
                        return;
                    }
                }
            }
        }

        private void FillTemporaryArea(int x, int y, List<TileType> allowedTileTypes, TileType floodFillType)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height) return;
            if (!allowedTileTypes.Contains(_field[x, y])) return;

            _field[x, y] = floodFillType;

            FillTemporaryArea(x - 1, y, allowedTileTypes, floodFillType);
            FillTemporaryArea(x + 1, y, allowedTileTypes, floodFillType);
            FillTemporaryArea(x, y - 1, allowedTileTypes, floodFillType);
            FillTemporaryArea(x, y + 1, allowedTileTypes, floodFillType);
        }

        private void Initialize()
        {
            _field = new TileType[_width, _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (x > _width - _waterSpace || x < _waterSpace - 1 || y > _height - _waterSpace ||
                        y < _waterSpace - 1)
                    {
                        _field[x, y] = TileType.Land;
                    }
                    else
                    {
                        _field[x, y] = TileType.Water;
                    }
                }
            }
        }
    }
}