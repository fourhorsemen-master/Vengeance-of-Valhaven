﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableAbilityRoomSaveData
{
    [SerializeField] private List<Ability2> abilityChoices;
    [SerializeField] private bool abilitiesViewed = false;
    [SerializeField] private bool abilitySelected = false;

    public List<Ability2> AbilityChoices { get => abilityChoices; set => abilityChoices = value; }
    public bool AbilitiesViewed { get => abilitiesViewed; set => abilitiesViewed = value; }
    public bool AbilitySelected { get => abilitySelected; set => abilitySelected = value; }

    public AbilityRoomSaveData Deserialize()
    {
        return new AbilityRoomSaveData
        {
            AbilityChoices = AbilityChoices,
            AbilitiesViewed = AbilitiesViewed,
            AbilitySelected = AbilitySelected
        };
    }
}
