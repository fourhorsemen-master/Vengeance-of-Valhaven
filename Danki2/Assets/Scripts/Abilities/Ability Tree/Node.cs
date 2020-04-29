using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private Node parent;

    private readonly Dictionary<Direction, Node> _children = new Dictionary<Direction, Node>();

    public AbilityReference Ability { get; }

    protected Node()
    {
    }

    protected Node(AbilityReference ability)
    {
        Ability = ability;
    }

    public bool HasParent()
    {
        return parent != null;
    }

    public Node GetParent()
    {
        return parent;
    }

    public void SetParent(Node parent)
    {
        this.parent = parent;
    }

    public bool HasChild(Direction direction)
    {
        return _children.TryGetValue(direction, out _);
    }

    public Node GetChild(Direction direction)
    {
        return _children[direction];
    }

    public void SetChild(Direction direction, Node value)
    {
        _children[direction] = value;
    }

    public int MaxDepth()
    {
        int maxLeftDepth = 0;
        if (HasChild(Direction.Left))
        {
            maxLeftDepth = GetChild(Direction.Left).MaxDepth();
        }

        int maxRightDepth = 0;
        if (HasChild(Direction.Right))
        {
            maxRightDepth = GetChild(Direction.Right).MaxDepth();
        }

        return Mathf.Max(maxLeftDepth, maxRightDepth) + 1;
    }

    /// <summary>
    /// Runs the given action for this node and all ancestor nodes up the tree.
    /// </summary>
    /// <param name="action"> The action to run </param>
    public void IterateUp(Action<Node> action)
    {
        action(this);
        parent?.IterateUp(action);
    }
}
