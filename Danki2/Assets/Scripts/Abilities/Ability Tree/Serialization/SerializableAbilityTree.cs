using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializable version of the ability tree. This works by turning the nodes of the ability tree into a flat list
/// of nodes that know about their child node ids, rather than storing a reference to the actual node itself.
/// </summary>
[Serializable]
public class SerializableAbilityTree
{
    private const int RootNodeId = 0;
    private const int NoChildId = -1;
    
    [SerializeField] private List<SerializableNode> serializableNodes = new List<SerializableNode>();
    [SerializeField] private List<SerializableOwnedAbility> serializableOwnedAbilities = new List<SerializableOwnedAbility>();

    public SerializableAbilityTree(AbilityTree abilityTree)
    {
        int id = RootNodeId;
        Dictionary<Node, int> nodeToIdLookup = new Dictionary<Node, int>();
        abilityTree.RootNode.IterateDown(node =>
        {
            nodeToIdLookup[node] = id;
            id++;
        });

        abilityTree.RootNode.IterateDown(node =>
        {
            int leftChildId = node.HasChild(Direction.Left) ? nodeToIdLookup[node.GetChild(Direction.Left)] : NoChildId;
            int rightChildId = node.HasChild(Direction.Right) ? nodeToIdLookup[node.GetChild(Direction.Right)] : NoChildId;
            serializableNodes.Add(new SerializableNode(node.Ability, nodeToIdLookup[node], leftChildId, rightChildId));
        });

        EnumUtils.ForEach<AbilityReference>(abilityReference =>
        {
            serializableOwnedAbilities.Add(new SerializableOwnedAbility(
                abilityReference,
                abilityTree.OwnedAbilities[abilityReference]
            ));
        });
    }

    public AbilityTree Deserialize()
    {
        Node rootNode = DeserializeRootNode();
        return AbilityTreeFactory.CreateTree(
            DeserializeOwnedAbilities(),
            rootNode.GetChild(Direction.Left),
            rootNode.GetChild(Direction.Right)
        );
    }

    private Node DeserializeRootNode()
    {
        Dictionary<int, SerializableNode> idToSerializableNodeLookup = new Dictionary<int, SerializableNode>();
        serializableNodes.ForEach(s => idToSerializableNodeLookup[s.Id] = s);
        return DeserializeNode(idToSerializableNodeLookup[RootNodeId], idToSerializableNodeLookup);
    }

    private Node DeserializeNode(SerializableNode serializableNode, Dictionary<int, SerializableNode> idToSerializableNodeLookup)
    {
        Node leftChild = idToSerializableNodeLookup.ContainsKey(serializableNode.LeftChildId)
            ? DeserializeNode(idToSerializableNodeLookup[serializableNode.LeftChildId], idToSerializableNodeLookup)
            : null;
        Node rightChild = idToSerializableNodeLookup.ContainsKey(serializableNode.RightChildId)
            ? DeserializeNode(idToSerializableNodeLookup[serializableNode.RightChildId], idToSerializableNodeLookup)
            : null;

        return AbilityTreeFactory.CreateNode(serializableNode.AbilityReference, leftChild, rightChild);
    }

    private EnumDictionary<AbilityReference, int> DeserializeOwnedAbilities()
    {
        EnumDictionary<AbilityReference, int> ownedAbilities = new EnumDictionary<AbilityReference, int>(0);
        serializableOwnedAbilities.ForEach(serializableOwnedAbility =>
        {
            ownedAbilities[serializableOwnedAbility.AbilityReference] = serializableOwnedAbility.Count;
        });
        return ownedAbilities;
    }
}
