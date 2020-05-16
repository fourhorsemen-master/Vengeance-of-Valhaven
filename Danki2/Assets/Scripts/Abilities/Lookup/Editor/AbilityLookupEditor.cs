using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityLookup))]
public class AbilityLookupEditor : Editor
{
    private readonly EnumDictionary<AbilityReference, EnumDictionary<AbilityDataDropdownGroup, bool>> foldoutStatus =
        new EnumDictionary<AbilityReference, EnumDictionary<AbilityDataDropdownGroup, bool>>(
            () => new EnumDictionary<AbilityDataDropdownGroup, bool>(false)
        );

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
        foldoutStatus[abilityReference][AbilityDataDropdownGroup.EntireAbility] = EditorGUILayout.Foldout(
            foldoutStatus[abilityReference][AbilityDataDropdownGroup.EntireAbility],
            serializableAbilityMetadata.Valid ? abilityReference.ToString() : $"{abilityReference.ToString()}*"
        );

        if (!foldoutStatus[abilityReference][AbilityDataDropdownGroup.EntireAbility]) return;

        EditorGUI.indentLevel++;
            
        EditDisplayName(serializableAbilityMetadata);
        EditTooltip(serializableAbilityMetadata);
        EditAbilityOrbType(serializableAbilityMetadata);
        EditBaseAbilityData(abilityReference, serializableAbilityMetadata);
        EditGeneratedOrbs(abilityReference, serializableAbilityMetadata);
        EditAbilityBonusData(abilityReference, serializableAbilityMetadata);
            
        EditorGUI.indentLevel--;
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

    private void EditBaseAbilityData(AbilityReference abilityReference, SerializableAbilityMetadata serializableAbilityMetadata)
    {
        foldoutStatus[abilityReference][AbilityDataDropdownGroup.BaseAbilityData] = EditorGUILayout.Foldout(
            foldoutStatus[abilityReference][AbilityDataDropdownGroup.BaseAbilityData],
            "Base Ability Data"
        );

        if (!foldoutStatus[abilityReference][AbilityDataDropdownGroup.BaseAbilityData]) return;

        EditorGUI.indentLevel++;

        AbilityData currentAbilityData = serializableAbilityMetadata.BaseAbilityData;
        int primaryDamage = EditorGUILayout.IntField("Primary Damage", currentAbilityData.PrimaryDamage);
        int secondaryDamage = EditorGUILayout.IntField("Secondary Damage", currentAbilityData.SecondaryDamage);
        int heal = EditorGUILayout.IntField("Heal", currentAbilityData.Heal);
        int shield = EditorGUILayout.IntField("Shield", currentAbilityData.Shield);
        serializableAbilityMetadata.BaseAbilityData = new AbilityData(primaryDamage, secondaryDamage, heal, shield);

        EditorGUI.indentLevel--;
    }

    private void EditGeneratedOrbs(AbilityReference abilityReference, SerializableAbilityMetadata serializableAbilityMetadata)
    {
        foldoutStatus[abilityReference][AbilityDataDropdownGroup.GeneratedOrbs] = EditorGUILayout.Foldout(
            foldoutStatus[abilityReference][AbilityDataDropdownGroup.GeneratedOrbs],
            "Generated Orbs"
        );

        if (!foldoutStatus[abilityReference][AbilityDataDropdownGroup.GeneratedOrbs]) return;

        EditorGUI.indentLevel++;

        EditOrbList(serializableAbilityMetadata.GeneratedOrbs, "Add Generated Orb");

        EditorGUI.indentLevel--;
    }

    private void EditAbilityBonusData(AbilityReference abilityReference, SerializableAbilityMetadata serializableAbilityMetadata)
    {
        foldoutStatus[abilityReference][AbilityDataDropdownGroup.BonusData] = EditorGUILayout.Foldout(
            foldoutStatus[abilityReference][AbilityDataDropdownGroup.BonusData],
            "Bonus Data"
        );

        if (!foldoutStatus[abilityReference][AbilityDataDropdownGroup.BonusData]) return;

        if (abilityAttributeDataLookup[abilityReference].Attribute.AbilityBonuses.Length == 0)
        {
            EditorGUI.indentLevel++;
            
            EditorGUILayout.LabelField($"There are no ability bonuses for {abilityReference.ToString()}.");
            
            EditorGUI.indentLevel--;
            return;
        }

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
