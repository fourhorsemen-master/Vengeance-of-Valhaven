using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityLookup2))]
public class AbilityLookup2Editor : Editor
{
    private readonly EnumDictionary<Ability2, bool> foldoutStatus = new EnumDictionary<Ability2, bool>(false);
    
    private AbilityLookup2 abilityLookup;
    
    public override void OnInspectorGUI()
    {
        abilityLookup = (AbilityLookup2) target;

        EditorUtils.ShowScriptLink(abilityLookup);

        ExpandAndCollapseAll();
        
        EnumUtils.ForEach<Ability2>(EditAbilityData);
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void ExpandAndCollapseAll()
    {
        GUILayout.BeginHorizontal();
        if (foldoutStatus.Values.All(value => value))
        {
            if (GUILayout.Button("Collapse All")) SetAllFoldoutStatuses(false);
        }
        else
        {
            if (GUILayout.Button("Expand All")) SetAllFoldoutStatuses(true);
        }
        GUILayout.EndHorizontal();
    }

    private void SetAllFoldoutStatuses(bool value)
    {
        EnumUtils.ForEach<Ability2>(ability => foldoutStatus[ability] = value);
    }

    private void EditAbilityData(Ability2 ability)
    {
        foldoutStatus[ability] = EditorGUILayout.Foldout(foldoutStatus[ability], ability.ToString());
        if (!foldoutStatus[ability]) return;

        EditorGUI.indentLevel++;

        EditDisplayName(ability);
        EditType(ability);
        EditDamage(ability);
        EditEmpowerments(ability);
        EditRarity(ability);
        EditCollisionSoundLevel(ability);
        EditIcon(ability);

        EditorGUI.indentLevel--;
    }

    private void EditDisplayName(Ability2 ability)
    {
        abilityLookup.abilityDisplayNameDictionary[ability] = EditorGUILayout.TextField(
            "Display Name",
            abilityLookup.abilityDisplayNameDictionary[ability]
        );
    }

    private void EditType(Ability2 ability)
    {
        abilityLookup.abilityTypeDictionary[ability] = (AbilityType2) EditorGUILayout.EnumPopup(
            "Type",
            abilityLookup.abilityTypeDictionary[ability]
        );
    }

    private void EditDamage(Ability2 ability)
    {
        abilityLookup.abilityDamageDictionary[ability] = EditorGUILayout.IntField(
            "Damage",
            abilityLookup.abilityDamageDictionary[ability]
        );
    }

    private void EditEmpowerments(Ability2 ability)
    {
        EditorUtils.Header("Empowerments");
        EditorGUI.indentLevel++;
        EditorUtils.ResizeableList(
            abilityLookup.abilityEmpowermentsDictionary[ability].Empowerments,
            empowerment => (Empowerment) EditorGUILayout.EnumPopup("Empowerment", empowerment),
            defaultValue: default
        );
        EditorGUI.indentLevel--;
    }

    private void EditRarity(Ability2 ability)
    {
        abilityLookup.abilityRarityDictionary[ability] = (Rarity) EditorGUILayout.EnumPopup(
            "Rarity",
            abilityLookup.abilityRarityDictionary[ability]
        );
    }

    private void EditCollisionSoundLevel(Ability2 ability)
    {
        abilityLookup.abilityCollisionSoundLevelDictionary[ability] = (CollisionSoundLevel) EditorGUILayout.EnumPopup(
            "Collision Sound Level",
            abilityLookup.abilityCollisionSoundLevelDictionary[ability]
        );
    }

    private void EditIcon(Ability2 ability)
    {
        abilityLookup.abilityIconDictionary[ability] = (Sprite) EditorGUILayout.ObjectField(
            "Icon",
            abilityLookup.abilityIconDictionary[ability],
            typeof(Sprite),
            false
        );
    }
}
