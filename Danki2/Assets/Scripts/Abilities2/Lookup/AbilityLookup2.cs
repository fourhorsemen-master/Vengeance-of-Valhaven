using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityDisplayNameDictionary : SerializableEnumDictionary<Ability2, string>
{
    public AbilityDisplayNameDictionary(string defaultValue) : base(defaultValue) {}
    public AbilityDisplayNameDictionary(Func<string> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityDamageDictionary : SerializableEnumDictionary<Ability2, int>
{
    public AbilityDamageDictionary(int defaultValue) : base(defaultValue) {}
    public AbilityDamageDictionary(Func<int> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class EmpowermentsWrapper
{
    [SerializeField] private List<Empowerment> empowerments = new List<Empowerment>();
    public List<Empowerment> Empowerments => empowerments;
}

[Serializable]
public class AbilityEmpowermentsDictionary : SerializableEnumDictionary<Ability2, EmpowermentsWrapper>
{
    public AbilityEmpowermentsDictionary(EmpowermentsWrapper defaultValue) : base(defaultValue) {}
    public AbilityEmpowermentsDictionary(Func<EmpowermentsWrapper> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityRarityDictionary : SerializableEnumDictionary<Ability2, Rarity>
{
    public AbilityRarityDictionary(Rarity defaultValue) : base(defaultValue) {}
    public AbilityRarityDictionary(Func<Rarity> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityCollisionSoundLevelDictionary : SerializableEnumDictionary<Ability2, CollisionSoundLevel>
{
    public AbilityCollisionSoundLevelDictionary(CollisionSoundLevel defaultValue) : base(defaultValue) {}
    public AbilityCollisionSoundLevelDictionary(Func<CollisionSoundLevel> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityIconDictionary2 : SerializableEnumDictionary<Ability2, Sprite>
{
    public AbilityIconDictionary2(Sprite defaultValue) : base(defaultValue) {}
    public AbilityIconDictionary2(Func<Sprite> defaultValueProvider) : base(defaultValueProvider) {}
}

public class AbilityLookup2 : Singleton<AbilityLookup2>
{
    public AbilityDisplayNameDictionary abilityDisplayNameDictionary = new AbilityDisplayNameDictionary("");
    public AbilityDamageDictionary abilityDamageDictionary = new AbilityDamageDictionary(0);
    public AbilityEmpowermentsDictionary abilityEmpowermentsDictionary = new AbilityEmpowermentsDictionary(() => new EmpowermentsWrapper());
    public AbilityRarityDictionary abilityRarityDictionary = new AbilityRarityDictionary(Rarity.Common);
    public AbilityCollisionSoundLevelDictionary abilityCollisionSoundLevelDictionary = new AbilityCollisionSoundLevelDictionary(CollisionSoundLevel.Low);
    public AbilityIconDictionary2 abilityIconDictionary = new AbilityIconDictionary2(defaultValue: null);

    public string GetDisplayName(Ability2 ability) => abilityDisplayNameDictionary[ability];
    public int GetDamage(Ability2 ability) => abilityDamageDictionary[ability];
    public List<Empowerment> GetEmpowerments(Ability2 ability) => abilityEmpowermentsDictionary[ability].Empowerments;
    public Rarity GetRarity(Ability2 ability) => abilityRarityDictionary[ability];
    public CollisionSoundLevel GetCollisionSoundLevel(Ability2 ability) => abilityCollisionSoundLevelDictionary[ability];
    public Sprite GetIcon(Ability2 ability) => abilityIconDictionary[ability];
}
