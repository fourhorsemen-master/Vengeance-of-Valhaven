using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SceneLookupTest
{
    private readonly ISet<RoomType> roomTypesToIgnore = new HashSet<RoomType>
    {
        RoomType.Victory,
        RoomType.Defeat,
        RoomType.Shop, // TODO: Remove this once we are supporting shop rooms.
    };
    
    private readonly ISet<Pole> trueEntranceDirectionsToIgnore = new HashSet<Pole>
    {
        Pole.North,
    };

    [OneTimeSetUp]
    public void SetUp()
    {
        TestUtils.InstantiatePrefab<MapGenerationLookup>();
        TestUtils.InstantiatePrefab<SceneLookup>();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        MapGenerationLookup.Instance.Destroy();
        SceneLookup.Instance.Destroy();
    }

    [UnityTest]
    public IEnumerator TestSceneLookupContainsAllPossibleRoomConfigurations()
    {
        bool hasInvalidConfigurations = false;

        EnumUtils.ForEach<RoomType>(roomType =>
        {
            if (roomTypesToIgnore.Contains(roomType)) return;
            
            EnumUtils.ForEach<Pole>(trueEntranceDirection =>
            {
                if (trueEntranceDirectionsToIgnore.Contains(trueEntranceDirection)) return;

                int minRoomExits = MapGenerationLookup.Instance.MinRoomExits;
                int maxRoomExits = MapGenerationLookup.Instance.MaxRoomExits;
                for (int numberOfExits = minRoomExits; numberOfExits <= maxRoomExits; numberOfExits++)
                {
                    List<Scene> validScenes = SceneLookup.Instance.GetValidScenes(roomType, trueEntranceDirection, numberOfExits);
                    
                    if (validScenes.Count > 0) continue;
                    
                    hasInvalidConfigurations = true;
                    Debug.Log(
                        $"RoomType: {roomType}, " +
                        $"TrueEntranceDirection: {trueEntranceDirection}, " +
                        $"NumberOfExits: {numberOfExits} " +
                        "has no valid scenes. Ensure that there is a scene that accommodates this configuration."
                    );
                }
                
            });
        });

        Assert.False(hasInvalidConfigurations, "Found invalid room configurations, see console for more info.");
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestGameplayScenesHaveDistinctRoomTypes()
    {
        bool hasScenesWithNonDistinctRoomTypes = false;
        
        EnumUtils.ForEach<Scene>(scene =>
        {
            SceneData sceneData = SceneLookup.Instance.sceneDataLookup[scene];
            if (sceneData.SceneType != SceneType.Gameplay) return;
            if (sceneData.GameplaySceneData.RoomTypes.IsDistinct()) return;

            hasScenesWithNonDistinctRoomTypes = true;
            Debug.Log($"Scene: {scene} has room types that are not distinct.");
        });

        Assert.False(
            hasScenesWithNonDistinctRoomTypes,
            "Found scenes with room types that are not distinct, see console for more info."
        );
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestGameplayScenesHaveDistinctCameraOrientations()
    {
        bool hasScenesWithNonDistinctCameraOrientations = false;
        
        EnumUtils.ForEach<Scene>(scene =>
        {
            SceneData sceneData = SceneLookup.Instance.sceneDataLookup[scene];
            if (sceneData.SceneType != SceneType.Gameplay) return;
            if (sceneData.GameplaySceneData.CameraOrientations.IsDistinct()) return;

            hasScenesWithNonDistinctCameraOrientations = true;
            Debug.Log($"Scene: {scene} has camera orientations that are not distinct.");
        });

        Assert.False(
            hasScenesWithNonDistinctCameraOrientations,
            "Found scenes with camera orientations that are not distinct, see console for more info."
        );
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestGameplayScenesHaveDistinctEntranceIds()
    {
        bool hasScenesWithNonDistinctEntranceIds = false;
        
        EnumUtils.ForEach<Scene>(scene =>
        {
            SceneData sceneData = SceneLookup.Instance.sceneDataLookup[scene];
            if (sceneData.SceneType != SceneType.Gameplay) return;
            if (sceneData.GameplaySceneData.EntranceData.IsDistinctById()) return;

            hasScenesWithNonDistinctEntranceIds = true;
            Debug.Log($"Scene: {scene} has entrance IDs that are not distinct.");
        });

        Assert.False(
            hasScenesWithNonDistinctEntranceIds,
            "Found scenes with entrance IDs that are not distinct, see console for more info."
        );
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestGameplayScenesHaveDistinctExitIds()
    {
        bool hasScenesWithNonDistinctExitIds = false;
        
        EnumUtils.ForEach<Scene>(scene =>
        {
            SceneData sceneData = SceneLookup.Instance.sceneDataLookup[scene];
            if (sceneData.SceneType != SceneType.Gameplay) return;
            if (sceneData.GameplaySceneData.ExitData.IsDistinctById()) return;

            hasScenesWithNonDistinctExitIds = true;
            Debug.Log($"Scene: {scene} has exit IDs that are not distinct.");
        });

        Assert.False(
            hasScenesWithNonDistinctExitIds,
            "Found scenes with exit IDs that are not distinct, see console for more info."
        );
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestGameplayScenesHaveDistinctEnemySpawnerIds()
    {
        bool hasScenesWithNonDistinctEnemySpawnerIds = false;
        
        EnumUtils.ForEach<Scene>(scene =>
        {
            SceneData sceneData = SceneLookup.Instance.sceneDataLookup[scene];
            if (sceneData.SceneType != SceneType.Gameplay) return;
            if (sceneData.GameplaySceneData.EnemySpawnerIds.IsDistinct()) return;

            hasScenesWithNonDistinctEnemySpawnerIds = true;
            Debug.Log($"Scene: {scene} has enemy spawner IDs that are not distinct.");
        });

        Assert.False(
            hasScenesWithNonDistinctEnemySpawnerIds,
            "Found scenes with enemy spawner IDs that are not distinct, see console for more info."
        );
        
        yield return null;
    }
}
