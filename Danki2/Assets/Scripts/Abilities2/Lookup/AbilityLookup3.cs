using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityDisplayNameDictionary2 : SerializableDictionary<SerializableGuid, string> {}

[Serializable]
public class AbilityTypeDictionary2 : SerializableDictionary<SerializableGuid, AbilityType2> {}

[Serializable]
public class AbilityDamageDictionary2 : SerializableDictionary<SerializableGuid, int> {}

[Serializable]
public class EmpowermentsWrapper2
{
    [SerializeField] private List<Empowerment> empowerments = new List<Empowerment>();
    public List<Empowerment> Empowerments => empowerments;
}

[Serializable]
public class AbilityEmpowermentsDictionary2 : SerializableDictionary<SerializableGuid, EmpowermentsWrapper2> {}

[Serializable]
public class AbilityRarityDictionary2 : SerializableDictionary<SerializableGuid, Rarity> {}

[Serializable]
public class AbilityCollisionSoundLevelDictionary2 : SerializableDictionary<SerializableGuid, CollisionSoundLevel> {}

[Serializable]
public class AbilityIconDictionary3 : SerializableDictionary<SerializableGuid, Sprite> {}

public class AbilityLookup3 : Singleton<AbilityLookup3>
{
    public List<SerializableGuid> abilityIds = new List<SerializableGuid>();

    public AbilityDisplayNameDictionary2 abilityDisplayNameDictionary = new AbilityDisplayNameDictionary2();
    public AbilityTypeDictionary2 abilityTypeDictionary = new AbilityTypeDictionary2();
    public AbilityDamageDictionary2 abilityDamageDictionary = new AbilityDamageDictionary2();
    public AbilityEmpowermentsDictionary2 abilityEmpowermentsDictionary = new AbilityEmpowermentsDictionary2();
    public AbilityRarityDictionary2 abilityRarityDictionary = new AbilityRarityDictionary2();
    public AbilityCollisionSoundLevelDictionary2 abilityCollisionSoundLevelDictionary = new AbilityCollisionSoundLevelDictionary2();
    public AbilityIconDictionary3 abilityIconDictionary = new AbilityIconDictionary3();

    public string GetDisplayName(SerializableGuid abilityId) => abilityDisplayNameDictionary[abilityId];
    public AbilityType2 GetAbilityType(SerializableGuid abilityId) => abilityTypeDictionary[abilityId];
    public int GetDamage(SerializableGuid abilityId) => abilityDamageDictionary[abilityId];
    public List<Empowerment> GetEmpowerments(SerializableGuid abilityId) => abilityEmpowermentsDictionary[abilityId].Empowerments;
    public Rarity GetRarity(SerializableGuid abilityId) => abilityRarityDictionary[abilityId];
    public CollisionSoundLevel GetCollisionSoundLevel(SerializableGuid abilityId) => abilityCollisionSoundLevelDictionary[abilityId];
    public Sprite GetIcon(SerializableGuid abilityId) => abilityIconDictionary[abilityId];
}
