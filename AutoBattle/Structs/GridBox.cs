using System.Numerics;

namespace AutoBattle.Types
{
    public struct GridBox
    {
        public Vector2 Coords { get; set; }
        public bool IsOccupied { get; set; }
        public int Index { get; private set; }

        public GridBox(int x, int y, bool isOccupied, int index)
        {
            Coords = new Vector2(x, y);
            IsOccupied = isOccupied;
            Index = index;
        }
    }
}
