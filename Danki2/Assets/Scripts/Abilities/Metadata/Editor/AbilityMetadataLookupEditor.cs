using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

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
            EditBaseAbilityData(serializableAbilityMetadata);
            EditAbilityOrbType(serializableAbilityMetadata);
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

    private void EditBaseAbilityData(SerializableAbilityMetadata serializableAbilityMetadata)
    {
        EditorGUILayout.LabelField("Base Ability Data");
        EditorGUI.indentLevel++;

        AbilityData currentAbilityData = serializableAbilityMetadata.BaseAbilityData;
        int primaryDamage = EditorGUILayout.IntField("Primary Damage", currentAbilityData.PrimaryDamage);
        int secondaryDamage = EditorGUILayout.IntField("Secondary Damage", currentAbilityData.SecondaryDamage);
        int heal = EditorGUILayout.IntField("Heal", currentAbilityData.Heal);
        int shield = EditorGUILayout.IntField("Shield", currentAbilityData.Shield);
        serializableAbilityMetadata.BaseAbilityData = new AbilityData(primaryDamage, secondaryDamage, heal, shield);

        EditorGUI.indentLevel--;
    }

    private void EditAbilityOrbType(SerializableAbilityMetadata serializableAbilityMetadata)
    {
        GUILayout.BeginHorizontal();

        if (serializableAbilityMetadata.AbilityOrbType.HasValue)
        {
            serializableAbilityMetadata.AbilityOrbType.Value = (OrbType) EditorGUILayout
                .EnumPopup("Ability Orb Type", serializableAbilityMetadata.AbilityOrbType.Value);
            if (GUILayout.Button("Remove Ability Orb Type"))
            {
                serializableAbilityMetadata.AbilityOrbType = null;
            }
        }
        else
        {
            GUILayout.Space (EditorGUI.indentLevel * 15);

            if (GUILayout.Button("Add Ability Orb Type"))
            {
                serializableAbilityMetadata.AbilityOrbType.HasValue = true;
                serializableAbilityMetadata.AbilityOrbType.Value = default;
            }
        }

        GUILayout.EndHorizontal();
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
