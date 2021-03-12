using System;
using UnityEngine;

[Serializable]
public class SerializableSpawnerSaveData
{
    [SerializeField] private int id;
    [SerializeField] private ActorType actorType;

    public int Id { get => id; set => id = value; }
    public ActorType ActorType { get => actorType; set => actorType = value; }
}
