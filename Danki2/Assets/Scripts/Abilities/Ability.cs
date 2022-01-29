using System.Collections.Generic;
using System.Linq;

public class Ability
{
    private Ability() { }

    public Ability(string displayName, AbilityType type, int damage, Rarity rarity, List<Empowerment> empowerments)
    {
        ID = SerializableGuid.NewGuid();
        DisplayName = displayName;
        Type = type;
        Damage = damage;
        Rarity = rarity;
        Empowerments = empowerments.Select(x => x).ToList();
    }

    public SerializableGuid ID { get; private set; }
    public string DisplayName { get; set; }
    public AbilityType Type { get; set; }
    public int Damage { get; set; }
    public Rarity Rarity { get; set; }
    public List<Empowerment> Empowerments { get; set; }

    public SerializableAbility Serialize()
    {
        return new SerializableAbility
        {
            ID = ID,
            DisplayName = DisplayName,
            Type = Type,
            Damage = Damage,
            Rarity = Rarity,
            Empowerments = Empowerments
        };
    }

    public static Ability FromSerializedAbility(SerializableAbility ability)
    {
        return new Ability
        {
            ID = ability.ID,
            DisplayName = ability.DisplayName,
            Type = ability.Type,
            Damage = ability.Damage,
            Rarity = ability.Rarity,
            Empowerments = ability.Empowerments
        };
    }
}
