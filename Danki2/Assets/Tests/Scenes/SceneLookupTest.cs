using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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
        
        EnumUtils.ForEach<RoomType>(roomType =>
        {
            if (roomTypesToIgnore.Contains(roomType)) return;
            
            EnumUtils.ForEach<Pole>(trueEntranceDirection =>
            {
                if (trueEntranceDirectionsToIgnore.Contains(trueEntranceDirection)) return;

                for (int numberOfExits = mapGenerationLookup.MinRoomExits; numberOfExits <= mapGenerationLookup.MaxRoomExits; numberOfExits++)
                {
                    List<Scene> validScenes = sceneLookup.GetValidScenes(roomType, trueEntranceDirection, numberOfExits);
                    Assert.Greater(
                        validScenes.Count,
                        0,
                        $"RoomType: {roomType}, " +
                        $"TrueEntranceDirection: {trueEntranceDirection}, " +
                        $"NumberOfExists: {numberOfExits} " +
                        "has no valid scenes. Ensure that there is a scene that accommodates this configuration."
                    );
                }
            });
        });
        
        yield return null;
    }
}
