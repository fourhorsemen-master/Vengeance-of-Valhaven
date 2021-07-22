using System;
using UnityEngine;

[Serializable]
public class SerializableOwnedAbility
{
    [SerializeField] private Ability2 abilityReference;
    [SerializeField] private int count;

    public Ability2 AbilityReference => abilityReference;
    public int Count => count;

    public SerializableOwnedAbility(Ability2 abilityReference, int count)
    {
        this.abilityReference = abilityReference;
        this.count = count;
    }
}
