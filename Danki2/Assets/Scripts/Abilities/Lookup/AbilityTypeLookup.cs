using System;
using UnityEngine;

[Serializable]
public class AbilityCollisionSoundLevelDictionary : SerializableEnumDictionary<AbilityType, CollisionSoundLevel> {
    public AbilityCollisionSoundLevelDictionary(CollisionSoundLevel defaultValue) : base(defaultValue) {}
    public AbilityCollisionSoundLevelDictionary(Func<CollisionSoundLevel> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityVocalisationTypeDictionary : SerializableEnumDictionary<AbilityType, AbilityVocalisationType> {
    public AbilityVocalisationTypeDictionary(AbilityVocalisationType defaultValue) : base(defaultValue) {}
    public AbilityVocalisationTypeDictionary(Func<AbilityVocalisationType> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityFmodEventDictionary : SerializableEnumDictionary<AbilityType, AbilityFmodEvents> {
    public AbilityFmodEventDictionary(AbilityFmodEvents defaultValue) : base(defaultValue) {}
    public AbilityFmodEventDictionary(Func<AbilityFmodEvents> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityIconDictionary : SerializableEnumDictionary<AbilityType, Sprite>
{
    public AbilityIconDictionary(Sprite defaultValue) : base(defaultValue) { }
    public AbilityIconDictionary(Func<Sprite> defaultValueProvider) : base(defaultValueProvider) { }
}

[Serializable]
public class AbilityBaseDamageDictionary : SerializableEnumDictionary<AbilityType, int>
{
    public AbilityBaseDamageDictionary(int defaultValue) : base(defaultValue) { }
    public AbilityBaseDamageDictionary(Func<int> defaultValueProvider) : base(defaultValueProvider) { }
}

public class AbilityTypeLookup : Singleton<AbilityTypeLookup>
{
    public AbilityCollisionSoundLevelDictionary abilityCollisionSoundLevelDictionary = new AbilityCollisionSoundLevelDictionary(CollisionSoundLevel.Low);
    public AbilityVocalisationTypeDictionary abilityVocalisationTypeDictionary = new AbilityVocalisationTypeDictionary(AbilityVocalisationType.None);
    public AbilityFmodEventDictionary abilityFmodEventDictionary = new AbilityFmodEventDictionary(() => new AbilityFmodEvents());
    public AbilityIconDictionary abilityIconDictionary = new AbilityIconDictionary(() => null);
    public AbilityBaseDamageDictionary abilityBaseDamageDictionary = new AbilityBaseDamageDictionary(() => 0);

    public CollisionSoundLevel GetCollisionSoundLevel(AbilityType abilityType) => abilityCollisionSoundLevelDictionary[abilityType];
    public AbilityVocalisationType GetAbilityVocalisationType(AbilityType abilityType) => abilityVocalisationTypeDictionary[abilityType];
    public AbilityFmodEvents GetAbilityFmodEvents(AbilityType abilityType) => abilityFmodEventDictionary[abilityType];
    public Sprite GetAbilityIcon(AbilityType abilityType) => abilityIconDictionary[abilityType];
    public int GetAbilityBaseDamage(AbilityType abilityType) => abilityBaseDamageDictionary[abilityType];
}
