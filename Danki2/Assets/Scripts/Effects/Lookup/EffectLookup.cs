using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EffectLookup : Singleton<EffectLookup>
{
    public EffectDisplayNameDictionary displayNameDictionary = new EffectDisplayNameDictionary();
    public EffectSpriteNameDictionary spriteDictionary = new EffectSpriteNameDictionary();

    protected override void Awake()
    {
        base.Awake();
        
        List<Type> effectTypes = ReflectionUtils
            .GetSubclasses(typeof(Effect), Assembly.GetExecutingAssembly(), ClassModifier.Abstract);
        
        Validate(displayNameDictionary, effectTypes, (s, t) =>
        {
            if (string.IsNullOrWhiteSpace(s)) Debug.LogError($"Null or empty display name for effect: {t.Name}");
        });
        
        Validate(spriteDictionary, effectTypes, (s, t) =>
        {
            if (s == null) Debug.LogError($"No sprite for effect: {t.Name}");
        });
    }

    public string GetDisplayName(Type effectType) => displayNameDictionary[effectType];

    public Sprite GetSprite(Type effectType) => spriteDictionary[effectType];

    private void Validate<T>(SerializableTypeDictionary<T> dictionary, List<Type> effectTypes, Action<T, Type> validate)
    {
        effectTypes.ForEach(effectType =>
        {
            if (!dictionary.ContainsKey(effectType))
            {
                Debug.LogError($"No entry for effect: {effectType.Name}");
                return;
            }

            validate(dictionary[effectType], effectType);
        });
        
        foreach (Type type in dictionary.Keys)
        {
            if (!effectTypes.Contains(type))
            {
                Debug.LogError($"Invalid effect type found in dictionary: {type.Name}");
            }
        }
    }
}
