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
    
    private readonly AbilityLookupSanitizer abilityLookupSanitizer = new AbilityLookupSanitizer();

    private List<AttributeData<AbilityAttribute>> abilityAttributeData;
    private Dictionary<AbilityReference, AttributeData<AbilityAttribute>> abilityAttributeDataLookup;

    public override void OnInspectorGUI()
    {
        SerializableMetadataLookup serializableMetadataLookup = ((AbilityLookup) target).serializableMetadataLookup;

        if (abilityAttributeDataLookup == null)
        {
            LoadAbilityAttributeData();
        }

        EditorUtils.MultilineLabelField(
            "Ability data depends on the ability attribute. This should be loaded automatically, but if you believe " +
            "there is data missing, the \"Reload\" button below will reload the attribute data."
        );
        if (GUILayout.Button("Reload"))
        {
            LoadAbilityAttributeData();
        }
        
        abilityLookupSanitizer.Sanitize(serializableMetadataLookup, abilityAttributeData);

        EditorUtils.MultilineLabelField("Abilities marked with an asterisk (*) are missing data.");

        foreach (AbilityReference abilityReference in Enum.GetValues(typeof(AbilityReference)))
        {
            EditSerializableAbilityMetadata(abilityReference, serializableMetadataLookup[abilityReference]);
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void LoadAbilityAttributeData()
    {
        abilityAttributeData = ReflectionUtils.GetAttributeData<AbilityAttribute>();
        abilityAttributeDataLookup = abilityAttributeData.ToDictionary(
            d => d.Attribute.AbilityReference,
            d => d
        );
    }

    private void EditSerializableAbilityMetadata(AbilityReference abilityReference, SerializableAbilityMetadata serializableAbilityMetadata)
    {
        foldoutStatus[abilityReference][AbilityDataDropdownGroup.EntireAbility] = EditorGUILayout.Foldout(
            foldoutStatus[abilityReference][AbilityDataDropdownGroup.EntireAbility],
            serializableAbilityMetadata.MissingData ? $"{abilityReference.ToString()}*" : abilityReference.ToString()
        );

        if (!foldoutStatus[abilityReference][AbilityDataDropdownGroup.EntireAbility]) return;

        EditorGUI.indentLevel++;
            
        serializableAbilityMetadata.DisplayName = EditorGUILayout.TextField("Display Name", serializableAbilityMetadata.DisplayName);
        serializableAbilityMetadata.Tooltip = EditorUtils.MultilineTextField("Tooltip", serializableAbilityMetadata.Tooltip, 3);
        serializableAbilityMetadata.Finisher = EditorGUILayout.Toggle("Finisher", serializableAbilityMetadata.Finisher);
        EditAbilityOrbType(serializableAbilityMetadata);
        EditBaseAbilityData(abilityReference, serializableAbilityMetadata);
        EditGeneratedOrbs(abilityReference, serializableAbilityMetadata);
        EditAbilityBonusData(abilityReference, serializableAbilityMetadata);
            
        EditorGUI.indentLevel--;
    }

    private void EditAbilityOrbType(SerializableAbilityMetadata serializableAbilityMetadata)
    {
        if (serializableAbilityMetadata.AbilityOrbType.HasValue)
        {
            GUILayout.BeginHorizontal();

            serializableAbilityMetadata.AbilityOrbType.Value = (OrbType) EditorGUILayout
                .EnumPopup("Ability Orb Type", serializableAbilityMetadata.AbilityOrbType.Value);

            if (GUILayout.Button("Remove Ability Orb Type"))
            {
                serializableAbilityMetadata.AbilityOrbType.HasValue = false;
            }

            GUILayout.EndHorizontal();
        }
        else
        {
            EditorUtils.IndentedButton("Add Ability Orb Type", () =>
            {
                serializableAbilityMetadata.AbilityOrbType.HasValue = true;
                serializableAbilityMetadata.AbilityOrbType.Value = default;
            });
        }
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
        SerializableAbilityBonusLookup serializableAbilityBonusLookup = serializableAbilityMetadata.AbilityBonusLookup;
        
        foldoutStatus[abilityReference][AbilityDataDropdownGroup.BonusData] = EditorGUILayout.Foldout(
            foldoutStatus[abilityReference][AbilityDataDropdownGroup.BonusData],
            serializableAbilityBonusLookup.MissingData ? "Bonus Data*" : "Bonus Data"
        );

        if (!foldoutStatus[abilityReference][AbilityDataDropdownGroup.BonusData]) return;

        if (abilityAttributeDataLookup[abilityReference].Attribute.AbilityBonuses.Length == 0)
        {
            EditorGUI.indentLevel++;
            
            EditorUtils.MultilineLabelField($"There are no ability bonuses for {abilityReference.ToString()}.");
            
            EditorGUI.indentLevel--;
            return;
        }

        EditorGUI.indentLevel++;

        foreach (string abilityBonus in abilityAttributeDataLookup[abilityReference].Attribute.AbilityBonuses)
        {
            EditorGUILayout.LabelField(abilityBonus);
            EditorGUI.indentLevel++;

            serializableAbilityBonusLookup[abilityBonus].DisplayName = EditorGUILayout.TextField(
                "Display Name",
                serializableAbilityBonusLookup[abilityBonus].DisplayName
            );

            serializableAbilityBonusLookup[abilityBonus].Tooltip = EditorUtils.MultilineTextField(
                "Tooltip",
                serializableAbilityBonusLookup[abilityBonus].Tooltip,
                3
            );

            EditOrbList(serializableAbilityBonusLookup[abilityBonus].RequiredOrbs, "Add Required Orb");
                
            EditorGUI.indentLevel--;
        }

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

        EditorUtils.IndentedButton(buttonLabel, () => orbTypes.Insert(0, default));
    }
}
