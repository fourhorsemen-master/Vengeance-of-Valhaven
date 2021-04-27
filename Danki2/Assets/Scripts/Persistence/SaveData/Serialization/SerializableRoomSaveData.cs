using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableRoomSaveData
{
    [SerializeField] private int id;
    [SerializeField] private int depth;
    [SerializeField] private Scene scene;
    [SerializeField] private RoomType roomType;
    [SerializeField] private SerializableCombatRoomSaveData serializableCombatRoomSaveData;
    [SerializeField] private SerializableAbilityRoomSaveData serializableAbilityRoomSaveData;
    [SerializeField] private SerializableHealingRoomSaveData serializableHealingRoomSaveData;
    [SerializeField] private List<SerializableTransitionData> serializableTransitionData;
    [SerializeField] private int moduleSeed;
    [SerializeField] private int transitionModuleSeed;
    [SerializeField] private Pole cameraOrientation;
    [SerializeField] private int playerSpawnerId;

    public int Id { get => id; set => id = value; }
    public int Depth { get => depth; set => depth = value; }
    public RoomType RoomType { get => roomType; set => roomType = value; }
    public Scene Scene { get => scene; set => scene = value; }
    public SerializableCombatRoomSaveData SerializableCombatRoomSaveData { get => serializableCombatRoomSaveData; set => serializableCombatRoomSaveData = value; }
    public SerializableAbilityRoomSaveData SerializableAbilityRoomSaveData { get => serializableAbilityRoomSaveData; set => serializableAbilityRoomSaveData = value; }
    public SerializableHealingRoomSaveData SerializableHealingRoomSaveData { get => serializableHealingRoomSaveData; set => serializableHealingRoomSaveData = value; }
    public List<SerializableTransitionData> SerializableTransitionData { get => serializableTransitionData; set => serializableTransitionData = value; }
    public int ModuleSeed { get => moduleSeed; set => moduleSeed = value; }
    public int TransitionModuleSeed { get => transitionModuleSeed; set => transitionModuleSeed = value; }
    public Pole CameraOrientation { get => cameraOrientation; set => cameraOrientation = value; }
    public int PlayerSpawnerId { get => playerSpawnerId; set => playerSpawnerId = value; }

    public RoomSaveData Deserialize()
    {
        return new RoomSaveData
        {
            Id = Id,
            Depth = Depth,
            Scene = Scene,
            RoomType = RoomType,
            CombatRoomSaveData = SerializableCombatRoomSaveData.Deserialize(),
            AbilityRoomSaveData = SerializableAbilityRoomSaveData.Deserialize(),
            HealingRoomSaveData = SerializableHealingRoomSaveData.Deserialize(),
            RoomTransitionerIdToTransitionData = SerializableTransitionData.ToDictionary(
                d => d.RoomTransitionerId,
                d => new TransitionData
                {
                    NextRoomId = d.NextRoomId,
                    IndicatesNextRoomType = d.IndicatesNextRoomType,
                    FurtherIndicatedRoomTypes = d.FurtherIndicatedRoomTypes
                }
            ),
            ModuleSeed = ModuleSeed,
            TransitionModuleSeed = TransitionModuleSeed,
            CameraOrientation =  CameraOrientation,
            PlayerSpawnerId = PlayerSpawnerId
        };
    }
}
