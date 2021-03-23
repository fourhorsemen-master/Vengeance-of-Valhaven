using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableAbilityRoomSaveData
{
    [SerializeField] private List<AbilityReference> abilityChoices = new List<AbilityReference>();

    public List<AbilityReference> AbilityChoices { get => abilityChoices; set => abilityChoices = value; }

    public AbilityRoomSaveData Deserialize()
    {
        return new AbilityRoomSaveData
        {
            AbilityChoices = AbilityChoices
        };
    }
}
