using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityLookup2))]
public class AbilityLookup2Editor : Editor
{
    private Dictionary<SerializableGuid, bool> foldoutStatus;
    
    private AbilityLookup2 abilityLookup;

    public override void OnInspectorGUI()
    {
        abilityLookup = (AbilityLookup2) target;

        EditorUtils.ShowScriptLink(abilityLookup);
        
        if (foldoutStatus == null) InitialiseFoldoutStatuses();

        for (int i = abilityLookup.abilityIds.Count - 1; i >= 0; i--)
        {
            EditAbilityData(abilityLookup.abilityIds[i]);
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
        foldoutStatus = abilityLookup.abilityIds.ToDictionary(abilityId => abilityId, abilityId => false);
    }

    private void EditAbilityData(SerializableGuid abilityId)
    {
        foldoutStatus[abilityId] = EditorGUILayout.Foldout(
            foldoutStatus[abilityId],
            abilityLookup.abilityDisplayNameDictionary[abilityId]
        );
        if (!foldoutStatus[abilityId]) return;

        EditorGUI.indentLevel++;

        EditorUtils.MultilineLabelField($"ID: \"{abilityId}\"");

        EditDisplayName(abilityId);
        EditType(abilityId);
        EditDamage(abilityId);
        EditEmpowerments(abilityId);
        EditRarity(abilityId);
        EditCollisionSoundLevel(abilityId);
        EditIcon(abilityId);

        RemoveAbilityButton(abilityId);

        EditorGUI.indentLevel--;
    }

    private void EditDisplayName(SerializableGuid abilityId)
    {
        abilityLookup.abilityDisplayNameDictionary[abilityId] = EditorGUILayout.TextField(
            "Display Name",
            abilityLookup.abilityDisplayNameDictionary[abilityId]
        );
    }

    private void EditType(SerializableGuid abilityId)
    {
        abilityLookup.abilityTypeDictionary[abilityId] = (AbilityType2) EditorGUILayout.EnumPopup(
            "Type",
            abilityLookup.abilityTypeDictionary[abilityId]
        );
    }

    private void EditDamage(SerializableGuid abilityId)
    {
        abilityLookup.abilityDamageDictionary[abilityId] = EditorGUILayout.IntField(
            "Damage",
            abilityLookup.abilityDamageDictionary[abilityId]
        );
    }

    private void EditEmpowerments(SerializableGuid abilityId)
    {
        EditorUtils.Header("Empowerments");
        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            abilityLookup.abilityEmpowermentsDictionary[abilityId].Empowerments,
            empowerment => (Empowerment) EditorGUILayout.EnumPopup("Empowerment", empowerment),
            defaultValue: default
        );
        EditorGUI.indentLevel--;
    }

    private void EditRarity(SerializableGuid abilityId)
    {
        abilityLookup.abilityRarityDictionary[abilityId] = (Rarity) EditorGUILayout.EnumPopup(
            "Rarity",
            abilityLookup.abilityRarityDictionary[abilityId]
        );
    }

    private void EditCollisionSoundLevel(SerializableGuid abilityId)
    {
        abilityLookup.abilityCollisionSoundLevelDictionary[abilityId] = (CollisionSoundLevel) EditorGUILayout.EnumPopup(
            "Collision Sound Level",
            abilityLookup.abilityCollisionSoundLevelDictionary[abilityId]
        );
    }

    private void EditIcon(SerializableGuid abilityId)
    {
        abilityLookup.abilityIconDictionary[abilityId] = (Sprite) EditorGUILayout.ObjectField(
            "Icon",
            abilityLookup.abilityIconDictionary[abilityId],
            typeof(Sprite),
            false
        );
    }

    private void AddAbilityButton()
    {
        if (GUILayout.Button("Add Ability"))
        {
            SerializableGuid abilityId = SerializableGuid.NewGuid();
            abilityLookup.abilityIds.Insert(0, abilityId);
            abilityLookup.abilityDisplayNameDictionary[abilityId] = "";
            abilityLookup.abilityTypeDictionary[abilityId] = AbilityType2.Slash;
            abilityLookup.abilityDamageDictionary[abilityId] = 0;
            abilityLookup.abilityEmpowermentsDictionary[abilityId] = new EmpowermentsWrapper();
            abilityLookup.abilityRarityDictionary[abilityId] = Rarity.Common;
            abilityLookup.abilityCollisionSoundLevelDictionary[abilityId] = CollisionSoundLevel.Low;
            abilityLookup.abilityIconDictionary[abilityId] = null;
            foldoutStatus[abilityId] = false;
        }
    }

    private void RemoveAbilityButton(SerializableGuid abilityId)
    {
        if (EditorUtils.IndentedButton("Remove Ability"))
        {
            abilityLookup.abilityIds.Remove(abilityId);
            abilityLookup.abilityDisplayNameDictionary.Remove(abilityId);
            abilityLookup.abilityTypeDictionary.Remove(abilityId);
            abilityLookup.abilityDamageDictionary.Remove(abilityId);
            abilityLookup.abilityEmpowermentsDictionary.Remove(abilityId);
            abilityLookup.abilityRarityDictionary.Remove(abilityId);
            abilityLookup.abilityCollisionSoundLevelDictionary.Remove(abilityId);
            abilityLookup.abilityIconDictionary.Remove(abilityId);
            foldoutStatus.Remove(abilityId);
        }
    }
}
