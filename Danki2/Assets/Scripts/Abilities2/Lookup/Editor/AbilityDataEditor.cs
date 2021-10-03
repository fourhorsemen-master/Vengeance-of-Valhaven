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

        for (int i = abilityData.abilityIds.Count - 1; i >= 0; i--)
        {
            EditAbilityData(abilityData.abilityIds[i]);
            EditorUtils.VerticalSpace();
        }
        AddAbilityButton();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void InitialiseFoldoutStatuses()
    {
        foldoutStatus = abilityData.abilityIds.ToDictionary(abilityId => abilityId, abilityId => false);
    }

    private void EditAbilityData(SerializableGuid abilityId)
    {
        foldoutStatus[abilityId] = EditorGUILayout.Foldout(
            foldoutStatus[abilityId],
            abilityData.abilityDisplayNameDictionary[abilityId]
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
        abilityData.abilityIconDictionary[abilityId] = (Sprite) EditorGUILayout.ObjectField(
            "Icon",
            abilityData.abilityIconDictionary[abilityId],
            typeof(Sprite),
            false
        );
    }

    private void EditDisplayName(SerializableGuid abilityId)
    {
        abilityData.abilityDisplayNameDictionary[abilityId] = EditorGUILayout.TextField(
            "Display Name",
            abilityData.abilityDisplayNameDictionary[abilityId]
        );
    }

    private void EditType(SerializableGuid abilityId)
    {
        abilityData.abilityTypeDictionary[abilityId] = (AbilityType2) EditorGUILayout.EnumPopup(
            "Type",
            abilityData.abilityTypeDictionary[abilityId]
        );
    }

    private void EditDamage(SerializableGuid abilityId)
    {
        abilityData.abilityDamageDictionary[abilityId] = EditorGUILayout.IntField(
            "Damage",
            abilityData.abilityDamageDictionary[abilityId]
        );
    }

    private void EditRarity(SerializableGuid abilityId)
    {
        abilityData.abilityRarityDictionary[abilityId] = (Rarity) EditorGUILayout.EnumPopup(
            "Rarity",
            abilityData.abilityRarityDictionary[abilityId]
        );
    }

    private void EditEmpowerments(SerializableGuid abilityId)
    {
        EditorUtils.Header("Empowerments");
        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            abilityData.abilityEmpowermentsDictionary[abilityId].Empowerments,
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
            abilityData.abilityIds.Insert(0, abilityId);
            abilityData.abilityDisplayNameDictionary[abilityId] = "";
            abilityData.abilityTypeDictionary[abilityId] = AbilityType2.Slash;
            abilityData.abilityDamageDictionary[abilityId] = 0;
            abilityData.abilityEmpowermentsDictionary[abilityId] = new EmpowermentsWrapper();
            abilityData.abilityRarityDictionary[abilityId] = Rarity.Common;
            abilityData.abilityIconDictionary[abilityId] = null;
            foldoutStatus[abilityId] = false;
        }
    }

    private void RemoveAbilityButton(SerializableGuid abilityId)
    {
        if (EditorUtils.IndentedButton("Remove Ability"))
        {
            abilityData.abilityIds.Remove(abilityId);
            abilityData.abilityDisplayNameDictionary.Remove(abilityId);
            abilityData.abilityTypeDictionary.Remove(abilityId);
            abilityData.abilityDamageDictionary.Remove(abilityId);
            abilityData.abilityEmpowermentsDictionary.Remove(abilityId);
            abilityData.abilityRarityDictionary.Remove(abilityId);
            abilityData.abilityIconDictionary.Remove(abilityId);
            foldoutStatus.Remove(abilityId);
        }
    }
}
