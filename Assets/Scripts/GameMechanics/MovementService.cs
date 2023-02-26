using GameData;
using UnityEngine;

namespace GameLogic
{
    public class MovementService : IMovementService
    {
        public void MoveObject(IMovable objMovable, IField field)
        {
            Vector2Int newPosition = new Vector2Int(objMovable.Position.x + objMovable.Direction.x,
                objMovable.Position.y + objMovable.Direction.y);
            if (newPosition.x < 0) newPosition = new Vector2Int(0, newPosition.y);
            if (newPosition.y < 0) newPosition = new Vector2Int(newPosition.x, 0);
            if (newPosition.x > field.Width - 1) newPosition = new Vector2Int(field.Width - 1, newPosition.y);
            if (newPosition.y > field.Height - 1) newPosition = new Vector2Int(newPosition.x, field.Height - 1);

            objMovable.SetPosition(newPosition);
        }

        public void MoveObjectWithReflection(IMovable objMovable, TileType borderTile, IField field)
        {
            Vector2Int newPosition = new Vector2Int(objMovable.Position.x + objMovable.Direction.x,
                objMovable.Position.y + objMovable.Direction.y);

            if (field.GetTile(new Vector2Int(newPosition.x, objMovable.Position.y)) == borderTile ||
                field.GetTile(new Vector2Int(newPosition.x, objMovable.Position.y)) == TileType.Border)
            {
                objMovable.SetDirection(new Vector2Int(-objMovable.Direction.x, objMovable.Direction.y));
            }

            if (field.GetTile(new Vector2Int(objMovable.Position.x, newPosition.y)) == borderTile ||
                field.GetTile(new Vector2Int(objMovable.Position.x, newPosition.y)) == TileType.Border)
            {
                objMovable.SetDirection(new Vector2Int(objMovable.Direction.x, -objMovable.Direction.y));
            }

            Vector2Int newPosition2 = new Vector2Int(objMovable.Position.x + objMovable.Direction.x,
                objMovable.Position.y + objMovable.Direction.y);

            objMovable.SetPosition(newPosition2);
        }
    }
}