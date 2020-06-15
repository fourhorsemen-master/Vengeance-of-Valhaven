using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private readonly Dictionary<Direction, Node> _children = new Dictionary<Direction, Node>();

    public bool IsRootNode => Parent == null;

    public Node Parent { get; set; }

    public AbilityReference Ability { get; private set; }

    protected Node()
    {
    }

    protected Node(AbilityReference ability)
    {
        Ability = ability;
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

    public void SetAbility(AbilityReference ability)
    {
        Ability = ability;
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
    /// <param name="predicate"> Optional predicate, if not provided then the action will run for every node </param>
    public void IterateUp(Action<Node> action, Func<Node, bool> predicate = null)
    {
        if (predicate == null || predicate(this)) action(this);
        Parent?.IterateUp(action, predicate);
    }
}
