using System;
using System.Numerics;
using AutoBattle.Types;

namespace AutoBattle.Entities
{
    public class Grid
    {
        public GridBox[,] Tiles { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        public Grid(int lines, int columns)
        {
            Height = lines;
            Width = columns;
            Tiles = new GridBox[Width, Height];
            Console.WriteLine("The battle field has been created\n");

            for (int y = 0; y < lines; y++)
            {
                for(int x = 0; x < columns; x++)
                {
                    GridBox newBox = new GridBox(x, y, false, (columns * y + x));
                    Tiles[x, y] = newBox;
                }
            }

            DrawBattlefield();
        }

        public void DrawBattlefield()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    GridBox currentgrid = Tiles[x, y];

                    if (currentgrid.IsOccupied)
                        Console.Write("[X]\t");
                    else
                        Console.Write($"[ ]\t");
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }
        public Vector2 ValidateMovement(Vector2 movement)
        {
            Vector2 result = movement;
            if (movement.X < 0) result.X = 0;
            if (movement.X >= Width) result.X = Width - 1;
            if (movement.Y < 0) result.Y = 0;
            if (movement.Y >= Height) result.Y = Height - 1;
            return result;
        }

        public void SetOccupied(Vector2 coord, bool isOccupied)
        {
            Tiles[(int)coord.X, (int)coord.Y].IsOccupied = isOccupied;
        }

        public GridBox GetTileAt(Vector2 coord)
        {
            return Tiles[(int)coord.X, (int)coord.Y];
        }

        public GridBox GetRandomTile()
        {
            int randomX = GetRandomInt(0, Width);
            int randomY = GetRandomInt(0, Height);
            return Tiles[randomX, randomY];
        }

        private int GetRandomInt(int min, int max)
        {
            var rand = new Random();
            int index = rand.Next(min, max);
            return index;
        }
    }
}