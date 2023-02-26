using System;

namespace GameLogic
{
    public class LevelMap
    {
        public int CurrentLevel { get; private set; }
        public Action<int> OnLevelChanged { get; set; }

        public LevelMap(int startLevel)
        {
            CurrentLevel = startLevel;
        }

        public void SetLevel(int level)
        {
            CurrentLevel = level;
            OnLevelChanged?.Invoke(CurrentLevel);
        }
    }
}