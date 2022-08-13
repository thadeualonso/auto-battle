namespace AutoBattle.Types
{
    public struct GridBox
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Vector2 Coords { get { return new Vector2(X, Y); } }
        public bool IsOccupied { get; set; }
        public int Index { get; private set; }

        public GridBox(int x, int y, bool isOccupied, int index)
        {
            X = x;
            Y = y;
            IsOccupied = isOccupied;
            Index = index;
        }
    }
}
