using AutoBattle.Types;

namespace AutoBattle.Entities.CharacterClasses
{
    public class Paladin : Character
    {
        public Paladin(string name, float health) 
            : base(name, health)
        {
            SetSkills(new CharacterSkills
            {
                BaseDamage = 10,
                DamageMultiplier = 1f,
                KnockBackPercentChance = 30
            });
        }
    }
}