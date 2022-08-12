using AutoBattle.Enums;

namespace AutoBattle.Types
{
    public struct CharacterClassSpecific
    {
        public CharacterClass CharacterClass { get; set; }
        public float HpModifier { get; set; }
        public float ClassDamage { get; set; }
        public CharacterSkills[] Skills { get; set; }
    }
}