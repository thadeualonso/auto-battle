using System;
using AutoBattle.Types;
using AutoBattle.Enums;

namespace AutoBattle.Entities
{
    public class Character
    {
        public string Name { get; private set; }
        public float Health { get; set; }
        public CharacterSkills Skills { get; set; }
        public int PlayerIndex { get; set; }
        public Character Target { get; set; }
        public bool IsDead { get; set; }

        public GridBox CurrentBox;

        public Character(string name, float health, CharacterClass characterClass)
        {
            Name = name;
            Health = health;

            switch (characterClass)
            {
                case CharacterClass.Paladin:
                    Skills = new CharacterSkills { BaseDamage = 10, DamageMultiplier = 1 };
                    break;
                case CharacterClass.Warrior:
                    Skills = new CharacterSkills { BaseDamage = 15, DamageMultiplier = 1.2f };
                    break;
                case CharacterClass.Cleric:
                    Skills = new CharacterSkills { BaseDamage = 5, DamageMultiplier = 2f };
                    break;
                case CharacterClass.Archer:
                    Skills = new CharacterSkills { BaseDamage = 5, DamageMultiplier = 1.8f };
                    break;
                default:
                    Console.WriteLine("Invalid class!");
                    break;
            }
        }

        public void TakeDamage(float amount)
        {
            Health -= amount;
            IsDead = Health <= 0;
        }

        public void StartTurn(Grid grid)
        {
            if (CheckCloseTargets(grid)) 
            {
                Attack(Target);
                return;
            }
            else 
            { 
                if(this.CurrentBox.X > Target.CurrentBox.X)
                {
                    MoveTo(grid, Directions.LEFT);
                } 
                else if(CurrentBox.X < Target.CurrentBox.X)
                {
                    MoveTo(grid, Directions.RIGHT);
                }
                else if (this.CurrentBox.Y > Target.CurrentBox.Y)
                {
                    MoveTo(grid, Directions.UP);
                }
                else if(this.CurrentBox.Y < Target.CurrentBox.Y)
                {
                    MoveTo(grid, Directions.DOWN);
                }
            }
        }

        private Vector2 GetDirection(Directions directions)
        {
            Vector2 result = new Vector2(0, 0);
            switch (directions)
            {
                case Directions.LEFT:
                    result.X = -1;
                    break;
                case Directions.RIGHT:
                    result.X = 1;
                    break;
                case Directions.UP:
                    result.Y = -1;
                    break;
                case Directions.DOWN:
                    result.Y = 1;
                    break;
            }
            return result;
        }

        private void MoveTo(Grid grid, Directions direction)
        {
            Vector2 moveDirection = GetDirection(direction);
            Vector2 finalPosition = new Vector2(CurrentBox.X + moveDirection.X, CurrentBox.Y + moveDirection.Y);
            finalPosition = grid.ValidateMovement(finalPosition);
            grid.SetOccupied(CurrentBox.Coords, false);
            grid.SetOccupied(finalPosition, true);
            CurrentBox = grid.GetTileAt(finalPosition);
            Console.WriteLine($"{Name} walked {direction}\n");
            grid.DrawBattlefield();
        }

        private bool CheckCloseTargets(Grid grid)
        {
            int x = CurrentBox.X;
            int y = CurrentBox.Y;
            Vector2 left = grid.ValidateMovement(new Vector2(x - 1, y));
            Vector2 right = grid.ValidateMovement(new Vector2(x + 1, y));
            Vector2 up = grid.ValidateMovement(new Vector2(x, y - 1));
            Vector2 down = grid.ValidateMovement(new Vector2(x, y + 1));

            bool hasEnemyOnLeft     = HasTargetOnTile(grid, left);
            bool hasEnemyOnRight    = HasTargetOnTile(grid, right);
            bool hasEnemyOnUp       = HasTargetOnTile(grid, up);
            bool hasEnemyOnDown     = HasTargetOnTile(grid, down);

            return hasEnemyOnDown || hasEnemyOnUp || hasEnemyOnLeft || hasEnemyOnRight;
        }

        private bool HasTargetOnTile(Grid grid, Vector2 coord)
        {
            return grid.Tiles[coord.X, coord.Y].IsOccupied && coord != CurrentBox.Coords;
        }

        public void Attack (Character target)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            int randomDamage = (int)Math.Ceiling(rand.Next(0, (int)Skills.BaseDamage) * Skills.DamageMultiplier);
            target.TakeDamage(randomDamage);
            Console.WriteLine($"{Name} is attacking the {Target.Name} and did {randomDamage} damage");
            Console.WriteLine($"{Name} Health: {Health} / {Target.Name} Health: {Target.Health}\n");
        }
    }
}