using System.Collections.Generic;
using UnityEngine;

public class CustomAbilityLookup : Singleton<CustomAbilityLookup>
{
    [SerializeField] private AbilityData abilityData = null;

    public Ability GetByName(string displayName)
    {
        foreach (KeyValuePair<SerializableGuid, string> kvp in abilityData.AbilityDisplayNameDictionary)
        {
            if (kvp.Value == displayName)
            {
                return new Ability(
                    abilityData.AbilityDisplayNameDictionary[kvp.Key],
                    abilityData.AbilityTypeDictionary[kvp.Key],
                    abilityData.AbilityDamageDictionary[kvp.Key],
                    abilityData.AbilityRarityDictionary[kvp.Key],
                    abilityData.AbilityEmpowermentsDictionary[kvp.Key].Empowerments
                );
            }
        }

        Debug.LogError($"ability name {displayName} not present in custom abilities");
        return null;
    }
}
