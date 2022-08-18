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
            if (lines <= 1 && columns <= 1)
            {
                throw new Exception("Invalid sizes to grid!");
            }

            Height = lines;
            Width = columns;
            Tiles = new GridBox[Width, Height];
            Console.WriteLine("> The battlefield has been created\n");

            for (int y = 0; y < lines; y++)
            {
                for(int x = 0; x < columns; x++)
                {
                    GridBox newBox = new GridBox(x, y, false);
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
                        Console.Write("[ ]\t");
                }
                Console.Write("\n\n");
            }
            Console.Write("\n\n");
        }

        public void RandomlyPlaceCharacterOnGrid(Character character)
        {
            GridBox RandomLocation = GetRandomTile();

            while (RandomLocation.IsOccupied)
                RandomLocation = GetRandomTile();

            Console.Write($"> Randomly placing {character.Name} at (x {RandomLocation.Position.X} y {RandomLocation.Position.Y})\n");
            RandomLocation.IsOccupied = true;
            Tiles[(int)RandomLocation.Position.X, (int)RandomLocation.Position.Y] = RandomLocation;
            character.CurrentBox = RandomLocation;
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
            var rand = new Random();
            int randomX = rand.Next(0, Width);
            int randomY = rand.Next(0, Height);
            return Tiles[randomX, randomY];
        }
    }
}