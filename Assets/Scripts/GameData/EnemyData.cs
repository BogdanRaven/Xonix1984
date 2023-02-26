using System;

namespace GameData
{
    [Serializable]
    public class EnemyData
    {
        public EnemyType EnemyType {  get; private set; }

        public EnemyData(EnemyType enemyType)
        {
            EnemyType = enemyType;
        }
    }
}