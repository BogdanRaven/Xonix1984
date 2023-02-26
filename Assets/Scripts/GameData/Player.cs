using System;
using UnityEngine;

namespace GameData
{
    public class Player : IMovable
    {
        public Vector2Int Direction { get; private set; }

        public Vector2Int Position { get; private set; }
        public int Lives { get; private set; }

        public bool InSafeArea { get; private set; }

        public Action<int> OnLivesChanged { get; set; }

        public Player(Vector2Int direction, Vector2Int position, int lives)
        {
            Direction = direction;
            Position = position;
            Lives = lives;
            InSafeArea = true;
        }

        public void SetPosition(Vector2Int position)
        {
            Position = position;
        }

        public void SetDirection(Vector2Int direction)
        {
            if (!IsOppositeDirectionValid(direction))
            {
                return;
            }

            Direction = direction;
        }

        public void SetSafeArea(bool inSafeAre)
        {
            InSafeArea = inSafeAre;
        }

        public void IncreaseLives(int count)
        {
            Lives += count;
            OnLivesChanged?.Invoke(Lives);
        }

        private bool IsOppositeDirectionValid(Vector2Int direction)
        {
            if (!InSafeArea && Direction == -direction)
            {
                return false;
            }

            return true;
        }
    }
}