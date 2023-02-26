using UnityEngine;

namespace GameData
{
    public class Enemy : IMovable
    {
        public Vector2Int Direction { get; private set; }

        public Vector2Int Position { get; private set; }

        public EnemyData EnemyData { get; private set; }

        public Enemy(Vector2Int direction, Vector2Int position, EnemyData enemyData)
        {
            Direction = direction;
            Position = position;
            EnemyData = enemyData;
        }

        public void SetPosition(Vector2Int position)
        {
            Position = position;
        }

        public void SetDirection(Vector2Int direction)
        {
            Direction = direction;
        }
    }
}