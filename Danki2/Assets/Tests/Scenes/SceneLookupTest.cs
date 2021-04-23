using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SceneLookupTest
{
    private const string SceneLookupAssetPath = "Assets/Prefabs/Meta/SceneLookup.prefab";
    private const string MapGenerationLookupAssetPath = "Assets/Prefabs/Meta/MapGenerationLookup.prefab";
    
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

    [UnityTest]
    public IEnumerator TestSceneLookupContainsAllPossibleRoomConfigurations()
    {
        SceneLookup sceneLookup = TestUtils.InstantiatePrefab<SceneLookup>(SceneLookupAssetPath);
        MapGenerationLookup mapGenerationLookup = TestUtils.InstantiatePrefab<MapGenerationLookup>(MapGenerationLookupAssetPath);
        
        bool hasInvalidConfigurations = false;

        EnumUtils.ForEach<RoomType>(roomType =>
        {
            if (roomTypesToIgnore.Contains(roomType)) return;
            
            EnumUtils.ForEach<Pole>(trueEntranceDirection =>
            {
                if (trueEntranceDirectionsToIgnore.Contains(trueEntranceDirection)) return;

                for (int numberOfExits = mapGenerationLookup.MinRoomExits; numberOfExits <= mapGenerationLookup.MaxRoomExits; numberOfExits++)
                {
                    List<Scene> validScenes = sceneLookup.GetValidScenes(roomType, trueEntranceDirection, numberOfExits);
                    
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
}
