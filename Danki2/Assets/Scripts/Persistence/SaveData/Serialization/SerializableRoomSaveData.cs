using System;
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
    [SerializeField] private SerializableAbilityRoomSaveData serializableAbilityRoomSaveData;
    [SerializeField] private List<SerializableRoomTransitioner> serializableRoomTransitioners;
    [SerializeField] private int moduleSeed;
    [SerializeField] private int transitionModuleSeed;
    [SerializeField] private Pole cameraOrientation;
    [SerializeField] private int playerSpawnerId;

    public int Id { get => id; set => id = value; }
    public RoomType RoomType { get => roomType; set => roomType = value; }
    public Scene Scene { get => scene; set => scene = value; }
    public SerializableCombatRoomSaveData SerializableCombatRoomSaveData { get => serializableCombatRoomSaveData; set => serializableCombatRoomSaveData = value; }
    public SerializableAbilityRoomSaveData SerializableAbilityRoomSaveData { get => serializableAbilityRoomSaveData; set => serializableAbilityRoomSaveData = value; }
    public List<SerializableRoomTransitioner> SerializableRoomTransitioners { get => serializableRoomTransitioners; set => serializableRoomTransitioners = value; }
    public int ModuleSeed { get => moduleSeed; set => moduleSeed = value; }
    public int TransitionModuleSeed { get => transitionModuleSeed; set => transitionModuleSeed = value; }
    public Pole CameraOrientation { get => cameraOrientation; set => cameraOrientation = value; }
    public int PlayerSpawnerId { get => playerSpawnerId; set => playerSpawnerId = value; }

    public RoomSaveData Deserialize()
    {
        return new RoomSaveData
        {
            Id = Id,
            Scene = Scene,
            RoomType = RoomType,
            CombatRoomSaveData = SerializableCombatRoomSaveData.Deserialize(),
            AbilityRoomSaveData = SerializableAbilityRoomSaveData.Deserialize(),
            RoomTransitionerIdToNextRoomId = SerializableRoomTransitioners.ToDictionary(
                t => t.RoomTransitionerId,
                t => t.NextRoomId
            ),
            ModuleSeed = ModuleSeed,
            TransitionModuleSeed = TransitionModuleSeed,
            CameraOrientation =  CameraOrientation,
            PlayerSpawnerId = PlayerSpawnerId
        };
    }
}
