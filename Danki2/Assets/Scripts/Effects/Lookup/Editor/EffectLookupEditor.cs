using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectLookup))]
public class EffectLookupEditor : Editor
{
    private readonly EffectLookupSanitizer sanitizer = new EffectLookupSanitizer();

    private readonly EnumDictionary<ActiveEffect, bool> activeEffectFoldoutStatus =
        new EnumDictionary<ActiveEffect, bool>(false);
    private readonly EnumDictionary<PassiveEffect, bool> passiveEffectFoldoutStatus =
        new EnumDictionary<PassiveEffect, bool>(false);
    private readonly EnumDictionary<StackingEffect, bool> stackingEffectFoldoutStatus =
        new EnumDictionary<StackingEffect, bool>(false);
    
    public override void OnInspectorGUI()
    {
        EffectLookup effectLookup = (EffectLookup) target;
        
        SerializableActiveEffectDictionary serializableActiveEffectDictionary = effectLookup.serializableActiveEffectDictionary;
        SerializablePassiveEffectDictionary serializablePassiveEffectDictionary = effectLookup.serializablePassiveEffectDictionary;
        SerializableStackingEffectDictionary serializableStackingEffectDictionary = effectLookup.serializableStackingEffectDictionary;
        
        sanitizer.SanitizeActiveEffects(serializableActiveEffectDictionary);
        sanitizer.SanitizePassiveEffects(serializablePassiveEffectDictionary);
        sanitizer.SanitizeStackingEffects(serializableStackingEffectDictionary);
        
        EditActiveEffects(serializableActiveEffectDictionary);
        EditPassiveEffects(serializablePassiveEffectDictionary);
        EditStackingEffects(serializableStackingEffectDictionary);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditActiveEffects(SerializableActiveEffectDictionary serializableActiveEffectDictionary)
    {
        EditorUtils.Header("Active Effects");

        EnumUtils.ForEach((ActiveEffect effect) =>
        {
            EditorGUI.indentLevel += 2;
            activeEffectFoldoutStatus[effect] = EditorGUILayout.Foldout(activeEffectFoldoutStatus[effect], effect.ToString());
            EditorGUI.indentLevel -= 2;
            
            if (!activeEffectFoldoutStatus[effect]) return;
            
            EditBaseEffectData(serializableActiveEffectDictionary[effect]);
        });
    }

    private void EditPassiveEffects(SerializablePassiveEffectDictionary serializablePassiveEffectDictionary)
    {
        EditorUtils.Header("Passive Effects");

        EnumUtils.ForEach((PassiveEffect effect) =>
        {
            EditorGUI.indentLevel += 2;
            passiveEffectFoldoutStatus[effect] = EditorGUILayout.Foldout(passiveEffectFoldoutStatus[effect], effect.ToString());
            EditorGUI.indentLevel -= 2;
            
            if (!passiveEffectFoldoutStatus[effect]) return;
            
            EditBaseEffectData(serializablePassiveEffectDictionary[effect]);
        });
    }

    private void EditBaseEffectData(SerializableEffectData effectData)
    {
        EditorGUI.indentLevel += 3;
        
        effectData.DisplayName = EditorGUILayout.TextField("Display Name", effectData.DisplayName);
        effectData.Sprite = (Sprite) EditorGUILayout.ObjectField("Sprite", effectData.Sprite, typeof(Sprite));
        
        EditorGUI.indentLevel -= 3;
    }

    private void EditStackingEffects(SerializableStackingEffectDictionary serializableStackingEffectDictionary)
    {
        EditorUtils.Header("Stacking Effects");
        
        EnumUtils.ForEach((StackingEffect effect) =>
        {
            EditorGUI.indentLevel += 2;
            stackingEffectFoldoutStatus[effect] = EditorGUILayout.Foldout(stackingEffectFoldoutStatus[effect], effect.ToString());
            EditorGUI.indentLevel -= 2;
            
            if (!stackingEffectFoldoutStatus[effect]) return;
            
            EditStackingEffectData(serializableStackingEffectDictionary[effect]);
        });
    }

    private void EditStackingEffectData(SerializableStackingEffectData effectData)
    {
        EditorGUI.indentLevel += 3;
        
        effectData.DisplayName = EditorGUILayout.TextField("Display Name", effectData.DisplayName);
        effectData.HasMaxStackSize = EditorGUILayout.Toggle("Has Max Stack Size", effectData.HasMaxStackSize);
        if (effectData.HasMaxStackSize)
        {
            effectData.MaxStackSize = EditorGUILayout.IntField("Max Stack Size", effectData.MaxStackSize);
        }
        effectData.Duration = EditorGUILayout.FloatField("Duration", effectData.Duration);
        effectData.Sprite = (Sprite) EditorGUILayout.ObjectField("Sprite", effectData.Sprite, typeof(Sprite));
        
        EditorGUI.indentLevel -= 3;
    }
}
