using System;
using AutoBattle.Types;
using AutoBattle.Enums;
using System.Numerics;

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
                Attack(grid, Target);
                return;
            }
            else 
            { 
                if(this.CurrentBox.Coords.X > Target.CurrentBox.Coords.X)
                {
                    MoveTo(grid, Directions.LEFT);
                } 
                else if(CurrentBox.Coords.X < Target.CurrentBox.Coords.X)
                {
                    MoveTo(grid, Directions.RIGHT);
                }
                else if (this.CurrentBox.Coords.Y > Target.CurrentBox.Coords.Y)
                {
                    MoveTo(grid, Directions.UP);
                }
                else if(this.CurrentBox.Coords.Y < Target.CurrentBox.Coords.Y)
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

        public void MoveTo(Grid grid, Directions direction)
        {
            Vector2 moveDirection = GetDirection(direction);
            Vector2 finalPosition = new Vector2(CurrentBox.Coords.X + moveDirection.X, CurrentBox.Coords.Y + moveDirection.Y);
            finalPosition = grid.ValidateMovement(finalPosition);
            grid.SetOccupied(CurrentBox.Coords, false);
            grid.SetOccupied(finalPosition, true);
            CurrentBox = grid.GetTileAt(finalPosition);
            Console.WriteLine($"{Name} walked {direction}\n");
            grid.DrawBattlefield();
        }

        private bool CheckCloseTargets(Grid grid)
        {
            float x = CurrentBox.Coords.X;
            float y = CurrentBox.Coords.Y;
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
            return grid.Tiles[(int)coord.X, (int)coord.Y].IsOccupied && coord != CurrentBox.Coords;
        }

        public void Attack (Grid grid, Character target)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            int randomDamage = (int)Math.Ceiling(rand.Next(0, (int)Skills.BaseDamage) * Skills.DamageMultiplier);
            target.TakeDamage(randomDamage);
            Console.WriteLine($"{Name} is attacking the {Target.Name} and did {randomDamage} damage");
            Console.WriteLine($"{Name} Health: {Health} / {Target.Name} Health: {Target.Health}\n");

            double knowBackChance = rand.NextDouble();

            if(knowBackChance >= 0.8f)
            {
                Vector2 position = new Vector2(CurrentBox.Coords.X, CurrentBox.Coords.Y);
                Vector2 targetPosition = new Vector2(target.CurrentBox.Coords.X, target.CurrentBox.Coords.Y);
                Vector2 direction = (targetPosition - position);
                Vector2.Normalize(direction);
                target.MoveTo(grid, GetDirection(direction));
                Console.WriteLine($"{Target.Name} got knocked back!");
            }
        }

        private Directions GetDirection(System.Numerics.Vector2 direction)
        {
            Directions result = Directions.LEFT;

            if (direction.X == 1) result = Directions.RIGHT;
            if (direction.X == -1) result = Directions.LEFT;
            if (direction.Y == 1) result = Directions.DOWN;
            if (direction.Y == -1) result = Directions.UP;

            return result;
        }
    }
}