﻿using System;
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
        public Character Target { get; set; }
        public bool IsDead { get; set; }
        public GridBox CurrentBox { get; set; }

        public Character(string name, float health)
        {
            Name = name;
            Health = health;
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
                if(this.CurrentBox.Position.X > Target.CurrentBox.Position.X)
                {
                    MoveTo(grid, Directions.LEFT);
                } 
                else if(CurrentBox.Position.X < Target.CurrentBox.Position.X)
                {
                    MoveTo(grid, Directions.RIGHT);
                }
                else if (this.CurrentBox.Position.Y > Target.CurrentBox.Position.Y)
                {
                    MoveTo(grid, Directions.UP);
                }
                else if(this.CurrentBox.Position.Y < Target.CurrentBox.Position.Y)
                {
                    MoveTo(grid, Directions.DOWN);
                }
            }
        }

        public void MoveTo(Grid grid, Directions direction)
        {
            Vector2 moveDirection = GetDirection(direction);
            Vector2 finalPosition = new Vector2(CurrentBox.Position.X + moveDirection.X, CurrentBox.Position.Y + moveDirection.Y);
            finalPosition = grid.ValidateMovement(finalPosition);
            grid.SetOccupied(CurrentBox.Position, false);
            grid.SetOccupied(finalPosition, true);
            CurrentBox = grid.GetTileAt(finalPosition);
            Console.WriteLine($"{Name} walked {direction}\n");
            grid.DrawBattlefield();
        }

        private bool CheckCloseTargets(Grid grid)
        {
            float x = CurrentBox.Position.X;
            float y = CurrentBox.Position.Y;
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
            return grid.Tiles[(int)coord.X, (int)coord.Y].IsOccupied && coord != CurrentBox.Position;
        }

        public void Attack (Grid grid, Character target)
        {
            var rand = new Random();
            int randomDamage = (int)Math.Ceiling(rand.Next(0, (int)Skills.BaseDamage) * Skills.DamageMultiplier);
            target.TakeDamage(randomDamage);
            Console.WriteLine($"{Name} is attacking the {Target.Name} and did {randomDamage} damage");
            Console.WriteLine($"{Name} Health: {Health} / {Target.Name} Health: {Target.Health}");

            int knowBackChance = rand.Next(0, 101);

            if(knowBackChance <= Skills.KnockBackPercentChance)
            {
                Vector2 position = new Vector2(CurrentBox.Position.X, CurrentBox.Position.Y);
                Vector2 targetPosition = new Vector2(target.CurrentBox.Position.X, target.CurrentBox.Position.Y);
                Vector2 direction = (targetPosition - position);
                Console.WriteLine($"{Target.Name} got knocked back!");
                target.MoveTo(grid, GetDirection(direction));
            }

            Console.WriteLine();
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

        private Directions GetDirection(Vector2 direction)
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