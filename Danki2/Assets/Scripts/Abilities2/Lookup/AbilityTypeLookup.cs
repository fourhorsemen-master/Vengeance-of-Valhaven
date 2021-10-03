using System;

[Serializable]
public class AbilityCollisionSoundLevelDictionary : SerializableEnumDictionary<AbilityType2, CollisionSoundLevel> {
    public AbilityCollisionSoundLevelDictionary(CollisionSoundLevel defaultValue) : base(defaultValue) {}
    public AbilityCollisionSoundLevelDictionary(Func<CollisionSoundLevel> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityVocalisationTypeDictionary : SerializableEnumDictionary<AbilityType2, AbilityVocalisationType> {
    public AbilityVocalisationTypeDictionary(AbilityVocalisationType defaultValue) : base(defaultValue) {}
    public AbilityVocalisationTypeDictionary(Func<AbilityVocalisationType> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class AbilityFmodEventDictionary : SerializableEnumDictionary<AbilityType2, AbilityFmodEvents> {
    public AbilityFmodEventDictionary(AbilityFmodEvents defaultValue) : base(defaultValue) {}
    public AbilityFmodEventDictionary(Func<AbilityFmodEvents> defaultValueProvider) : base(defaultValueProvider) {}
}

public class AbilityTypeLookup : Singleton<AbilityTypeLookup>
{
    public AbilityCollisionSoundLevelDictionary abilityCollisionSoundLevelDictionary = new AbilityCollisionSoundLevelDictionary(CollisionSoundLevel.Low);
    public AbilityVocalisationTypeDictionary abilityVocalisationTypeDictionary = new AbilityVocalisationTypeDictionary(AbilityVocalisationType.None);
    public AbilityFmodEventDictionary abilityFmodEventDictionary = new AbilityFmodEventDictionary(() => new AbilityFmodEvents());

    public CollisionSoundLevel GetCollisionSoundLevel(AbilityType2 abilityType) => abilityCollisionSoundLevelDictionary[abilityType];
    public AbilityVocalisationType GetAbilityVocalisationType(AbilityType2 abilityType) => abilityVocalisationTypeDictionary[abilityType];
    public AbilityFmodEvents GetAbilityFmodEvents(AbilityType2 abilityType) => abilityFmodEventDictionary[abilityType];
}
