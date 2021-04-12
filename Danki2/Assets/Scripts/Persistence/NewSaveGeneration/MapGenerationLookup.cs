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

public class MapGenerationLookup : Singleton<MapGenerationLookup>
{
    [SerializeField] private int abilityChoices = 0;
    
    [SerializeField] private int minRoomDepth = 0;
    [SerializeField] private int maxRoomDepth = 0;
    [SerializeField] private int minRoomExits = 0;
    [SerializeField] private int maxRoomExits = 0;

    [SerializeField] private RoomDataLookup roomDataLookup = new RoomDataLookup(() => new RoomData());

    public int AbilityChoices { get => abilityChoices; set => abilityChoices = value; }

    public int MinRoomDepth { get => minRoomDepth; set => minRoomDepth = value; }
    public int MaxRoomDepth { get => maxRoomDepth; set => maxRoomDepth = value; }
    public int MinRoomExits { get => minRoomExits; set => minRoomExits = value; }
    public int MaxRoomExits { get => maxRoomExits; set => maxRoomExits = value; }

    public RoomDataLookup RoomDataLookup { get => roomDataLookup; set => roomDataLookup = value; }

    protected override bool DestroyOnLoad => false;
}
