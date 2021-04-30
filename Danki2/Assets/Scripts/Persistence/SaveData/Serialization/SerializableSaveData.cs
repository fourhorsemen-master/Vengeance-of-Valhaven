using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableSaveData
{
    [SerializeField] private int version;
    [SerializeField] private int seed;
    [SerializeField] private int playerHealth;
    [SerializeField] private SerializableAbilityTree serializableAbilityTree;
    [SerializeField] private List<RuneSocket> runeSockets;
    [SerializeField] private List<Rune> runeOrder;
    [SerializeField] private int nextRuneIndex;
    [SerializeField] private int currentRoomId;
    [SerializeField] private int defeatRoomId;
    [SerializeField] private List<SerializableRoomSaveData> serializableRoomSaveDataList;
    
    public int Version { get => version; set => version = value; }
    public int Seed { get => seed; set => seed = value; }
    public int PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public SerializableAbilityTree SerializableAbilityTree { get => serializableAbilityTree; set => serializableAbilityTree = value; }
    public List<RuneSocket> RuneSockets { get => runeSockets; set => runeSockets = value; }
    public List<Rune> RuneOrder { get => runeOrder; set => runeOrder = value; }
    public int NextRuneIndex { get => nextRuneIndex; set => nextRuneIndex = value; }
    public int CurrentRoomId { get => currentRoomId; set => currentRoomId = value; }
    public int DefeatRoomId { get => defeatRoomId; set => defeatRoomId = value; }
    public List<SerializableRoomSaveData> SerializableRoomSaveDataList { get => serializableRoomSaveDataList; set => serializableRoomSaveDataList = value; }

    public SaveData Deserialize()
    {
        return new SaveData
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
            RoomSaveDataLookup = SerializableRoomSaveDataList.ToDictionary(
                d => d.Id,
                d => d.Deserialize()
            )
        };
    }
}
