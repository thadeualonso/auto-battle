using AutoBattle.Types;

namespace AutoBattle.Entities.CharacterClasses
{
    public class Archer : Character
    {
        public Archer(string name, float health) 
            : base(name, health)
        {
            Skills = new CharacterSkills { BaseDamage = 5, DamageMultiplier = 1.8f, KnockBackPercentChance = 10 };
        }
    }
}