using System;
using System.Collections.Generic;
using System.Linq;
using AutoBattle.Entities;
using AutoBattle.Enums;
using AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new Grid(5, 5);
            Character PlayerCharacter;
            Character EnemyCharacter;
            List<Character> AllPlayers = new List<Character>();
            int currentTurn = 0;
            int numberOfPossibleTiles = grid.Tiles.Length;
            Setup(); 

            void Setup()
            {
                GetPlayerChoice();
                CreateEnemyCharacter();
                StartGame();
            }

            void GetPlayerChoice()
            {
                Console.WriteLine("Choose Between One of this Classes:\n");
                Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
                string choice = Console.ReadLine();
                CharacterClass currentClass = new CharacterClass();

                switch (choice)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                        currentClass = (CharacterClass)Int32.Parse(choice);
                        break;
                    default:
                        GetPlayerChoice();
                        break;
                }

                PlayerCharacter = new Character("Player", 100, currentClass);
                Console.WriteLine($"Player Class Choice: {currentClass}");
            }

            void CreateEnemyCharacter()
            {
                var rand = new Random();
                int randomInteger = rand.Next(1, 4);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                Console.WriteLine($"Enemy Class Choice: {enemyClass}");
                EnemyCharacter = new Character("Enemy", 100, enemyClass);
            }

            void StartGame()
            {
                //populates the character variables and targets
                EnemyCharacter.Target = PlayerCharacter;
                PlayerCharacter.Target = EnemyCharacter;
                AllPlayers.Add(PlayerCharacter);
                AllPlayers.Add(EnemyCharacter);
                RandomlyAlocateCharacterOnGrid(PlayerCharacter);
                RandomlyAlocateCharacterOnGrid(EnemyCharacter);
                Console.WriteLine("Characters placed on grid!");
                grid.DrawBattlefield();
                Console.WriteLine("Press to start battle!");
                Console.ReadLine();
                StartTurn();
            }

            void StartTurn()
            {
                foreach(Character character in AllPlayers)
                {
                    character.StartTurn(grid);
                }

                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                if(PlayerCharacter.IsDead)
                {
                    Console.WriteLine("Player lost!");
                    Console.ReadLine();
                    return;
                } else if (EnemyCharacter.IsDead)
                {
                    Console.WriteLine("Player won!");
                    Console.ReadLine();
                    return;
                } else
                {
                    Console.WriteLine("Click on any key to start the next turn...\n");
                    Console.Write(Environment.NewLine);

                    Console.ReadKey();
                    StartTurn();
                }
            }

            void RandomlyAlocateCharacterOnGrid(Character character)
            {
                GridBox RandomLocation = grid.GetRandomTile();

                while (RandomLocation.IsOccupied)
                {
                    RandomLocation = grid.GetRandomTile();
                }

                Console.Write($"Randomly placing {character.Name} at (x {RandomLocation.X} y {RandomLocation.Y})\n");
                RandomLocation.IsOccupied = true;
                grid.Tiles[RandomLocation.X, RandomLocation.Y] = RandomLocation;
                character.CurrentBox = RandomLocation;
            }

        }
    }
}
