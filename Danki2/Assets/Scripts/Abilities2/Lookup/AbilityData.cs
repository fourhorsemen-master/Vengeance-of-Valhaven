using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AbilityDisplayNameDictionary : SerializableDictionary<SerializableGuid, string> {}

[Serializable]
public class AbilityTypeDictionary : SerializableDictionary<SerializableGuid, AbilityType2> {}

[Serializable]
public class AbilityDamageDictionary : SerializableDictionary<SerializableGuid, int> {}

[Serializable]
public class EmpowermentsWrapper
{
    [SerializeField] private List<Empowerment> empowerments = new List<Empowerment>();
    public List<Empowerment> Empowerments => empowerments;
}

[Serializable]
public class AbilityEmpowermentsDictionary : SerializableDictionary<SerializableGuid, EmpowermentsWrapper> {}

[Serializable]
public class AbilityRarityDictionary : SerializableDictionary<SerializableGuid, Rarity> {}

[Serializable]
public class AbilityIconDictionary2 : SerializableDictionary<SerializableGuid, Sprite> {}

[CreateAssetMenu(menuName = "Abilities/AbilityData")]
public class AbilityData : ScriptableObject
{
    public List<SerializableGuid> abilityIds = new List<SerializableGuid>();

    public AbilityDisplayNameDictionary abilityDisplayNameDictionary = new AbilityDisplayNameDictionary();
    public AbilityTypeDictionary abilityTypeDictionary = new AbilityTypeDictionary();
    public AbilityDamageDictionary abilityDamageDictionary = new AbilityDamageDictionary();
    public AbilityEmpowermentsDictionary abilityEmpowermentsDictionary = new AbilityEmpowermentsDictionary();
    public AbilityRarityDictionary abilityRarityDictionary = new AbilityRarityDictionary();
    public AbilityIconDictionary2 abilityIconDictionary = new AbilityIconDictionary2();

    public string GetDisplayName(SerializableGuid abilityId) => abilityDisplayNameDictionary[abilityId];
    public AbilityType2 GetAbilityType(SerializableGuid abilityId) => abilityTypeDictionary[abilityId];
    public int GetDamage(SerializableGuid abilityId) => abilityDamageDictionary[abilityId];
    public List<Empowerment> GetEmpowerments(SerializableGuid abilityId) => abilityEmpowermentsDictionary[abilityId].Empowerments;
    public Rarity GetRarity(SerializableGuid abilityId) => abilityRarityDictionary[abilityId];
    public Sprite GetIcon(SerializableGuid abilityId) => abilityIconDictionary[abilityId];

    public bool TryGetAbilityId(string displayName, out SerializableGuid abilityId)
    {
        foreach (KeyValuePair<SerializableGuid,string> kvp in abilityDisplayNameDictionary)
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

    public void ForEachAbilityId(Action<SerializableGuid> action) => abilityIds.ForEach(action);

    public string[] GetAbilityNames() => abilityIds.Select(GetDisplayName).ToArray();
}