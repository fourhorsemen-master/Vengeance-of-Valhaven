﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveData2
{
    public int Version { get; set; }
    public int Seed { get; set; }
    public Random.State RandomState { get; set; }

    public int PlayerHealth { get; set; }
    public SerializableAbilityTree SerializableAbilityTree { get; set; }
    public List<RuneSocket> RuneSockets { get; set; }
    public List<Rune> RuneOrder { get; set; }

    public RoomNode CurrentRoomNode { get; set; }
    public RoomNode DefeatRoom { get; set; }

    public SerializableSaveData Serialize()
    {
        SerializableSaveData serializableSaveData = new SerializableSaveData
        {
            Version = Version,
            Seed = Seed,
            RandomState = RandomState,
            PlayerHealth = PlayerHealth,
            SerializableAbilityTree = SerializableAbilityTree,
            RuneSockets = RuneSockets,
            RuneOrder = RuneOrder
        };
        
        AddRoomData(serializableSaveData);
        
        return serializableSaveData;
    }

    private void AddRoomData(SerializableSaveData serializableSaveData)
    {
        RoomNode rootNode = CurrentRoomNode.GetRootNode();

        int id = 0;
        Dictionary<RoomNode, int> roomNodeToId = new Dictionary<RoomNode, int>();
        rootNode.IterateDown(node =>
        {
            roomNodeToId[node] = id;
            id++;
        });

        List<SerializableRoomNode> serializableRoomNodes = new List<SerializableRoomNode>();

        rootNode.IterateDown(node =>
        {
            SerializableRoomNode serializableRoomNode = node.Serialize();
            serializableRoomNode.Id = roomNodeToId[node];
            serializableRoomNode.ParentId = node.IsRootNode ? -1 : roomNodeToId[node.Parent];
            serializableRoomNode.ChildrenIds = node.Children
                .Select(c => roomNodeToId[c])
                .ToList();
            serializableRoomNodes.Add(serializableRoomNode);
        });

        serializableSaveData.CurrentRoomId = roomNodeToId[CurrentRoomNode];
        serializableSaveData.SerializableRoomNodes = serializableRoomNodes;
        serializableSaveData.SerializableDefeatRoom = DefeatRoom.Serialize();
    }
}
