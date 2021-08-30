using System;
using UnityEngine;

[Serializable]
public class SerializableOwnedAbility
{
    [SerializeField] private SerializableGuid abilityId;
    [SerializeField] private int count;

    public SerializableGuid AbilityId => abilityId;
    public int Count => count;

    public SerializableOwnedAbility(SerializableGuid abilityId, int count)
    {
        this.abilityId = abilityId;
        this.count = count;
    }
}
