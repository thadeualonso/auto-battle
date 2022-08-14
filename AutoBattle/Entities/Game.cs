using System;
using System.Collections.Generic;

namespace AutoBattle.Entities
{
    class Game
    {
        public Character Player { get; private set; }
        public Character Enemy { get; private set; }

        private readonly List<Character> allPlayers;
        private Grid grid;

        public Game(Character playerCharacter, Character enemyCharacter)
        {
            Player = playerCharacter;
            Enemy = enemyCharacter;
            allPlayers = new List<Character>();
        }

        public void StartGame()
        {
            Console.WriteLine("----- CREATING BATTLEFIELD -----");
            grid = new Grid(5, 5);
            SetFirstPlayer();

            foreach (var character in allPlayers)
            {
                grid.RandomlyPlaceCharacterOnGrid(character);
            }

            Console.WriteLine("> Characters placed on grid!");
            grid.DrawBattlefield();
            Console.WriteLine("Press any key to start battle!");
            Console.ReadKey();

            StartTurn();
        }

        private void SetFirstPlayer()
        {
            var rand = new Random();
            if (rand.Next(0, 2) == 0)
            {
                allPlayers.Add(Player);
                allPlayers.Add(Enemy);
            }
            else
            {
                allPlayers.Add(Enemy);
                allPlayers.Add(Player);
            }
        }

        private void StartTurn()
        {
            do
            {
                foreach (Character character in allPlayers) 
                    character.StartTurn(grid);
            } while (!HandleTurn());
        }

        private bool HandleTurn()
        {
            bool isGameOver = false;

            if (Player.IsDead)
            {
                Console.WriteLine("GAME OVER - Player lost!");
                Console.ReadLine();
                isGameOver = true;
            }
            else if (Enemy.IsDead)
            {
                Console.WriteLine("GAME OVER - Player won!");
                Console.ReadLine();
                isGameOver = true;
            }
            else
            {
                Console.WriteLine("Click on any key to start the next turn...\n");
                Console.Write(Environment.NewLine);

                Console.ReadKey();
                isGameOver = false;
            }

            return isGameOver;
        }
    }
}
