using System;
using UnityEngine;

[Serializable]
public class SerializableHealingRoomSaveData
{
    [SerializeField] private bool hasHealed;

    public bool HasHealed { get => hasHealed; set => hasHealed = value; }

    public HealingRoomSaveData Deserialize()
    {
        return new HealingRoomSaveData
        {
            HasHealed = HasHealed
        };
    }
}
