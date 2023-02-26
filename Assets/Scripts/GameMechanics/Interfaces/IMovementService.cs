using GameData;

namespace GameLogic
{
    public interface IMovementService
    {
        public void MoveObject(IMovable objMovable, IField field);
        public void MoveObjectWithReflection(IMovable objMovable, TileType borderTile, IField field);
    }
}