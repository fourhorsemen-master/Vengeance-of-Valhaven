using System;
using UnityEngine;

[Serializable]
public class SerializableRoomTransitioner
{
    [SerializeField] private int roomTransitionerId;
    [SerializeField] private int nextRoomId;

    public int RoomTransitionerId { get => roomTransitionerId; set => roomTransitionerId = value; }
    public int NextRoomId { get => nextRoomId; set => nextRoomId = value; }
}
