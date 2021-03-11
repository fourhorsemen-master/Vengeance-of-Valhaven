using System;
using UnityEngine;

[Serializable]
public class SerializableOwnedAbility
{
    [SerializeField] private AbilityReference abilityReference;
    [SerializeField] private int count;

    public AbilityReference AbilityReference => abilityReference;
    public int Count => count;

    public SerializableOwnedAbility(AbilityReference abilityReference, int count)
    {
        this.abilityReference = abilityReference;
        this.count = count;
    }
}
