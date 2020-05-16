using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityLookup))]
public class AbilityLookupEditor : Editor
{
    private readonly EnumDictionary<AbilityReference, bool> foldoutStatus = new EnumDictionary<AbilityReference, bool>(false);

    private Dictionary<AbilityReference, AttributeData<AbilityAttribute>> abilityAttributeDataLookup;

    public override void OnInspectorGUI()
    {
        AbilityLookup abilityLookup = (AbilityLookup) target;

        if (abilityAttributeDataLookup == null)
        {
            LoadAbilityAttributeData();
        }

        if (GUILayout.Button("Refresh"))
        {
            LoadAbilityAttributeData();
        }

        SerializableMetadataLookup serializableMetadataLookup = abilityLookup.serializableMetadataLookup;
        
        EditorGUILayout.LabelField("Abilities marked with an asterisk (*) are missing data.");

        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            if (!serializableMetadataLookup.ContainsKey(abilityReference))
            {
                serializableMetadataLookup[abilityReference] = new SerializableAbilityMetadata();
            }

            EditSerializableAbilityMetadata(abilityReference, serializableMetadataLookup[abilityReference]);
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void LoadAbilityAttributeData()
    {
        abilityAttributeDataLookup = ReflectionUtils.GetAttributeData<AbilityAttribute>().ToDictionary(
            d => d.Attribute.AbilityReference,
            d => d
        );
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
            EditAbilityOrbType(serializableAbilityMetadata);
            EditBaseAbilityData(serializableAbilityMetadata);
            EditGeneratedOrbs(serializableAbilityMetadata);
            EditAbilityBonusData(abilityReference, serializableAbilityMetadata);
            
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

    private void EditGeneratedOrbs(SerializableAbilityMetadata serializableAbilityMetadata)
    {
        EditorGUILayout.LabelField("Generated Orbs");
        EditorGUI.indentLevel++;

        EditOrbList(serializableAbilityMetadata.GeneratedOrbs, "Add Generated Orb");

        EditorGUI.indentLevel--;
    }

    private void EditAbilityBonusData(AbilityReference abilityReference, SerializableAbilityMetadata serializableAbilityMetadata)
    {
        EditorGUILayout.LabelField("Bonus Data");
        EditorGUI.indentLevel++;

        SerializableAbilityBonusLookup oldSerializableAbilityBonusLookup = serializableAbilityMetadata.AbilityBonusLookup;
        SerializableAbilityBonusLookup newSerializableAbilityBonusLookup = new SerializableAbilityBonusLookup();

        foreach (string abilityBonus in abilityAttributeDataLookup[abilityReference].Attribute.AbilityBonuses)
        {
            EditorGUILayout.LabelField(abilityBonus);
            EditorGUI.indentLevel++;
            
            newSerializableAbilityBonusLookup[abilityBonus] = oldSerializableAbilityBonusLookup.ContainsKey(abilityBonus)
                ? oldSerializableAbilityBonusLookup[abilityBonus]
                : new SerializableAbilityBonusMetadata();

            newSerializableAbilityBonusLookup[abilityBonus].DisplayName = EditorGUILayout.TextField(
                "Display Name",
                newSerializableAbilityBonusLookup[abilityBonus].DisplayName
            );

            newSerializableAbilityBonusLookup[abilityBonus].Tooltip = EditorGUILayout.TextField(
                "Tooltip",
                newSerializableAbilityBonusLookup[abilityBonus].Tooltip
            );

            EditOrbList(newSerializableAbilityBonusLookup[abilityBonus].RequiredOrbs, "Add Required Orb");
            
            EditorGUI.indentLevel--;
        }

        serializableAbilityMetadata.AbilityBonusLookup = newSerializableAbilityBonusLookup;

        EditorGUI.indentLevel--;
    }

    private void EditOrbList(List<OrbType> orbTypes, string buttonLabel)
    {
        for (int i = orbTypes.Count - 1; i >= 0; i--)
        {
            GUILayout.BeginHorizontal();

            orbTypes[i] = (OrbType)EditorGUILayout.EnumPopup($"Orb {orbTypes.Count - i}", orbTypes[i]);
            if (GUILayout.Button("Remove Orb"))
            {
                orbTypes.RemoveAt(i);
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Space (EditorGUI.indentLevel * 15);

        if (GUILayout.Button(buttonLabel))
        {
            orbTypes.Insert(0, default);
        }

        GUILayout.EndHorizontal();
    }
}
