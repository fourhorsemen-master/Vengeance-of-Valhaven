using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class EffectLookup : Singleton<EffectLookup>
{
    public EffectDisplayNameMap serializedDisplayNameMap = new EffectDisplayNameMap();
    public EffectSpriteNameMap serializedSpriteMap = new EffectSpriteNameMap();

    private Dictionary<Type, string> displayNameMap = new Dictionary<Type, string>();
    private Dictionary<Type, Sprite> spriteMap = new Dictionary<Type, Sprite>();

    protected override void Awake()
    {
        base.Awake();
        
        Validate();

        displayNameMap = serializedDisplayNameMap.Keys
            .ToDictionary(
                Type.GetType,
                key => serializedDisplayNameMap[key]
            );

        spriteMap = serializedSpriteMap.Keys
            .ToDictionary(
                Type.GetType,
                key => serializedSpriteMap[key]
            );
    }

    public string GetDisplayName(Type effectType) => displayNameMap[effectType];

    public Sprite GetSprite(Type effectType) => spriteMap[effectType];

    private void Validate()
    {
        List<string> stringEffectTypes = ReflectionUtils
            .GetSubclasses(typeof(Effect), Assembly.GetExecutingAssembly(), ClassModifier.Abstract)
            .Select(type => type.AssemblyQualifiedName)
            .ToList();
        
        stringEffectTypes.ForEach(stringEffectType =>
        {
            if (!serializedDisplayNameMap.ContainsKey(stringEffectType))
            {
                Debug.LogError($"No display name entry for effect: {stringEffectType}");
                return;
            }

            if (!serializedSpriteMap.ContainsKey(stringEffectType))
            {
                Debug.LogError($"No sprite entry for effect: {stringEffectType}");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(serializedDisplayNameMap[stringEffectType]))
            {
                Debug.LogError($"No display name set for effect: {stringEffectType}");
            }

            if (serializedSpriteMap[stringEffectType] == null)
            {
                Debug.LogError($"No icon found for effect: {stringEffectType}");
            }
        });
        
        foreach (string stringEffectType in serializedDisplayNameMap.Keys)
        {
            if (!stringEffectTypes.Contains(stringEffectType))
            {
                Debug.LogError($"Invalid effect type found in display name map: {stringEffectType}");
            }
        }
        
        foreach (string stringEffectType in serializedSpriteMap.Keys)
        {
            if (!stringEffectTypes.Contains(stringEffectType))
            {
                Debug.LogError($"Invalid effect type found in sprite map: {stringEffectType}");
            }
        }
    }
}
