using System.Collections.Generic;
using System.Linq;

public class Ability
{
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
    public string DisplayName { get; private set; }
    public AbilityType Type { get; private set; }
    public int Damage { get; private set; }
    public Rarity Rarity { get; private set; }
    public List<Empowerment> Empowerments { get; private set; }

    public SerializableAbility Serialize()
    {
        return new SerializableAbility
        {
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
        (
            ability.DisplayName,
            ability.Type,
            ability.Damage,
            ability.Rarity,
            ability.Empowerments
        );
    }
}
