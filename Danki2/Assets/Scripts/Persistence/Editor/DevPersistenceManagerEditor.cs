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
        devPersistenceManager.currencyAmount = EditorGUILayout.IntField("Currency Amount", devPersistenceManager.currencyAmount);
        EditRuneSockets(devPersistenceManager);
        EditRuneOrder(devPersistenceManager);
        EditorGUI.indentLevel--;
        EditorUtils.VerticalSpace();
        
        EditorUtils.Header("Ability Tree");
        EditorGUI.indentLevel++;
        devPersistenceManager.ownedAbilityCount = EditorGUILayout.IntField("Owned Ability Count", devPersistenceManager.ownedAbilityCount);
        devPersistenceManager.leftAbility = (Ability2) EditorGUILayout.EnumPopup("Left Ability", devPersistenceManager.leftAbility);
        devPersistenceManager.rightAbility = (Ability2) EditorGUILayout.EnumPopup("Left Ability", devPersistenceManager.rightAbility);
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
        devPersistenceManager.depth = EditorGUILayout.IntField("Depth", devPersistenceManager.depth);
        devPersistenceManager.zone = (Zone) EditorGUILayout.EnumPopup("Zone", devPersistenceManager.zone);
        devPersistenceManager.depthInZone = EditorGUILayout.IntField("Depth In Zone", devPersistenceManager.depthInZone);
        devPersistenceManager.roomType = (RoomType) EditorGUILayout.EnumPopup("Room Type", devPersistenceManager.roomType);
        if (devPersistenceManager.roomType == RoomType.Combat) EditCombatRoomData(devPersistenceManager);
        if (devPersistenceManager.roomType == RoomType.Ability) EditAbilityRoomData(devPersistenceManager);
        if (devPersistenceManager.roomType == RoomType.Healing) EditHealingRoomData(devPersistenceManager);
        EditorGUI.indentLevel--;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditRuneSockets(DevPersistenceManager devPersistenceManager)
    {
        EditorUtils.Header("Rune Sockets");
        EditorGUI.indentLevel++;

        EditorUtils.ResizeableList(
            devPersistenceManager.runeSockets,
            runeSocket =>
            {
                runeSocket.HasRune = EditorGUILayout.Toggle("Has Rune", runeSocket.HasRune);
                if (!runeSocket.HasRune) EditorGUI.BeginDisabledGroup(true);
                runeSocket.Rune = (Rune) EditorGUILayout.EnumPopup("Rune", runeSocket.Rune);
                if (!runeSocket.HasRune) EditorGUI.EndDisabledGroup();
            },
            () => new RuneSocket()
        );

        EditorGUI.indentLevel--;
    }

    private void EditRuneOrder(DevPersistenceManager devPersistenceManager)
    {
        EditorUtils.Header("Rune Order");
        EditorGUI.indentLevel++;

        EditorUtils.ResizeableList(
            devPersistenceManager.runeOrder,
            rune => (Rune) EditorGUILayout.EnumPopup("Rune", rune),
            Rune.DeepWounds
        );

        EditorGUI.indentLevel--;
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
        devPersistenceManager.enemiesCleared = EditorGUILayout.Toggle("Enemies Cleared", devPersistenceManager.enemiesCleared);

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
        EditorUtils.Header("Ability Choices");

        EditorGUI.indentLevel++;

        EditorUtils.ResizeableList(
            devPersistenceManager.abilityChoices,
            abilityChoice => (Ability2) EditorGUILayout.EnumPopup("Ability Choice", abilityChoice),
            Ability2.Slash1
        );

        EditorGUI.indentLevel--;
    }

    private void EditHealingRoomData(DevPersistenceManager devPersistenceManager)
    {
        devPersistenceManager.hasHealed = EditorGUILayout.Toggle("Has Healed", devPersistenceManager.hasHealed);
    }
}
