using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLookup2 : Singleton<AbilityLookup2>
{
    [SerializeField] private AbilityData abilityData = null;
    
    public string GetDisplayName(SerializableGuid abilityId) => abilityData.abilityDisplayNameDictionary[abilityId];
    public AbilityType2 GetAbilityType(SerializableGuid abilityId) => abilityData.abilityTypeDictionary[abilityId];
    public int GetDamage(SerializableGuid abilityId) => abilityData.abilityDamageDictionary[abilityId];
    public List<Empowerment> GetEmpowerments(SerializableGuid abilityId) => abilityData.abilityEmpowermentsDictionary[abilityId].Empowerments;
    public Rarity GetRarity(SerializableGuid abilityId) => abilityData.abilityRarityDictionary[abilityId];
    public Sprite GetIcon(SerializableGuid abilityId) => abilityData.abilityIconDictionary[abilityId];

    public CollisionSoundLevel GetCollisionSoundLevel(SerializableGuid abilityId) => AbilityTypeLookup.Instance.GetCollisionSoundLevel(GetAbilityType(abilityId));
    public AbilityVocalisationType GetAbilityVocalisationType(SerializableGuid abilityId) => AbilityTypeLookup.Instance.GetAbilityVocalisationType(GetAbilityType(abilityId));
    public AbilityFmodEvents GetAbilityFmodEvents(SerializableGuid abilityId) => AbilityTypeLookup.Instance.GetAbilityFmodEvents(GetAbilityType(abilityId));
    
    public bool TryGetAbilityId(string displayName, out SerializableGuid abilityId)
    {
        foreach (KeyValuePair<SerializableGuid,string> kvp in abilityData.abilityDisplayNameDictionary)
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
    
    public void ForEachAbilityId(Action<SerializableGuid> action) => abilityData.abilityIds.ForEach(action);
}
