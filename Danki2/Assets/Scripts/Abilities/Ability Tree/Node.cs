using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private readonly Dictionary<Direction, Node> children = new Dictionary<Direction, Node>();

    public Ability Ability { get; }

    protected Node()
    {
    }

    protected Node(Ability ability)
    {
        Ability = ability;
    }

    public bool HasChild(Direction direction)
    {
        return children.TryGetValue(direction, out _);
    }

    public Node GetChild(Direction direction)
    {
        return children[direction];
    }

    public void SetChild(Direction direction, Node value)
    {
        children[direction] = value;
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
}
