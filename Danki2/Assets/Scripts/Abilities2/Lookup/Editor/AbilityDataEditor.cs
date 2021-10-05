using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityData))]
public class AbilityDataEditor : Editor
{
    private Dictionary<SerializableGuid, bool> foldoutStatus;
    
    private AbilityData abilityData;

    public override void OnInspectorGUI()
    {
        abilityData = (AbilityData) target;
        
        if (foldoutStatus == null) InitialiseFoldoutStatuses();

        for (int i = abilityData.AbilityIds.Count - 1; i >= 0; i--)
        {
            EditAbilityData(abilityData.AbilityIds[i]);
            EditorUtils.VerticalSpace();
        }
        AddAbilityButton();
        
        if (GUILayout.Button("Save")) AssetDatabase.SaveAssets();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void InitialiseFoldoutStatuses()
    {
        foldoutStatus = abilityData.AbilityIds.ToDictionary(abilityId => abilityId, abilityId => false);
    }

    private void EditAbilityData(SerializableGuid abilityId)
    {
        foldoutStatus[abilityId] = EditorGUILayout.Foldout(
            foldoutStatus[abilityId],
            abilityData.AbilityDisplayNameDictionary[abilityId]
        );
        if (!foldoutStatus[abilityId]) return;

        EditorGUI.indentLevel++;

        EditorUtils.MultilineLabelField($"ID: \"{abilityId}\"");

        EditIcon(abilityId);
        EditDisplayName(abilityId);
        EditType(abilityId);
        EditDamage(abilityId);
        EditRarity(abilityId);
        EditEmpowerments(abilityId);

        RemoveAbilityButton(abilityId);

        EditorGUI.indentLevel--;
    }

    private void EditIcon(SerializableGuid abilityId)
    {
        abilityData.AbilityIconDictionary[abilityId] = (Sprite) EditorGUILayout.ObjectField(
            "Icon",
            abilityData.AbilityIconDictionary[abilityId],
            typeof(Sprite),
            false
        );
    }

    private void EditDisplayName(SerializableGuid abilityId)
    {
        abilityData.AbilityDisplayNameDictionary[abilityId] = EditorGUILayout.TextField(
            "Display Name",
            abilityData.AbilityDisplayNameDictionary[abilityId]
        );
    }

    private void EditType(SerializableGuid abilityId)
    {
        abilityData.AbilityTypeDictionary[abilityId] = (AbilityType2) EditorGUILayout.EnumPopup(
            "Type",
            abilityData.AbilityTypeDictionary[abilityId]
        );
    }

    private void EditDamage(SerializableGuid abilityId)
    {
        abilityData.AbilityDamageDictionary[abilityId] = EditorGUILayout.IntField(
            "Damage",
            abilityData.AbilityDamageDictionary[abilityId]
        );
    }

    private void EditRarity(SerializableGuid abilityId)
    {
        abilityData.AbilityRarityDictionary[abilityId] = (Rarity) EditorGUILayout.EnumPopup(
            "Rarity",
            abilityData.AbilityRarityDictionary[abilityId]
        );
    }

    private void EditEmpowerments(SerializableGuid abilityId)
    {
        EditorUtils.Header("Empowerments");
        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            abilityData.AbilityEmpowermentsDictionary[abilityId].Empowerments,
            empowerment => (Empowerment) EditorGUILayout.EnumPopup("Empowerment", empowerment),
            defaultValue: default
        );
        EditorGUI.indentLevel--;
    }

    private void AddAbilityButton()
    {
        if (GUILayout.Button("Add Ability"))
        {
            SerializableGuid abilityId = SerializableGuid.NewGuid();
            abilityData.AbilityIds.Insert(0, abilityId);
            abilityData.AbilityDisplayNameDictionary[abilityId] = "";
            abilityData.AbilityTypeDictionary[abilityId] = AbilityType2.Slash;
            abilityData.AbilityDamageDictionary[abilityId] = 0;
            abilityData.AbilityEmpowermentsDictionary[abilityId] = new EmpowermentsWrapper();
            abilityData.AbilityRarityDictionary[abilityId] = Rarity.Common;
            abilityData.AbilityIconDictionary[abilityId] = null;
            foldoutStatus[abilityId] = false;
        }
    }

    private void RemoveAbilityButton(SerializableGuid abilityId)
    {
        if (EditorUtils.IndentedButton("Remove Ability"))
        {
            abilityData.AbilityIds.Remove(abilityId);
            abilityData.AbilityDisplayNameDictionary.Remove(abilityId);
            abilityData.AbilityTypeDictionary.Remove(abilityId);
            abilityData.AbilityDamageDictionary.Remove(abilityId);
            abilityData.AbilityEmpowermentsDictionary.Remove(abilityId);
            abilityData.AbilityRarityDictionary.Remove(abilityId);
            abilityData.AbilityIconDictionary.Remove(abilityId);
            foldoutStatus.Remove(abilityId);
        }
    }
}
