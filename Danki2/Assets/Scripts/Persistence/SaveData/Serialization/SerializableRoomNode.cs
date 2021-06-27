using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableRoomNode
{
    [SerializeField] private int id;
    [SerializeField] private int parentId;
    [SerializeField] private List<int> childIds;
    [SerializeField] private int depth;
    [SerializeField] private Zone zone;
    [SerializeField] private int depthInZone;
    [SerializeField] private RoomType roomType;
    [SerializeField] private Scene scene;
    [SerializeField] private SerializableCombatRoomSaveData serializableCombatRoomSaveData;
    [SerializeField] private SerializableAbilityRoomSaveData serializableAbilityRoomSaveData;
    [SerializeField] private SerializableHealingRoomSaveData serializableHealingRoomSaveData;
    [SerializeField] private SerializableRuneRoomSaveData serializableRuneRoomSaveData;
    [SerializeField] private List<SerializableTransitionData> serializableTransitionData;
    [SerializeField] private int moduleSeed;
    [SerializeField] private int transitionModuleSeed;
    [SerializeField] private Pole cameraOrientation;
    [SerializeField] private int playerSpawnerId;

    public int Id { get => id; set => id = value; }
    public int ParentId { get => parentId; set => parentId = value; }
    public List<int> ChildIds { get => childIds; set => childIds = value; }
    public int Depth { get => depth; set => depth = value; }
    public Zone Zone { get => zone; set => zone = value; }
    public int DepthInZone { get => depthInZone; set => depthInZone = value; }
    public RoomType RoomType { get => roomType; set => roomType = value; }
    public Scene Scene { get => scene; set => scene = value; }
    public SerializableCombatRoomSaveData SerializableCombatRoomSaveData { get => serializableCombatRoomSaveData; set => serializableCombatRoomSaveData = value; }
    public SerializableAbilityRoomSaveData SerializableAbilityRoomSaveData { get => serializableAbilityRoomSaveData; set => serializableAbilityRoomSaveData = value; }
    public SerializableHealingRoomSaveData SerializableHealingRoomSaveData { get => serializableHealingRoomSaveData; set => serializableHealingRoomSaveData = value; }
    public SerializableRuneRoomSaveData SerializableRuneRoomSaveData { get => serializableRuneRoomSaveData; set => serializableRuneRoomSaveData = value; }
    public List<SerializableTransitionData> SerializableTransitionData { get => serializableTransitionData; set => serializableTransitionData = value; }
    public int ModuleSeed { get => moduleSeed; set => moduleSeed = value; }
    public int TransitionModuleSeed { get => transitionModuleSeed; set => transitionModuleSeed = value; }
    public Pole CameraOrientation { get => cameraOrientation; set => cameraOrientation = value; }
    public int PlayerSpawnerId { get => playerSpawnerId; set => playerSpawnerId = value; }

    public RoomNode Deserialize()
    {
        return new RoomNode
        {
            Depth = Depth,
            Zone = Zone,
            DepthInZone = DepthInZone,
            RoomType = RoomType,
            Scene = Scene,
            CombatRoomSaveData = SerializableCombatRoomSaveData.Deserialize(),
            AbilityRoomSaveData = SerializableAbilityRoomSaveData.Deserialize(),
            HealingRoomSaveData = SerializableHealingRoomSaveData.Deserialize(),
            RuneRoomSaveData = SerializableRuneRoomSaveData.Deserialize(),
            ModuleSeed = ModuleSeed,
            TransitionModuleSeed = TransitionModuleSeed,
            CameraOrientation = CameraOrientation,
            PlayerSpawnerId = playerSpawnerId
        };
    }
}
