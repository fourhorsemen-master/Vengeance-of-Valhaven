using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectLookup))]
public class EffectLookupEditor : Editor
{
    private List<Type> effectTypes;
    private List<string> stringEffectTypes;
    
    public override void OnInspectorGUI()
    {
        EffectLookup effectLookup = (EffectLookup) target;

        if (effectTypes == null)
        {
            effectTypes = ReflectionUtils.GetSubclasses(typeof(Effect), ClassModifier.Abstract);
            stringEffectTypes = effectTypes
                .Select(type => type.AssemblyQualifiedName)
                .ToList();
        }

        SanitizeDictionaries(effectLookup);
        
        effectTypes.ForEach(effectType =>
        {
            string stringEffectType = effectType.AssemblyQualifiedName;
            
            EditorGUILayout.LabelField(effectType.Name, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            effectLookup.serializedDisplayNameMap[stringEffectType] = EditorGUILayout.TextField(
                "Display name",
                effectLookup.serializedDisplayNameMap[stringEffectType]
            );
            
            effectLookup.serializedSpriteMap[stringEffectType] = (Sprite) EditorGUILayout.ObjectField(
                "Sprite",
                effectLookup.serializedSpriteMap[stringEffectType],
                typeof(Sprite),
                false,
                null
            );
            
            EditorGUI.indentLevel--;
        });

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void SanitizeDictionaries(EffectLookup effectLookup)
    {
        stringEffectTypes.ForEach(stringEffectType =>
        {
            if (!effectLookup.serializedDisplayNameMap.ContainsKey(stringEffectType))
            {
                effectLookup.serializedDisplayNameMap[stringEffectType] = "";
            }

            if (!effectLookup.serializedSpriteMap.ContainsKey(stringEffectType))
            {
                effectLookup.serializedSpriteMap[stringEffectType] = null;
            }
        });

        List<string> serializedDisplayNameKeys = effectLookup.serializedDisplayNameMap.Keys.ToList();
        serializedDisplayNameKeys.ForEach(stringEffectType =>
        {
            if (!stringEffectTypes.Contains(stringEffectType))
            {
                effectLookup.serializedDisplayNameMap.Remove(stringEffectType);
            }
        });

        List<string> serializedSpriteKeys = effectLookup.serializedSpriteMap.Keys.ToList();
        serializedSpriteKeys.ForEach(stringEffectType =>
        {
            if (!stringEffectTypes.Contains(stringEffectType))
            {
                effectLookup.serializedSpriteMap.Remove(stringEffectType);
            }
        });
    }
}
