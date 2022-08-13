using System.Numerics;

namespace AutoBattle.Types
{
    public struct GridBox
    {
        public Vector2 Position { get; set; }
        public bool IsOccupied { get; set; }

        public GridBox(int x, int y, bool isOccupied)
        {
            Position = new Vector2(x, y);
            IsOccupied = isOccupied;
        }
    }
}
