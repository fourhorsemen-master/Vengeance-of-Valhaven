using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableRoomTransition
{
    [SerializeField] private int fromId;
    [SerializeField] private List<int> toIds;

    public int FromId { get => fromId; set => fromId = value; }
    public List<int> ToIds { get => toIds; set => toIds = value; }
}
