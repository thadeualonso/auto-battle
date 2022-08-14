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

            Character PlayerCharacter = GetPlayerChoice();
            Character EnemyCharacter = CreateEnemyCharacter();
            EnemyCharacter.Target = PlayerCharacter;
            PlayerCharacter.Target = EnemyCharacter;

            Game game = new Game(PlayerCharacter, EnemyCharacter);
            game.StartGame();
        }

        public static Character GetPlayerChoice()
        {
            Console.WriteLine("Choose Between One of this Classes:\n");
            Console.Write("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer: ");
            string choice = Console.ReadLine();
            
            while (!IsValidOption(choice))
            {
                Console.Write("Invalid option! Try again: ");
                choice = Console.ReadLine();
            }

            CharacterClasses currentClass = (CharacterClasses)int.Parse(choice);
            Console.WriteLine($"> Player Class Choice: {currentClass}");
            return new Character("Player", 100, currentClass);
        }

        public static Character CreateEnemyCharacter()
        {
            var rand = new Random();
            int randomInteger = rand.Next(1, 4);
            CharacterClasses enemyClass = (CharacterClasses)randomInteger;
            Console.WriteLine($"> Enemy Class Choice: {enemyClass}");
            return new Character("Enemy", 100, enemyClass);
        }

        public static bool IsValidOption(string input)
        {
            if (!int.TryParse(input, out int option))
            {
                return false;
            }
            else
            {
                return option >= 0 && option <= 4;
            }
        }
    }
}