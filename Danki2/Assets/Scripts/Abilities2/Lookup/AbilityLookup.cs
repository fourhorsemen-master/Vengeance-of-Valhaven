using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLookup : Singleton<AbilityLookup>
{
    [SerializeField] private AbilityData abilityData = null;
    
    public string GetDisplayName(SerializableGuid abilityId) => abilityData.AbilityDisplayNameDictionary[abilityId];
    public AbilityType GetAbilityType(SerializableGuid abilityId) => abilityData.AbilityTypeDictionary[abilityId];
    public int GetDamage(SerializableGuid abilityId) => abilityData.AbilityDamageDictionary[abilityId];
    public List<Empowerment> GetEmpowerments(SerializableGuid abilityId) => abilityData.AbilityEmpowermentsDictionary[abilityId].Empowerments;
    public Rarity GetRarity(SerializableGuid abilityId) => abilityData.AbilityRarityDictionary[abilityId];
    public Sprite GetIcon(SerializableGuid abilityId) => abilityData.AbilityIconDictionary[abilityId];
    
    public bool TryGetAbilityId(string displayName, out SerializableGuid abilityId)
    {
        foreach (KeyValuePair<SerializableGuid,string> kvp in abilityData.AbilityDisplayNameDictionary)
        {
            if (kvp.Value == displayName)
            {
                abilityId = kvp.Key;
                return true;
            }
        }
    
        abilityId = null;
        return false;
    }
    
    public void ForEachAbilityId(Action<SerializableGuid> action) => abilityData.AbilityIds.ForEach(action);
}
