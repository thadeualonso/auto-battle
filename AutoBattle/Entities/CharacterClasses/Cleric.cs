using AutoBattle.Types;

namespace AutoBattle.Entities.CharacterClasses
{
    public class Cleric : Character
    {
        public Cleric(string name, float health) 
            : base(name, health)
        {
            Skills = new CharacterSkills { BaseDamage = 5, DamageMultiplier = 2f, KnockBackPercentChance = 10 };
        }
    }
}