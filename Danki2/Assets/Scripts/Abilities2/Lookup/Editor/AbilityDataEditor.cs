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

        EditDisplayName(abilityId);
        EditType(abilityId);
        EditDamage(abilityId);
        EditEmpowerments(abilityId);
        EditRarity(abilityId);
        EditCollisionSoundLevel(abilityId);
        EditVocalisationType(abilityId);
        EditFmodEvents(abilityId);
        EditIcon(abilityId);

        RemoveAbilityButton(abilityId);

        EditorGUI.indentLevel--;
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

    private void EditRarity(SerializableGuid abilityId)
    {
        abilityData.abilityRarityDictionary[abilityId] = (Rarity) EditorGUILayout.EnumPopup(
            "Rarity",
            abilityData.abilityRarityDictionary[abilityId]
        );
    }

    private void EditCollisionSoundLevel(SerializableGuid abilityId)
    {
        abilityData.abilityCollisionSoundLevelDictionary[abilityId] = (CollisionSoundLevel) EditorGUILayout.EnumPopup(
            "Collision Sound Level",
            abilityData.abilityCollisionSoundLevelDictionary[abilityId]
        );
    }

    private void EditVocalisationType(SerializableGuid abilityId)
    {
        abilityData.abilityVocalisationTypeDictionary[abilityId] = (AbilityVocalisationType) EditorGUILayout.EnumPopup(
            "Vocalisation Type",
            abilityData.abilityVocalisationTypeDictionary[abilityId]
        );
    }

    private void EditFmodEvents(SerializableGuid abilityId)
    {
        SerializedProperty keys = serializedObject.FindProperty("abilityFmodEventDictionary._keys");
        int valueIndex = 0;
        for (int i = 0; i < keys.arraySize; i++)
        {
            SerializedProperty serializedGuid = keys.GetArrayElementAtIndex(i);
            if (serializedGuid.FindPropertyRelative("value").stringValue != abilityId.ToString()) continue;
            valueIndex = i;
            break;
        }

        SerializedProperty serializedAbilityFmodEvents = serializedObject.FindProperty("abilityFmodEventDictionary._values").GetArrayElementAtIndex(valueIndex);

        EditorUtils.Header("Fmod Events:");

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(serializedAbilityFmodEvents.FindPropertyRelative("fmodStartEventRef"), new GUIContent("Start"));
        if (EditorUtils.IndentedButton("Clear")) abilityData.abilityFmodEventDictionary[abilityId].FmodStartEventRef = null;

        EditorGUILayout.PropertyField(serializedAbilityFmodEvents.FindPropertyRelative("fmodSwingEventRef"), new GUIContent("Swing"));
        if (EditorUtils.IndentedButton("Clear")) abilityData.abilityFmodEventDictionary[abilityId].FmodSwingEventRef = null;

        EditorGUILayout.PropertyField(serializedAbilityFmodEvents.FindPropertyRelative("fmodImpactEventRef"), new GUIContent("Impact"));
        if (EditorUtils.IndentedButton("Clear")) abilityData.abilityFmodEventDictionary[abilityId].FmodImpactEventRef = null;

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
            abilityData.abilityCollisionSoundLevelDictionary[abilityId] = CollisionSoundLevel.Low;
            abilityData.abilityVocalisationTypeDictionary[abilityId] = AbilityVocalisationType.None;
            abilityData.abilityFmodEventDictionary[abilityId] = new AbilityFmodEvents();
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
            abilityData.abilityCollisionSoundLevelDictionary.Remove(abilityId);
            abilityData.abilityVocalisationTypeDictionary.Remove(abilityId);
            abilityData.abilityFmodEventDictionary.Remove(abilityId);
            abilityData.abilityIconDictionary.Remove(abilityId);
            foldoutStatus.Remove(abilityId);
        }
    }
}
