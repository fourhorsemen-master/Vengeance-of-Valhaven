using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SerializableSaveData
{
    [SerializeField] private int version;
    [SerializeField] private int seed;
    [SerializeField] private Random.State randomState;
    [SerializeField] private int playerHealth;
    [SerializeField] private SerializableAbilityTree serializableAbilityTree;
    [SerializeField] private List<RuneSocket> runeSockets;
    [SerializeField] private List<Rune> runeOrder;
    [SerializeField] private int currentRoomId;
    [SerializeField] private List<SerializableRoomNode> serializableRoomNodes;
    [SerializeField] private SerializableRoomNode serializableDefeatRoom;
    
    public int Version { get => version; set => version = value; }
    public int Seed { get => seed; set => seed = value; }
    public Random.State RandomState { get => randomState; set => randomState = value; }
    public int PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public SerializableAbilityTree SerializableAbilityTree { get => serializableAbilityTree; set => serializableAbilityTree = value; }
    public List<RuneSocket> RuneSockets { get => runeSockets; set => runeSockets = value; }
    public List<Rune> RuneOrder { get => runeOrder; set => runeOrder = value; }
    public int CurrentRoomId { get => currentRoomId; set => currentRoomId = value; }
    public List<SerializableRoomNode> SerializableRoomNodes { get => serializableRoomNodes; set => serializableRoomNodes = value; }
    public SerializableRoomNode SerializableDefeatRoom { get => serializableDefeatRoom; set => serializableDefeatRoom = value; }

    public SaveData2 Deserialize()
    {
        SaveData2 saveData = new SaveData2
        {
            Version = Version,
            Seed = Seed,
            RandomState = RandomState,
            PlayerHealth = PlayerHealth,
            SerializableAbilityTree = SerializableAbilityTree,
            RuneSockets = RuneSockets,
            RuneOrder = RuneOrder,
        };

        AddRoomData(saveData);

        return saveData;
    }

    private void AddRoomData(SaveData2 saveData)
    {
        Dictionary<int, SerializableRoomNode> idToSerializableRoomNode = new Dictionary<int, SerializableRoomNode>();
        serializableRoomNodes.ForEach(s => idToSerializableRoomNode[s.Id] = s);
        
        DeserializeRoomNodes(idToSerializableRoomNode[0], null, idToSerializableRoomNode, saveData);

        saveData.DefeatRoom = serializableDefeatRoom.Deserialize();
    }

    private RoomNode DeserializeRoomNodes(
        SerializableRoomNode serializableNode,
        RoomNode parent,
        Dictionary<int, SerializableRoomNode> idToSerializableRoomNode,
        SaveData2 saveData
    )
    {
        RoomNode roomNode = serializableNode.Deserialize();
        roomNode.Parent = parent;
        roomNode.Children = serializableNode.ChildrenIds
            .Select(id => DeserializeRoomNodes(idToSerializableRoomNode[id], roomNode, idToSerializableRoomNode, saveData))
            .ToList();

        if (serializableNode.Id == currentRoomId) saveData.CurrentRoomNode = roomNode;
        
        return roomNode;
    }
}
