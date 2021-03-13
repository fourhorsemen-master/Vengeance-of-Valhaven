﻿using System.Collections.Generic;
using System.Linq;

public class RoomSaveData
{
    public int Id { get; set; }
    public Scene Scene { get; set; }
    public RoomType RoomType { get; set; }
    public CombatRoomSaveData CombatRoomSaveData { get; set; }
    public Dictionary<int, int> RoomTransitionerIdToNextRoomId { get; set; }

    public SerializableRoomSaveData Serialize()
    {
        return new SerializableRoomSaveData
        {
            Id = Id,
            Scene = Scene,
            RoomType = RoomType,
            SerializableCombatRoomSaveData = CombatRoomSaveData?.Serialize(),
            SerializableRoomTransitioners = RoomTransitionerIdToNextRoomId?.Keys
                .Select(transitionerId => new SerializableRoomTransitioner
                {
                    RoomTransitionerId = transitionerId,
                    NextRoomId = RoomTransitionerIdToNextRoomId[transitionerId]
                })
                .ToList()
        };
    }
}