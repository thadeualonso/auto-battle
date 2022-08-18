using System;
using AutoBattle.Entities;
using AutoBattle.Enums;

namespace AutoBattle
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("----- AUTO BATTLE GAME -----");

            Game game = new Game();

            CharacterClass playerClass = GetPlayerChoice();
            CharacterClass enemyClass = CreateEnemyCharacter();

            game.SetPlayerClass(playerClass);
            game.SetEnemyClass(enemyClass);
            game.StartGame();
        }

        public static CharacterClass GetPlayerChoice()
        {
            Console.WriteLine("Choose Between One of this Classes:\n");
            Console.Write("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer: ");
            string choice = Console.ReadLine();
            
            while (!IsValidOption(choice))
            {
                Console.Write("Invalid option! Try again: ");
                choice = Console.ReadLine();
            }

            return GetClassByInt(Int32.Parse(choice));
        }

        public static CharacterClass CreateEnemyCharacter()
        {
            var rand = new Random();
            int randomInteger = rand.Next(1, 4);
            return GetClassByInt(randomInteger);
        }

        private static CharacterClass GetClassByInt(int randomInteger)
        {
            CharacterClass enemyClass = (CharacterClass)randomInteger;
            Console.WriteLine($"> Enemy Class Choice: {enemyClass}");
            return enemyClass;
        }

        public static bool IsValidOption(string input)
        {
            if (!int.TryParse(input, out int option))
            {
                return false;
            }
            else
            {
                return option > 0 && option <= 4;
            }
        }
    }
}