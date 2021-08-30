using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerationLookup))]
public class MapGenerationLookupEditor : Editor
{
    private MapGenerationLookup mapGenerationLookup;
    
    private readonly EnumDictionary<RoomType, bool> roomTypeFoldoutStatus = new EnumDictionary<RoomType, bool>(false);
    private readonly EnumDictionary<Zone, bool> spawnedEnemiesPerZoneFoldoutStatus = new EnumDictionary<Zone, bool>(false);
    
    public override void OnInspectorGUI()
    {
        mapGenerationLookup = (MapGenerationLookup) target;

        EditorUtils.ShowScriptLink(mapGenerationLookup);

        mapGenerationLookup.AbilityNameStore = AbilityNameStoreUtils.EditAbilityNameStore(mapGenerationLookup.AbilityNameStore);
        EditLayoutData();
        EditorUtils.VerticalSpace();
        EditRoomDataLookup();
        EditorUtils.VerticalSpace();
        EditSpawnedEnemiesPerZoneLookup();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void EditLayoutData()
    {
        EditorUtils.Header("Player Data");
        EditorGUI.indentLevel++;

        mapGenerationLookup.AbilityChoices = EditorGUILayout.IntField("Ability Choices", mapGenerationLookup.AbilityChoices);
        mapGenerationLookup.RuneSockets = EditorGUILayout.IntField("Rune Sockets", mapGenerationLookup.RuneSockets);
        mapGenerationLookup.LeftStartingAbilityName = AbilityNameStoreUtils.EditAbilityName(
            "Left Starting Ability Name",
            mapGenerationLookup.LeftStartingAbilityName,
            mapGenerationLookup.AbilityNameStore
        );
        mapGenerationLookup.RightStartingAbilityName = AbilityNameStoreUtils.EditAbilityName(
            "Right Starting Ability Name",
            mapGenerationLookup.RightStartingAbilityName,
            mapGenerationLookup.AbilityNameStore
        );
        
        EditorGUI.indentLevel--;
        
        EditorUtils.Header("Layout Data");
        EditorGUI.indentLevel++;

        mapGenerationLookup.GeneratedRoomDepth = EditorGUILayout.IntField("Generated Room Depth", mapGenerationLookup.GeneratedRoomDepth);
        mapGenerationLookup.RequiredSpawners = EditorGUILayout.IntField("Required Spawners", mapGenerationLookup.RequiredSpawners);
        mapGenerationLookup.MinRoomExits = EditorGUILayout.IntField("Min Room Exits", mapGenerationLookup.MinRoomExits);
        mapGenerationLookup.MaxRoomExits = EditorGUILayout.IntField("Max Room Exits", mapGenerationLookup.MaxRoomExits);
        mapGenerationLookup.ChanceIndicatesChildRoomType = EditorGUILayout.FloatField(
            "Chance Room Type Indicated By Parent",
            mapGenerationLookup.ChanceIndicatesChildRoomType
        );
        mapGenerationLookup.ChanceIndicatesGrandchildRoomType = EditorGUILayout.FloatField(
            "Chance Room Type Indicated By Grandparent",
            mapGenerationLookup.ChanceIndicatesGrandchildRoomType
        );
        EditRoomsPerZoneLookup();

        EditorGUI.indentLevel--;
    }

    private void EditRoomsPerZoneLookup()
    {
        EditorUtils.Header("Rooms Per Zone");
        EditorGUI.indentLevel++;
        
        EnumUtils.ForEach<Zone>(zone => mapGenerationLookup.RoomsPerZoneLookup[zone] = EditorGUILayout.IntField(
            zone.ToString(),
            mapGenerationLookup.RoomsPerZoneLookup[zone]
        ));
        
        EditorGUI.indentLevel--;
    }

    private void EditRoomDataLookup()
    {
        EditorUtils.Header("Room Data");
        EditorGUI.indentLevel++;

        EnumUtils.ForEach<RoomType>(roomType =>
        {
            roomTypeFoldoutStatus[roomType] = EditorGUILayout.Foldout(roomTypeFoldoutStatus[roomType], roomType.ToString());
            if (!roomTypeFoldoutStatus[roomType]) return;

            EditorGUI.indentLevel++;
            
            EditRoomData(mapGenerationLookup.RoomDataLookup[roomType]);

            EditorGUI.indentLevel--;
            EditorUtils.VerticalSpace();
        });

        EditorGUI.indentLevel--;
    }

    private void EditRoomData(RoomData roomData)
    {
        roomData.AvailableInPool = EditorGUILayout.Toggle("Available In Pool", roomData.AvailableInPool);
        EditorGUI.BeginDisabledGroup(!roomData.AvailableInPool);

        EditorUtils.Header("Weights");
        EditorGUI.indentLevel++;

        int index = 1;
        EditorUtils.ResizeableList(
            roomData.Weights,
            weight => EditorGUILayout.IntField($"Weight {index++}", weight),
            0
        );
        
        EditorGUI.indentLevel--;
        
        EditorGUI.EndDisabledGroup();
    }

    private void EditSpawnedEnemiesPerZoneLookup()
    {
        EditorUtils.Header("Spawned Enemies Per Zone");
        EditorGUI.indentLevel++;

        int startDepth = 2;
        int endDepth = -1;
        EnumUtils.ForEach<Zone>(zone =>
        {
            endDepth += mapGenerationLookup.RoomsPerZoneLookup[zone];
            spawnedEnemiesPerZoneFoldoutStatus[zone] = EditorGUILayout.Foldout(spawnedEnemiesPerZoneFoldoutStatus[zone], zone.ToString());

            if (spawnedEnemiesPerZoneFoldoutStatus[zone])
            {
                EditorGUI.indentLevel++;

                for (int i = startDepth; i <= endDepth; i++)
                {
                    if (!mapGenerationLookup.SpawnedEnemiesPerDepthLookup.ContainsKey(i))
                    {
                        mapGenerationLookup.SpawnedEnemiesPerDepthLookup[i] = new SpawnedEnemiesWrapper();
                    }
                    SpawnedEnemiesWrapper spawnedEnemiesWrapper = mapGenerationLookup.SpawnedEnemiesPerDepthLookup[i];
                    EditActorList(spawnedEnemiesWrapper, i);
                }
                
                EditorGUI.indentLevel--;
            }

            startDepth += mapGenerationLookup.RoomsPerZoneLookup[zone];
        });

        EditorGUI.indentLevel--;
    }

    private void EditActorList(SpawnedEnemiesWrapper spawnedEnemiesWrapper, int depth)
    {
        EditorUtils.Header($"Room {depth}");
        EditorGUI.indentLevel++;
        
        spawnedEnemiesWrapper.SpawnedEnemies.Trim(mapGenerationLookup.RequiredSpawners);
        EditorUtils.ResizeableList(
            spawnedEnemiesWrapper.SpawnedEnemies,
            actorType => (ActorType) EditorGUILayout.EnumPopup("Actor Type", actorType),
            ActorType.Wolf,
            maxSize: mapGenerationLookup.RequiredSpawners
        );
        
        EditorGUI.indentLevel--;
    }
}
