using System;
using System.Collections.Generic;
using AutoBattle.Types;

namespace AutoBattle.Entities
{
    public class Grid
    {
        public List<GridBox> Grids { get; private set; } = new List<GridBox>();
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Grid(int lines, int columns)
        {
            Width = lines;
            Height = columns;
            Console.WriteLine("The battle field has been created\n");

            for (int y = 0; y < lines; y++)
            {
                for(int x = 0; x < columns; x++)
                {
                    GridBox newBox = new GridBox(x, y, false, (columns * y + x));
                    Grids.Add(newBox);
                    Console.Write($"{newBox.Index}\n");
                }
            }
        }

        public void DrawBattlefield(int lines, int columns)
        {
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    GridBox currentgrid = new GridBox();
                    if (currentgrid.IsOccupied)
                    {
                        Console.Write("[X]\t");
                    }
                    else
                    {
                        Console.Write($"[ ]\t");
                    }
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}