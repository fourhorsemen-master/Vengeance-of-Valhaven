using System;
using UnityEngine;

[Serializable]
public class SerializableOwnedAbility
{
    [SerializeField] private Ability2 ability;
    [SerializeField] private int count;

    public Ability2 Ability => ability;
    public int Count => count;

    public SerializableOwnedAbility(Ability2 ability, int count)
    {
        this.ability = ability;
        this.count = count;
    }
}
