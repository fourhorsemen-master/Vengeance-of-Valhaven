﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityMetadataLookup))]
public class AbilityMetadataLookupEditor : Editor
{
    private readonly EnumDictionary<AbilityReference, bool> foldoutStatus = new EnumDictionary<AbilityReference, bool>(false);
    
    public override void OnInspectorGUI()
    {
        AbilityMetadataLookup abilityMetadataLookup = (AbilityMetadataLookup) target;

        SerializedMetadataLookup serializedMetadataLookup = abilityMetadataLookup.serializedMetadataLookup;
        
        EditorGUILayout.LabelField("Abilities marked with an asterisk (*) are missing data.");

        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            if (!serializedMetadataLookup.ContainsKey(abilityReference))
            {
                serializedMetadataLookup[abilityReference] = new SerializableAbilityMetadata();
            }

            EditSerializableAbilityMetadata(abilityReference, serializedMetadataLookup[abilityReference]);
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditSerializableAbilityMetadata(AbilityReference abilityReference, SerializableAbilityMetadata serializableAbilityMetadata)
    {
        foldoutStatus[abilityReference] = EditorGUILayout.Foldout(
            foldoutStatus[abilityReference],
            serializableAbilityMetadata.Valid ? abilityReference.ToString() : $"{abilityReference.ToString()}*"
        );

        if (foldoutStatus[abilityReference])
        {
            EditorGUI.indentLevel++;
            
            EditDisplayName(serializableAbilityMetadata);
            EditTooltip(serializableAbilityMetadata);
            EditAbilityData(serializableAbilityMetadata);
            EditGeneratedOrbs(serializableAbilityMetadata);
            
            EditorGUI.indentLevel--;
        }
    }

    private void EditDisplayName(SerializableAbilityMetadata serializableAbilityMetadata)
    {
        serializableAbilityMetadata.DisplayName = EditorGUILayout.TextField("Display Name", serializableAbilityMetadata.DisplayName);
    }

    private void EditTooltip(SerializableAbilityMetadata serializableAbilityMetadata)
    {
        serializableAbilityMetadata.Tooltip = EditorGUILayout.TextField("Tooltip", serializableAbilityMetadata.Tooltip);
    }

    private void EditAbilityData(SerializableAbilityMetadata serializableAbilityMetadata)
    {
        EditorGUILayout.LabelField("Ability Data");
        EditorGUI.indentLevel++;

        AbilityData abilityData = serializableAbilityMetadata.AbilityData;
        int primaryDamage = EditorGUILayout.IntField("Primary Damage", abilityData.PrimaryDamage);
        int secondaryDamage = EditorGUILayout.IntField("Secondary Damage", abilityData.SecondaryDamage);
        int heal = EditorGUILayout.IntField("Heal", abilityData.Heal);
        int shield = EditorGUILayout.IntField("Shield", abilityData.Shield);
        serializableAbilityMetadata.AbilityData = new AbilityData(primaryDamage, secondaryDamage, heal, shield);

        EditorGUI.indentLevel--;
    }

    private void EditGeneratedOrbs(SerializableAbilityMetadata serializableAbilityMetadata)
    {
        EditorGUILayout.LabelField("Generated Orbs");
        EditorGUI.indentLevel++;

        List<OrbType> generatedOrbs = serializableAbilityMetadata.GeneratedOrbs;

        for (int i = generatedOrbs.Count - 1; i >= 0; i--)
        {
            GUILayout.BeginHorizontal();

            generatedOrbs[i] = (OrbType)EditorGUILayout.EnumPopup($"Orb {generatedOrbs.Count - i}", generatedOrbs[i]);
            if (GUILayout.Button("Remove Orb"))
            {
                generatedOrbs.RemoveAt(i);
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Space (EditorGUI.indentLevel * 15);

        if (GUILayout.Button("Add Orb"))
        {
            serializableAbilityMetadata.GeneratedOrbs.Add(default);
        }

        GUILayout.EndHorizontal();
        EditorGUI.indentLevel--;
    }
}
