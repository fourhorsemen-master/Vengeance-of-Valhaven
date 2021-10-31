using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AbilityDisplayNameDictionary : SerializableDictionary<SerializableGuid, string> {}

[Serializable]
public class AbilityTypeDictionary : SerializableDictionary<SerializableGuid, AbilityType> {}

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
public class AbilityIconDictionary : SerializableDictionary<SerializableGuid, Sprite> {}

public class AbilityData : ScriptableObject
{
    [SerializeField]
    private List<SerializableGuid> abilityIds = new List<SerializableGuid>();
    public List<SerializableGuid> AbilityIds => abilityIds;

    [SerializeField]
    private AbilityDisplayNameDictionary abilityDisplayNameDictionary = new AbilityDisplayNameDictionary();
    public AbilityDisplayNameDictionary AbilityDisplayNameDictionary => abilityDisplayNameDictionary;
    
    [SerializeField]
    private AbilityTypeDictionary abilityTypeDictionary = new AbilityTypeDictionary();
    public AbilityTypeDictionary AbilityTypeDictionary => abilityTypeDictionary;
    
    [SerializeField]
    private AbilityDamageDictionary abilityDamageDictionary = new AbilityDamageDictionary();
    public AbilityDamageDictionary AbilityDamageDictionary => abilityDamageDictionary;
    
    [SerializeField]
    private AbilityEmpowermentsDictionary abilityEmpowermentsDictionary = new AbilityEmpowermentsDictionary();
    public AbilityEmpowermentsDictionary AbilityEmpowermentsDictionary => abilityEmpowermentsDictionary;
    
    [SerializeField]
    private AbilityRarityDictionary abilityRarityDictionary = new AbilityRarityDictionary();
    public AbilityRarityDictionary AbilityRarityDictionary => abilityRarityDictionary;
    
    [SerializeField]
    private AbilityIconDictionary abilityIconDictionary = new AbilityIconDictionary();
    public AbilityIconDictionary AbilityIconDictionary => abilityIconDictionary;

    public string[] GetAbilityNames() => abilityIds.Select(id => abilityDisplayNameDictionary[id]).ToArray();
}
