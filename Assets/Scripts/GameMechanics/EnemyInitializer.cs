using System.Collections.Generic;
using GameData;
using UnityEngine;

namespace GameLogic
{
    public class EnemyInitializer
    {
        public List<Enemy> InitializeEnemies(EnemyType enemyType, int number)
        {
            List<Enemy> enemies = new List<Enemy>();

            for (int i = 0; i < number; i++)
            {
                Enemy enemy = new Enemy(Vector2Int.zero, Vector2Int.zero, new EnemyData(enemyType));
                enemies.Add(enemy);
            }

            return enemies;
        }

        public void SetPositionEnemies(List<Enemy> enemies, Field field, TileType tileType)
        {
            foreach (var enemy in enemies)
            {
                do
                {
                    int x = Random.Range(0, field.Width);
                    int y = Random.Range(0, field.Height);
                    enemy.SetPosition(new Vector2Int(x, y));
                } while (field.GetTile(enemy.Position) != tileType);
            }
        }

        public void SetDirection(List<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                var direction = new Vector2Int();
                direction = Random.Range(0, 1) == 0
                    ? new Vector2Int(1, direction.y)
                    : new Vector2Int(-1, direction.y);

                direction = Random.Range(0, 1) == 0
                    ? new Vector2Int(direction.x, 1)
                    : new Vector2Int(direction.x, -1);

                enemy.SetDirection(direction);
            }
        }
    }
}