using System.Collections.Generic;
using System.Linq;

public class RoomSaveData
{
    public int Id { get; set; }
    public int Depth { get; set; }
    public Scene Scene { get; set; }
    public RoomType RoomType { get; set; }
    public CombatRoomSaveData CombatRoomSaveData { get; set; }
    public AbilityRoomSaveData AbilityRoomSaveData { get; set; }
    public HealingRoomSaveData HealingRoomSaveData { get; set; }
    public RuneRoomSaveData RuneRoomSaveData { get; set; }
    public Dictionary<int, TransitionData> RoomTransitionerIdToTransitionData { get; set; }
    public int ModuleSeed { get; set; }
    public int TransitionModuleSeed { get; set; }
    public Pole CameraOrientation { get; set; }
    public int PlayerSpawnerId { get; set; }

    public SerializableRoomSaveData Serialize()
    {
        return new SerializableRoomSaveData
        {
            Id = Id,
            Depth = Depth,
            Scene = Scene,
            RoomType = RoomType,
            SerializableCombatRoomSaveData = CombatRoomSaveData?.Serialize(),
            SerializableAbilityRoomSaveData = AbilityRoomSaveData?.Serialize(),
            SerializableHealingRoomSaveData = HealingRoomSaveData?.Serialize(),
            SerializableRuneRoomSaveData = RuneRoomSaveData?.Serialize(),
            SerializableTransitionData = RoomTransitionerIdToTransitionData?.Keys
                .Select(transitionerId => new SerializableTransitionData
                {
                    RoomTransitionerId = transitionerId,
                    NextRoomId = RoomTransitionerIdToTransitionData[transitionerId].NextRoomId,
                    IndicatesNextRoomType = RoomTransitionerIdToTransitionData[transitionerId].IndicatesNextRoomType,
                    FurtherIndicatedRoomTypes = RoomTransitionerIdToTransitionData[transitionerId].FurtherIndicatedRoomTypes,
                })
                .ToList(),
            ModuleSeed = ModuleSeed,
            TransitionModuleSeed = TransitionModuleSeed,
            CameraOrientation = CameraOrientation,
            PlayerSpawnerId = PlayerSpawnerId
        };
    }
}
