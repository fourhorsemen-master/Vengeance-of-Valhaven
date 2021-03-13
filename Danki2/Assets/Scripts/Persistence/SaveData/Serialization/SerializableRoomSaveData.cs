﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableRoomSaveData
{
    [SerializeField] private int id;
    [SerializeField] private Scene scene;
    [SerializeField] private RoomType roomType;
    [SerializeField] private SerializableCombatRoomSaveData serializableCombatRoomSaveData;
    [SerializeField] private List<SerializableRoomTransitioner> serializableRoomTransitioners;

    public int Id {get => id; set => id = value; }
    public RoomType RoomType {get => roomType; set => roomType = value; }
    public Scene Scene {get => scene; set => scene = value; }
    public SerializableCombatRoomSaveData SerializableCombatRoomSaveData {get => serializableCombatRoomSaveData; set => serializableCombatRoomSaveData = value; }
    public List<SerializableRoomTransitioner> SerializableRoomTransitioners { get => serializableRoomTransitioners; set => serializableRoomTransitioners = value; }

    public RoomSaveData Deserialize()
    {
        return new RoomSaveData
        {
            Id = Id,
            Scene = Scene,
            RoomType = RoomType,
            CombatRoomSaveData = SerializableCombatRoomSaveData.Deserialize(),
            RoomTransitionerIdToNextRoomId = SerializableRoomTransitioners.ToDictionary(
                t => t.RoomTransitionerId,
                t => t.NextRoomId
            )
        };
    }
}