using System;
using AutoBattle.Types;
using AutoBattle.Enums;
using System.Numerics;

namespace AutoBattle.Entities
{
    public class Character
    {
        private const int DIRECTION = 1;

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

        protected void SetSkills(CharacterSkills skills)
        {
            Skills = skills;
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
            int offset = 1;

            Vector2 left = grid.ValidateMovement(new Vector2(x - offset, y));
            Vector2 right = grid.ValidateMovement(new Vector2(x + offset, y));
            Vector2 up = grid.ValidateMovement(new Vector2(x, y - offset));
            Vector2 down = grid.ValidateMovement(new Vector2(x, y + offset));

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
            const int MIN_VALUE = 0;

            var rand = new Random();
            int randomDamage = (int)Math.Ceiling(rand.Next(MIN_VALUE, (int)Skills.BaseDamage) * Skills.DamageMultiplier);
            target.TakeDamage(randomDamage);
            Console.WriteLine($"{Name} is attacking the {Target.Name} and did {randomDamage} damage");
            Console.WriteLine($"{Name} Health: {Health} / {Target.Name} Health: {Target.Health}");
            TryKnockBack(grid, target, rand);
            Console.WriteLine();
        }

        private void TryKnockBack(Grid grid, Character target, Random rand)
        {
            const int MIN_VALUE = 0;
            const int MAX_VALUE = 101;
            int knowBackChance = rand.Next(MIN_VALUE, MAX_VALUE);

            if (knowBackChance <= Skills.KnockBackPercentChance)
            {
                Vector2 position = new Vector2(CurrentBox.Position.X, CurrentBox.Position.Y);
                Vector2 targetPosition = new Vector2(target.CurrentBox.Position.X, target.CurrentBox.Position.Y);
                Vector2 direction = (targetPosition - position);
                Console.WriteLine($"{Target.Name} got knocked back!");
                target.MoveTo(grid, GetDirection(direction));
            }
        }

        private Vector2 GetDirection(Directions directions)
        {
            Vector2 result = Vector2.Zero;

            switch (directions)
            {
                case Directions.LEFT:
                    result.X = -DIRECTION;
                    break;
                case Directions.RIGHT:
                    result.X = DIRECTION;
                    break;
                case Directions.UP:
                    result.Y = -DIRECTION;
                    break;
                case Directions.DOWN:
                    result.Y = DIRECTION;
                    break;
            }
            return result;
        }

        private Directions GetDirection(Vector2 direction)
        {
            Directions result = Directions.LEFT;

            if (direction.X == DIRECTION) result = Directions.RIGHT;
            if (direction.X == -DIRECTION) result = Directions.LEFT;
            if (direction.Y == DIRECTION) result = Directions.DOWN;
            if (direction.Y == -DIRECTION) result = Directions.UP;

            return result;
        }
    }
}