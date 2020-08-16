using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectLookup))]
public class EffectLookupEditor : Editor
{
    private EffectLookup effectLookup;
    private List<Type> effectTypes;
    
    public override void OnInspectorGUI()
    {
        effectLookup = (EffectLookup) target;

        if (effectTypes == null)
        {
            effectTypes = ReflectionUtils.GetSubclasses(typeof(Effect), ClassModifier.Abstract);
        }

        Sanitize(effectLookup.displayNameDictionary, "");
        Sanitize(effectLookup.spriteDictionary, null);
        
        effectTypes.ForEach(Edit);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void Sanitize<T>(SerializableTypeDictionary<T> dictionary, T defaultValue)
    {
        effectTypes.ForEach(effectType =>
        {
            if (!dictionary.ContainsKey(effectType))
            {
                dictionary[effectType] = defaultValue;
            }
        });

        dictionary.Keys.ToList().ForEach(type =>
        {
            if (!effectTypes.Contains(type))
            {
                dictionary.Remove(type);
            }
        });
    }

    private void Edit(Type effectType)
    {
        EditorGUILayout.LabelField(effectType.Name, EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        effectLookup.displayNameDictionary[effectType] = EditorGUILayout.TextField(
            "Display name",
            effectLookup.displayNameDictionary[effectType]
        );
            
        effectLookup.spriteDictionary[effectType] = (Sprite) EditorGUILayout.ObjectField(
            "Sprite",
            effectLookup.spriteDictionary[effectType],
            typeof(Sprite),
            false,
            null
        );
            
        EditorGUI.indentLevel--;
    }
}
