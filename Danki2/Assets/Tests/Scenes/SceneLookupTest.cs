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
        TestUtils.InstantiatePrefab<MapGenerationLookup>(TestUtils.MapGenerationLookupAssetPath);
        TestUtils.InstantiatePrefab<SceneLookup>(TestUtils.SceneLookupAssetPath);
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
}
