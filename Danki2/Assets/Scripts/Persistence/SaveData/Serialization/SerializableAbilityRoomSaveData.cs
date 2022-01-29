using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableAbilityRoomSaveData
{
    [SerializeField] private List<SerializableAbility> abilityChoices;
    [SerializeField] private bool abilitiesViewed = false;
    [SerializeField] private bool abilitySelected = false;

    public List<SerializableAbility> AbilityChoices { get => abilityChoices; set => abilityChoices = value; }
    public bool AbilitiesViewed { get => abilitiesViewed; set => abilitiesViewed = value; }
    public bool AbilitySelected { get => abilitySelected; set => abilitySelected = value; }

    public AbilityRoomSaveData Deserialize()
    {
        return new AbilityRoomSaveData
        {
            AbilityChoices = AbilityChoices.Select(x => Ability.FromSerializedAbility(x)).ToList(),
            AbilitiesViewed = AbilitiesViewed,
            AbilitySelected = AbilitySelected
        };
    }
}
