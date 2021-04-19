﻿using System;
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
    [SerializeField] private float chanceRoomTypeIndicatedByParent = 0;
    [SerializeField] private float chanceRoomTypeIndicatedByGrandparent = 0;

    [SerializeField] private RoomDataLookup roomDataLookup = new RoomDataLookup(() => new RoomData());

    public int AbilityChoices { get => abilityChoices; set => abilityChoices = value; }

    public int MinRoomDepth { get => minRoomDepth; set => minRoomDepth = value; }
    public int MaxRoomDepth { get => maxRoomDepth; set => maxRoomDepth = value; }
    public int MinRoomExits { get => minRoomExits; set => minRoomExits = value; }
    public int MaxRoomExits { get => maxRoomExits; set => maxRoomExits = value; }
    public float ChanceRoomTypeIndicatedByParent { get => chanceRoomTypeIndicatedByParent; set => chanceRoomTypeIndicatedByParent = value; }
    public float ChanceRoomTypeIndicatedByGrandparent { get => chanceRoomTypeIndicatedByGrandparent; set => chanceRoomTypeIndicatedByGrandparent = value; }

    public RoomDataLookup RoomDataLookup => roomDataLookup;

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