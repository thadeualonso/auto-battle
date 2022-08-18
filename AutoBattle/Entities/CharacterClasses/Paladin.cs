using AutoBattle.Types;

namespace AutoBattle.Entities.CharacterClasses
{
    public class Paladin : Character
    {
        public Paladin(string name, float health) 
            : base(name, health)
        {
            Skills = new CharacterSkills { BaseDamage = 10, DamageMultiplier = 1, KnockBackPercentChance = 30 };
        }
    }
}