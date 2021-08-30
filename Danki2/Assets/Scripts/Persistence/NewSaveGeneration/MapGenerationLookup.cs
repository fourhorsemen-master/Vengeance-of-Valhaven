using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomData
{
    [SerializeField] private bool availableInPool = true;
    [SerializeField] private List<int> weights = new List<int>();

    public bool AvailableInPool { get => availableInPool; set => availableInPool = value; }
    public List<int> Weights { get => weights; set => weights = value; }
}

[Serializable]
public class RoomDataLookup : SerializableEnumDictionary<RoomType, RoomData>
{
    public RoomDataLookup(RoomData defaultValue) : base(defaultValue){}
    public RoomDataLookup(Func<RoomData> defaultValueProvider) : base(defaultValueProvider) {}
}

[Serializable]
public class SpawnedEnemiesWrapper
{
    [SerializeField] private List<ActorType> spawnedEnemies = new List<ActorType>();

    public List<ActorType> SpawnedEnemies => spawnedEnemies;
}

[Serializable]
public class SpawnedEnemiesPerDepthLookup : SerializableDictionary<int, SpawnedEnemiesWrapper> {}

[Serializable]
public class RoomsPerZoneLookup : SerializableEnumDictionary<Zone, int>
{
    public RoomsPerZoneLookup(int defaultValue) : base(defaultValue) {}
    public RoomsPerZoneLookup(Func<int> defaultValueProvider) : base(defaultValueProvider) {}
}

public class MapGenerationLookup : Singleton<MapGenerationLookup>
{
    [SerializeField] private int abilityChoices = 0;
    [SerializeField] private int runeSockets = 0;
    [SerializeField] private string leftStartingAbilityName;
    [SerializeField] private string rightStartingAbilityName;
    
    [SerializeField] private int generatedRoomDepth = 0;
    [SerializeField] private int minRoomExits = 0;
    [SerializeField] private int maxRoomExits = 0;
    [SerializeField] private int requiredSpawners = 0;
    [SerializeField] private float chanceIndicatesChildRoomType = 0;
    [SerializeField] private float chanceIndicatesGrandchildRoomType = 0;
    [SerializeField] private RoomsPerZoneLookup roomsPerZoneLookup = new RoomsPerZoneLookup(0);

    [SerializeField] private RoomDataLookup roomDataLookup = new RoomDataLookup(() => new RoomData());

    [SerializeField] private SpawnedEnemiesPerDepthLookup spawnedEnemiesPerDepthLookup = new SpawnedEnemiesPerDepthLookup();

    [SerializeField] private TextAsset abilityNameStore = null;

    public int AbilityChoices { get => abilityChoices; set => abilityChoices = value; }
    public int RuneSockets { get => runeSockets; set => runeSockets = value; }
    public string LeftStartingAbilityName { get => leftStartingAbilityName; set => leftStartingAbilityName = value; }
    public string RightStartingAbilityName { get => rightStartingAbilityName; set => rightStartingAbilityName = value; }

    public int GeneratedRoomDepth { get => generatedRoomDepth; set => generatedRoomDepth = value; }
    public int MinRoomExits { get => minRoomExits; set => minRoomExits = value; }
    public int MaxRoomExits { get => maxRoomExits; set => maxRoomExits = value; }
    public int RequiredSpawners { get => requiredSpawners; set => requiredSpawners = value; }
    public float ChanceIndicatesChildRoomType { get => chanceIndicatesChildRoomType; set => chanceIndicatesChildRoomType = value; }
    public float ChanceIndicatesGrandchildRoomType { get => chanceIndicatesGrandchildRoomType; set => chanceIndicatesGrandchildRoomType = value; }
    public virtual RoomsPerZoneLookup RoomsPerZoneLookup { get => roomsPerZoneLookup; set => roomsPerZoneLookup = value; }

    public RoomDataLookup RoomDataLookup => roomDataLookup;

    public SpawnedEnemiesPerDepthLookup SpawnedEnemiesPerDepthLookup => spawnedEnemiesPerDepthLookup;

    public TextAsset AbilityNameStore { get => abilityNameStore; set => abilityNameStore = value; }

    protected override bool DestroyOnLoad => false;

    public bool IsAvailableInPool(RoomType roomType) => RoomDataLookup[roomType].AvailableInPool;

    public int GetDistanceWhenRequired(RoomType roomType) => RoomDataLookup[roomType].Weights.Count + 1;

    public int GetWeight(RoomType roomType, int distanceFromPrevious) => RoomDataLookup[roomType].Weights[distanceFromPrevious - 1];

    public void ForEachRoomTypeInPool(Action<RoomType> action)
    {
        EnumUtils.ForEach<RoomType>(roomType =>
        {
            if (IsAvailableInPool(roomType)) action(roomType);
        });
    }
}
