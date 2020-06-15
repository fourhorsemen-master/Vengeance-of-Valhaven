using System;
using UnityEngine;

public static class AbilityTreeFactory
{
    // Root node must have two children for ability tree to be functional.
    public static AbilityTree CreateTree(Node leftChild, Node rightChild)
    {
        Node rootNode = new NodeImplementation();
        SetParentAndChild(rootNode, leftChild, Direction.Left);
        SetParentAndChild(rootNode, rightChild, Direction.Right);
        return new AbilityTreeImplementation(rootNode);
    }

    public static Node CreateNode(AbilityReference ability, Node leftChild = null, Node rightChild = null)
    {
        Node node = new NodeImplementation(ability);
        if (leftChild != null) SetParentAndChild(node, leftChild, Direction.Left);
        if (rightChild != null) SetParentAndChild(node, rightChild, Direction.Right);
        return node;
    }

    private static void SetParentAndChild(Node parent, Node child, Direction direction)
    {
        parent.SetChild(direction, child);
        child.Parent = parent;
    }

    public static void InsertAbility(AbilityReference ability, Node node, InsertArea area)
    {
        if (node.IsRootNode)
        {
            Debug.LogError("Tried to insert ability relative to root node.");
            return;
        }

        Node newNode = CreateNode(ability);

        switch (area)
        {
            case InsertArea.Centre:
                node.SetAbility(ability);
                break;

            case InsertArea.TopLeft:
                if (!node.Parent.HasChild(Direction.Right))
                {
                    Debug.LogError("Tried to insert parent node on wrong side.");
                }

                SetParentAndChild(node.Parent, newNode, Direction.Right);
                SetParentAndChild(newNode, node, Direction.Right);
                break;

            case InsertArea.TopRight:
                if (!node.Parent.HasChild(Direction.Left))
                {
                    Debug.LogError("Tried to insert parent node on wrong side.");
                }

                SetParentAndChild(node.Parent, newNode, Direction.Left);
                SetParentAndChild(newNode, node, Direction.Left);
                break;

            case InsertArea.BottomLeft:
                if (node.HasChild(Direction.Left))
                {
                    Debug.LogError("Tried to insert child where child already exists.");
                }

                SetParentAndChild(node, newNode, Direction.Left);
                break;

            case InsertArea.BottomRight:
                if (node.HasChild(Direction.Right))
                {
                    Debug.LogError("Tried to insert child where child already exists.");
                }

                SetParentAndChild(node, newNode, Direction.Right);
                break;
        }
    }

    // These private implementations of the abstract classes ensure that all instantiation
    // of ability trees and nodes happen from this factory class. See the factory pattern
    // for more information
    private class AbilityTreeImplementation : AbilityTree
    {
        public AbilityTreeImplementation(Node rootNode) : base(rootNode)
        {
        }
    }

    private class NodeImplementation : Node
    {
        public NodeImplementation() : base()
        {
        }

        public NodeImplementation(AbilityReference ability) : base(ability)
        {
        }
    }
}
