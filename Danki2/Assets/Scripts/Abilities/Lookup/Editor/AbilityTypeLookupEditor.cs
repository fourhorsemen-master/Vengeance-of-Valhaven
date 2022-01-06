using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityTypeLookup))]
public class AbilityTypeLookupEditor : Editor
{
    private EnumDictionary<AbilityType, bool> foldoutStatus = new EnumDictionary<AbilityType, bool>(false);
    
    private AbilityTypeLookup abilityTypeLookup;

    public override void OnInspectorGUI()
    {
        abilityTypeLookup = (AbilityTypeLookup) target;
        
        EnumUtils.ForEach<AbilityType>(EditAbilityType);
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditAbilityType(AbilityType abilityType)
    {
        foldoutStatus[abilityType] = EditorGUILayout.Foldout(
            foldoutStatus[abilityType],
            abilityType.ToString()
        );
        if (!foldoutStatus[abilityType]) return;

        EditIcon(abilityType);
        EditCollisionSoundLevel(abilityType);
        EditAbilityVocalisationType(abilityType);
        EditAbilityFmodEvents(abilityType);
    }

    private void EditIcon(AbilityType abilityType)
    {
        abilityTypeLookup.abilityIconDictionary[abilityType] = (Sprite)EditorGUILayout.ObjectField(
            "Icon",
            abilityTypeLookup.abilityIconDictionary[abilityType],
            typeof(Sprite),
            false
        );
    }

    private void EditCollisionSoundLevel(AbilityType abilityType)
    {
        abilityTypeLookup.abilityCollisionSoundLevelDictionary[abilityType] = (CollisionSoundLevel) EditorGUILayout.EnumPopup(
            "Collision Sound Level",
            abilityTypeLookup.abilityCollisionSoundLevelDictionary[abilityType]
        );
    }

    private void EditAbilityVocalisationType(AbilityType abilityType)
    {
        abilityTypeLookup.abilityVocalisationTypeDictionary[abilityType] = (AbilityVocalisationType) EditorGUILayout.EnumPopup(
            "Vocalisation Type",
            abilityTypeLookup.abilityVocalisationTypeDictionary[abilityType]
        );
    }

    private void EditAbilityFmodEvents(AbilityType abilityType)
    {
        SerializedProperty keys = serializedObject.FindProperty("abilityFmodEventDictionary._keys");
        int valueIndex = 0;
        for (int i = 0; i < keys.arraySize; i++)
        {
            SerializedProperty key = keys.GetArrayElementAtIndex(i);
            if (key.stringValue != abilityType.ToString()) continue;
            valueIndex = i;
            break;
        }

        SerializedProperty serializedAbilityFmodEvents = serializedObject.FindProperty("abilityFmodEventDictionary._values").GetArrayElementAtIndex(valueIndex);

        EditorUtils.Header("Fmod Events:");

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(serializedAbilityFmodEvents.FindPropertyRelative("fmodStartEventRef"), new GUIContent("Start"));
        if (EditorUtils.IndentedButton("Clear")) abilityTypeLookup.abilityFmodEventDictionary[abilityType].FmodStartEventRef = null;

        EditorGUILayout.PropertyField(serializedAbilityFmodEvents.FindPropertyRelative("fmodSwingEventRef"), new GUIContent("Swing"));
        if (EditorUtils.IndentedButton("Clear")) abilityTypeLookup.abilityFmodEventDictionary[abilityType].FmodSwingEventRef = null;

        EditorGUILayout.PropertyField(serializedAbilityFmodEvents.FindPropertyRelative("fmodImpactEventRef"), new GUIContent("Impact"));
        if (EditorUtils.IndentedButton("Clear")) abilityTypeLookup.abilityFmodEventDictionary[abilityType].FmodImpactEventRef = null;

        EditorGUI.indentLevel--;
    }
}
