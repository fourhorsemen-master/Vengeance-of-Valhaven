using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableAbility
{
    [SerializeField] private SerializableGuid id;
    [SerializeField] private string displayName;
    [SerializeField] private AbilityType abilityType;
    [SerializeField] private int damage;
    [SerializeField] private Rarity rarity;
    [SerializeField] private List<Empowerment> empowerments;

    public SerializableGuid ID { get => id; set => id = value; }
    public string DisplayName { get => displayName; set => displayName = value; }
    public AbilityType Type { get => abilityType; set => abilityType = value; }
    public int Damage { get => damage; set => damage = value; }
    public Rarity Rarity { get => rarity; set => rarity = value; }
    public List<Empowerment> Empowerments { get => empowerments; set => empowerments = value; }
}
