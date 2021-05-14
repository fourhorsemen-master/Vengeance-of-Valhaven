using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SceneLookupTest : PlayModeTestBase
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

    protected override IEnumerator SetUp()
    {
        yield return base.SetUp();
        TestUtils.InstantiatePrefab<MapGenerationLookup>();
        TestUtils.InstantiatePrefab<SceneLookup>();
        yield return null;
    }

    protected override IEnumerator TearDown()
    {
        MapGenerationLookup.Instance.Destroy();
        SceneLookup.Instance.Destroy();
        yield return null;
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
        AssertDistinctGameplayDataElement(
            d => d.RoomTypes.IsDistinct(),
            s => $"Scene: {s} has room types that are not distinct.",
            "Found scenes with room types that are not distinct, see console for more info."
        );
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestGameplayScenesHaveDistinctCameraOrientations()
    {
        AssertDistinctGameplayDataElement(
            d => d.CameraOrientations.IsDistinct(),
            s => $"Scene: {s} has camera orientations that are not distinct.",
            "Found scenes with camera orientations that are not distinct, see console for more info."
        );
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestGameplayScenesHaveDistinctEntranceIds()
    {
        AssertDistinctGameplayDataElement(
            d => d.EntranceData.IsDistinctById(),
            s => $"Scene: {s} has entrance IDs that are not distinct.",
            "Found scenes with entrance IDs that are not distinct, see console for more info."
        );
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestGameplayScenesHaveDistinctExitIds()
    {
        AssertDistinctGameplayDataElement(
            d => d.ExitData.IsDistinctById(),
            s => $"Scene: {s} has exit IDs that are not distinct.",
            "Found scenes with exit IDs that are not distinct, see console for more info."
        );
        yield return null;
    }

    private void AssertDistinctGameplayDataElement(
        Func<GameplaySceneData, bool> uniqueCheck,
        Func<Scene, string> sceneErrorMessageProvider,
        string overallErrorMessage
    )
    {
        bool allScenesHaveDistinctElements = true;
        
        EnumUtils.ForEach<Scene>(scene =>
        {
            SceneData sceneData = SceneLookup.Instance.sceneDataLookup[scene];
            if (sceneData.SceneType != SceneType.Gameplay) return;
            if (uniqueCheck(sceneData.GameplaySceneData)) return;

            allScenesHaveDistinctElements = false;
            Debug.Log(sceneErrorMessageProvider(scene));
        });

        Assert.True(allScenesHaveDistinctElements, overallErrorMessage);
    }
}
