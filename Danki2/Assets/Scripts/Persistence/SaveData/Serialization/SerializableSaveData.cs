﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableSaveData
{
    [SerializeField] private int version;
    [SerializeField] private int playerHealth;
    [SerializeField] private SerializableAbilityTree serializableAbilityTree;
    [SerializeField] private int currentRoomId;
    [SerializeField] private int defeatRoomId;
    [SerializeField] private List<SerializableRoomSaveData> serializableRoomSaveDataList;
    [SerializeField] private List<SerializableRoomTransition> serializableRoomTransitions;
    
    public int Version { get => version; set => version = value; }
    public int PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public SerializableAbilityTree SerializableAbilityTree { get => serializableAbilityTree; set => serializableAbilityTree = value; }
    public int CurrentRoomId { get => currentRoomId; set => currentRoomId = value; }
    public int DefeatRoomId { get => defeatRoomId; set => defeatRoomId = value; }
    public List<SerializableRoomSaveData> SerializableRoomSaveDataList { get => serializableRoomSaveDataList; set => serializableRoomSaveDataList = value; }
    public List<SerializableRoomTransition> SerializableRoomTransitions { get => serializableRoomTransitions; set => serializableRoomTransitions = value; }

    public SaveData Deserialize()
    {
        return new SaveData
        {
            Version = Version,
            PlayerHealth = PlayerHealth,
            AbilityTree = SerializableAbilityTree.Deserialize(),
            CurrentRoomId = CurrentRoomId,
            DefeatRoomId = DefeatRoomId,
            RoomSaveDataLookup = SerializableRoomSaveDataList.ToDictionary(
                d => d.Id,
                d => d.Deserialize()
            ),
            RoomTransitions = SerializableRoomTransitions.ToDictionary(
                t => t.FromId,
                t => t.ToIds
            )
        };
    }
}