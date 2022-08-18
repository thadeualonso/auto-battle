using AutoBattle.Entities.CharacterClasses;
using AutoBattle.Enums;
using System;
using System.Collections.Generic;

namespace AutoBattle.Entities
{
    class Game
    {
        private const int INITIAL_HEALTH = 100;
        private const int GRID_HEIGHT = 1;
        private const int GRID_WIDTH = 3;

        public Character Player { get; private set; }
        public Character Enemy { get; private set; }


        private readonly List<Character> allPlayers;
        private Grid grid;

        public Game()
        {
            allPlayers = new List<Character>();
        }

        public void SetEnemyClass(CharacterClass enemyClass)
        {
            Enemy = GetCharacterClass("Enemy", INITIAL_HEALTH, enemyClass);
        }

        public void SetPlayerClass(CharacterClass playerClass)
        {
            Player = GetCharacterClass("Player", INITIAL_HEALTH, playerClass);
        }

        public void StartGame()
        {
            Console.WriteLine("----- CREATING BATTLEFIELD -----");
            grid = new Grid(GRID_HEIGHT, GRID_WIDTH);
            Player.Target = Enemy;
            Enemy.Target = Player;

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

        private Character GetCharacterClass(string name, int initialHealth, CharacterClass characterClass)
        {
            return characterClass switch
            {
                CharacterClass.Paladin => new Paladin(name, initialHealth),
                CharacterClass.Warrior => new Warrior(name, initialHealth),
                CharacterClass.Cleric => new Cleric(name, initialHealth),
                CharacterClass.Archer => new Archer(name, initialHealth),
                _ => new Character(name, initialHealth),
            };
        }
        private void SetFirstPlayer()
        {
            var rand = new Random();
            int minValue = 0;
            int maxValue = 2;

            if (rand.Next(minValue, maxValue) == 0)
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
            bool isGameOver;

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
