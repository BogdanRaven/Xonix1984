using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.Tilemaps;
using XonixFactory;

namespace GameLogic
{
    public class MapRenderer : MonoBehaviour
    {
        [SerializeField] private Tilemap mainTileMap;
        [SerializeField] private Tilemap playerTileMap;
        [SerializeField] private Tilemap enemiesTileMap;

        private Field _field;
        private Player _player;
        private ITileFactory _tileFactory;
        private List<Enemy> _waterEnemies;
        private List<Enemy> _landEnemies;

        public void Context(Field field, Player player, List<Enemy> waterEnemies, List<Enemy> landEnemies,
            ITileFactory tileFactory)
        {
            _field = field;
            _player = player;
            _waterEnemies = waterEnemies;
            _landEnemies = landEnemies;
            _tileFactory = tileFactory;
        }

        public void DrawMainTileMap()
        {
            for (int y = 0; y < _field.Height; y++)
            {
                for (int x = 0; x < _field.Width; x++)
                {
                    mainTileMap.SetTile(new Vector3Int(x, y), _tileFactory.Get(_field.GetTile(new Vector2Int(x, y))));
                }
            }
        }

        public void DrawPlayerTileMap()
        {
            playerTileMap.ClearAllTiles();
            playerTileMap.SetTile(new Vector3Int(_player.Position.x, _player.Position.y),
                _tileFactory.Get(TileType.Player));
        }

        public void DrawEnemiesTileMap()
        {
            enemiesTileMap.ClearAllTiles();
            DrawEnemiesTile(_waterEnemies);
            DrawEnemiesTile(_landEnemies);
        }

        private void DrawEnemiesTile(IEnumerable<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                TileType enemyTileType = TileType.WaterEnemy;
                switch (enemy.EnemyData.EnemyType)
                {
                    case EnemyType.Water:
                        enemyTileType = TileType.WaterEnemy;
                        break;
                    case EnemyType.Land:
                        enemyTileType = TileType.LandEnemy;
                        break;
                }

                enemiesTileMap.SetTile(new Vector3Int(enemy.Position.x, enemy.Position.y),
                    _tileFactory.Get(enemyTileType));
            }
        }
    }
}