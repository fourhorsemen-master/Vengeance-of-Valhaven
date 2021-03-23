using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableAbilityRoomSaveData
{
    [SerializeField] private List<AbilityReference> abilityChoices = new List<AbilityReference>();
    [SerializeField] private bool abilitiesViewed = false;

    public List<AbilityReference> AbilityChoices { get => abilityChoices; set => abilityChoices = value; }
    public bool AbilitiesViewed { get => abilitiesViewed; set => abilitiesViewed = value; }

    public AbilityRoomSaveData Deserialize()
    {
        return new AbilityRoomSaveData
        {
            AbilityChoices = AbilityChoices,
            AbilitiesViewed = AbilitiesViewed
        };
    }
}
