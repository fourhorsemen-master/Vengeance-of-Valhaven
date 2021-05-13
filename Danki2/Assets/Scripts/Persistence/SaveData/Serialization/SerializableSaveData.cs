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

    public SaveData Deserialize()
    {
        SaveData saveData = new SaveData
        {
            Version = Version,
            Seed = Seed,
            RandomState = RandomState,
            PlayerHealth = PlayerHealth,
            SerializableAbilityTree = SerializableAbilityTree,
            RuneSockets = RuneSockets,
            RuneOrder = RuneOrder,
        };

        AddGraphData(saveData);

        return saveData;
    }

    private void AddGraphData(SaveData saveData)
    {
        Dictionary<int, SerializableRoomNode> idToSerializableRoomNode = SerializableRoomNodes.ToDictionary(
            serializableRoomNode => serializableRoomNode.Id
        );
        Dictionary<int, RoomNode> idToRoomNode = idToSerializableRoomNode.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Deserialize()
        );
        
        DeserializeRoomNodeGraphData(0, idToRoomNode, idToSerializableRoomNode);
        saveData.CurrentRoomNode = idToRoomNode[CurrentRoomId];
        saveData.DefeatRoom = serializableDefeatRoom.Deserialize();
    }

    private RoomNode DeserializeRoomNodeGraphData(
        int id,
        Dictionary<int, RoomNode> idToRoomNode,
        Dictionary<int, SerializableRoomNode> idToSerializableRoomNode
    )
    {
        RoomNode roomNode = idToRoomNode[id];
        SerializableRoomNode serializableRoomNode = idToSerializableRoomNode[id];
        
        roomNode.Parent = serializableRoomNode.ParentId == -1
            ? null
            : idToRoomNode[serializableRoomNode.ParentId];

        idToSerializableRoomNode[id].ChildIds.ForEach(childId =>
        {
            roomNode.Children.Add(idToRoomNode[childId]);
            DeserializeRoomNodeGraphData(childId, idToRoomNode, idToSerializableRoomNode);
        });

        serializableRoomNode.SerializableTransitionData.ForEach(serializableTransitionData =>
            {
                int exitId = serializableTransitionData.RoomTransitionerId;
                RoomNode child = idToRoomNode[serializableTransitionData.NextRoomId];

                roomNode.ExitIdToChildLookup[exitId] = child;
                roomNode.ChildToExitIdLookup[child] = exitId;
                roomNode.ExitIdToIndicatesNextRoomType[exitId] = serializableTransitionData.IndicatesNextRoomType;
                roomNode.ExitIdToFurtherIndicatedRoomTypes[exitId] = serializableTransitionData.FurtherIndicatedRoomTypes;
            });
        
        return roomNode;
    }
}
