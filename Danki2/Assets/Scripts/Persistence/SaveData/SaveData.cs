using System.Collections.Generic;
using System.Linq;

public class SaveData : ISerializable<SerializableSaveData>
{
    public int Version { get; set; }

    public int Seed { get; set; }

    public int PlayerHealth { get; set; }
    public SerializableAbilityTree SerializableAbilityTree { get; set; }
    public List<RuneSocket> RuneSockets { get; set; }
    public List<Rune> RuneOrder { get; set; }
    public int NextRuneIndex { get; set; }

    public int CurrentRoomId { get; set; }
    public int DefeatRoomId { get; set; }
    public Dictionary<int, RoomSaveData> RoomSaveDataLookup { get; set; }

    public RoomSaveData CurrentRoomSaveData => RoomSaveDataLookup[CurrentRoomId];
    public RoomSaveData DefeatRoomSaveData => RoomSaveDataLookup[DefeatRoomId];

    public SerializableSaveData Serialize()
    {
        return new SerializableSaveData
        {
            Version = Version,
            Seed = Seed,
            PlayerHealth = PlayerHealth,
            SerializableAbilityTree = SerializableAbilityTree,
            RuneSockets = RuneSockets,
            RuneOrder = RuneOrder,
            NextRuneIndex = NextRuneIndex,
            CurrentRoomId = CurrentRoomId,
            DefeatRoomId = DefeatRoomId,
            SerializableRoomSaveDataList = RoomSaveDataLookup.Values
                .Select(roomData => roomData.Serialize())
                .ToList()
        };
    }
}
