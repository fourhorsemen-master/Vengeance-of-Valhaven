﻿using System.Collections.Generic;
using System.Linq;

public class SaveData
{
    public int Version { get; set; }

    public int Seed { get; set; }

    public int PlayerHealth { get; set; }
    public AbilityTree AbilityTree { get; set; }

    public int CurrentRoomId { get; set; }
    public int DefeatRoomId { get; set; }
    public Dictionary<int, RoomSaveData> RoomSaveDataLookup { get; set; }

    public RoomSaveData CurrentRoomSaveData => RoomSaveDataLookup[CurrentRoomId];
    public RoomSaveData DefeatRoomSaveData => RoomSaveDataLookup[DefeatRoomId];

    public SerializableSaveData Serialize()
    {
        return new SerializableSaveData
        {
            Version = Version,
            Seed = Seed,
            PlayerHealth = PlayerHealth,
            SerializableAbilityTree = new SerializableAbilityTree(AbilityTree),
            CurrentRoomId = CurrentRoomId,
            DefeatRoomId = DefeatRoomId,
            SerializableRoomSaveDataList = RoomSaveDataLookup.Values
                .Select(roomData => roomData.Serialize())
                .ToList()
        };
    }
}
