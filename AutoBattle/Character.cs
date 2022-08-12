using System;
using AutoBattle.Types;
using AutoBattle.Enums;

namespace AutoBattle.Entities
{
    public class Character
    {
        public string Name { get; private set; }
        public float Health { get; set; }
        public float BaseDamage { get; set; }
        public float DamageMultiplier { get; private set; }
        public GridBox CurrentBox;
        public int PlayerIndex { get; set; }
        public Character Target { get; set; } 

        public Character(CharacterClass characterClass)
        {

        }

        public bool TakeDamage(float amount)
        {
            if((Health -= BaseDamage) <= 0)
            {
                Die();
                return true;
            }
            return false;
        }

        public void Die()
        {
            //TODO >> maybe kill him?
        }

        public void WalkTo(bool canWalk)
        {

        }

        public void StartTurn(Grid battlefield)
        {
            if (CheckCloseTargets(battlefield)) 
            {
                Attack(Target);
                return;
            }
            else
            {   // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                if(this.CurrentBox.X > Target.CurrentBox.X)
                {
                    if ((battlefield.Grids.Exists(x => x.Index == CurrentBox.Index - 1)))
                    {
                        CurrentBox.IsOccupied = false;
                        battlefield.Grids[CurrentBox.Index] = CurrentBox;
                        CurrentBox = (battlefield.Grids.Find(x => x.Index == CurrentBox.Index - 1));
                        CurrentBox.IsOccupied = true;
                        battlefield.Grids[CurrentBox.Index] = CurrentBox;
                        Console.WriteLine($"Player {PlayerIndex} walked left\n");
                        battlefield.DrawBattlefield(5, 5);

                        return;
                    }
                } 
                else if(CurrentBox.X < Target.CurrentBox.X)
                {
                    CurrentBox.IsOccupied = false;
                    battlefield.Grids[CurrentBox.Index] = CurrentBox;
                    CurrentBox = (battlefield.Grids.Find(x => x.Index == CurrentBox.Index + 1));
                    CurrentBox.IsOccupied = true;
                    battlefield.Grids[CurrentBox.Index] = CurrentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked right\n");
                    battlefield.DrawBattlefield(5, 5);
                }

                if (this.CurrentBox.Y > Target.CurrentBox.Y)
                {
                    battlefield.DrawBattlefield(5, 5);
                    this.CurrentBox.IsOccupied = false;
                    battlefield.Grids[CurrentBox.Index] = CurrentBox;
                    this.CurrentBox = (battlefield.Grids.Find(x => x.Index == CurrentBox.Index - battlefield.Width));
                    this.CurrentBox.IsOccupied = true;
                    battlefield.Grids[CurrentBox.Index] = CurrentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked up\n");
                    return;
                }
                else if(this.CurrentBox.Y < Target.CurrentBox.Y)
                {
                    this.CurrentBox.IsOccupied = true;
                    battlefield.Grids[CurrentBox.Index] = this.CurrentBox;
                    this.CurrentBox = (battlefield.Grids.Find(x => x.Index == CurrentBox.Index + battlefield.Width));
                    this.CurrentBox.IsOccupied = false;
                    battlefield.Grids[CurrentBox.Index] = CurrentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked down\n");
                    battlefield.DrawBattlefield(5, 5);

                    return;
                }
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.Grids.Find(x => x.Index == CurrentBox.Index - 1).IsOccupied);
            bool right = (battlefield.Grids.Find(x => x.Index == CurrentBox.Index + 1).IsOccupied);
            bool up = (battlefield.Grids.Find(x => x.Index == CurrentBox.Index + battlefield.Width).IsOccupied);
            bool down = (battlefield.Grids.Find(x => x.Index == CurrentBox.Index - battlefield.Width).IsOccupied);

            if (left & right & up & down) 
            {
                return true;
            }
            return false; 
        }

        public void Attack (Character target)
        {
            var rand = new Random();
            target.TakeDamage(rand.Next(0, (int)BaseDamage));
            Console.WriteLine($"Player {PlayerIndex} is attacking the player {Target.PlayerIndex} and did {BaseDamage} damage\n");
        }
    }
}
