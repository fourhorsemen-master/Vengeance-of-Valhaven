using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DevPersistenceManager))]
public class DevPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DevPersistenceManager devPersistenceManager = (DevPersistenceManager) target;

        EditorUtils.ShowScriptLink(devPersistenceManager);

        EditorUtils.Header("Player");
        EditorGUI.indentLevel++;
        devPersistenceManager.playerSpawnerId = EditorGUILayout.IntField("Player Spawner ID", devPersistenceManager.playerSpawnerId);
        devPersistenceManager.playerHealth = EditorGUILayout.IntField("Player Health", devPersistenceManager.playerHealth);
        EditorGUI.indentLevel--;
        EditorUtils.VerticalSpace();
        
        EditorUtils.Header("Ability Tree");
        EditorGUI.indentLevel++;
        devPersistenceManager.ownedAbilityCount = EditorGUILayout.IntField("Owned Ability Count", devPersistenceManager.ownedAbilityCount);
        devPersistenceManager.leftAbility = (AbilityReference) EditorGUILayout.EnumPopup("Left Ability", devPersistenceManager.leftAbility);
        devPersistenceManager.rightAbility = (AbilityReference) EditorGUILayout.EnumPopup("Left Ability", devPersistenceManager.rightAbility);
        EditorGUI.indentLevel--;
        EditorUtils.VerticalSpace();
        
        EditorUtils.Header("Scene");
        EditorGUI.indentLevel++;
        devPersistenceManager.cameraOrientation = (Pole) EditorGUILayout.EnumPopup("Camera Orientation", devPersistenceManager.cameraOrientation);
        devPersistenceManager.useRandomSeeds = EditorGUILayout.Toggle("Use Random Seeds", devPersistenceManager.useRandomSeeds);
        if (devPersistenceManager.useRandomSeeds) EditorGUI.BeginDisabledGroup(true);
        devPersistenceManager.moduleSeed = EditorGUILayout.IntField("Module Seed", devPersistenceManager.moduleSeed);
        devPersistenceManager.transitionModuleSeed = EditorGUILayout.IntField("Transitions Module Seed", devPersistenceManager.transitionModuleSeed);
        if (devPersistenceManager.useRandomSeeds) EditorGUI.EndDisabledGroup();
        EditTransitions(devPersistenceManager);
        EditorGUI.indentLevel--;
        EditorUtils.VerticalSpace();
        
        EditorUtils.Header("Room");
        EditorGUI.indentLevel++;
        devPersistenceManager.roomType = (RoomType) EditorGUILayout.EnumPopup("Room Type", devPersistenceManager.roomType);
        if (devPersistenceManager.roomType == RoomType.Combat) EditCombatRoomData(devPersistenceManager);
        if (devPersistenceManager.roomType == RoomType.Ability) EditAbilityRoomData(devPersistenceManager);
        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditTransitions(DevPersistenceManager devPersistenceManager)
    {
        EditorUtils.Header("Active Transitions");
        EditorGUI.indentLevel++;

        EditorUtils.ResizeableList(
            devPersistenceManager.activeTransitions,
            t => EditorGUILayout.IntField("Transitioner ID", t),
            0
        );

        EditorGUI.indentLevel--;
    }

    private void EditCombatRoomData(DevPersistenceManager devPersistenceManager)
    {
        EditorUtils.Header("Spawned Enemies");
        EditorGUI.indentLevel++;

        EditorUtils.ResizeableList(
            devPersistenceManager.spawnedEnemies,
            spawnedEnemy =>
            {
                spawnedEnemy.SpawnerId = EditorGUILayout.IntField("Spawner ID", spawnedEnemy.SpawnerId);
                spawnedEnemy.ActorType = (ActorType) EditorGUILayout.EnumPopup("Actor Type", spawnedEnemy.ActorType);
            },
            () => new DevPersistenceManager.SpawnedEnemy()
        );
        
        EditorGUI.indentLevel--;
    }

    private void EditAbilityRoomData(DevPersistenceManager devPersistenceManager)
    {
        devPersistenceManager.abilityChoice1 = (AbilityReference) EditorGUILayout.EnumPopup("Ability Choice 1", devPersistenceManager.abilityChoice1);
        devPersistenceManager.abilityChoice2 = (AbilityReference) EditorGUILayout.EnumPopup("Ability Choice 2", devPersistenceManager.abilityChoice2);
        devPersistenceManager.abilityChoice3 = (AbilityReference) EditorGUILayout.EnumPopup("Ability Choice 3", devPersistenceManager.abilityChoice3);
    }
}
