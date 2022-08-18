using AutoBattle.Types;

namespace AutoBattle.Entities.CharacterClasses
{
    public class Warrior : Character
    {
        public Warrior(string name, float health) 
            : base(name, health)
        {
            SetSkills(new CharacterSkills
            {
                BaseDamage = 15,
                DamageMultiplier = 1.2f,
                KnockBackPercentChance = 50
            });
        }
    }
}