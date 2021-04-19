using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableTransitionData
{
    [SerializeField] private int roomTransitionerId;
    [SerializeField] private int nextRoomId;
    [SerializeField] private bool indicatesNextRoomType;
    [SerializeField] private List<RoomType> furtherIndicatedIndicatedRoomTypes;

    public int RoomTransitionerId { get => roomTransitionerId; set => roomTransitionerId = value; }
    public int NextRoomId { get => nextRoomId; set => nextRoomId = value; }
    public bool IndicatesNextRoomType { get => indicatesNextRoomType; set => indicatesNextRoomType = value; }
    public List<RoomType> FurtherIndicatedRoomTypes { get => furtherIndicatedIndicatedRoomTypes; set => furtherIndicatedIndicatedRoomTypes = value; }
}

