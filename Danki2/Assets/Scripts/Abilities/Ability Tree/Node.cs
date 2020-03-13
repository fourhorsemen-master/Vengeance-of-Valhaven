using System;
using UnityEngine;

[Serializable]
public class Node
{
    [SerializeField]
    private AbilityTreeChildren _children = new AbilityTreeChildren((Node)null);

    [SerializeField]
    private AbilityReference _ability;

    public bool HasChild(Direction direction)
    {
        return _children[direction] != null;
    }

    public Node GetChild(Direction direction)
    {
        return _children[direction];
    }

    public void SetChild(Direction direction, Node value)
    {
        _children[direction] = value;
    }

    public AbilityReference GetAbility()
    {
        return _ability;
    }

    public void SetAbility(AbilityReference ability)
    {
        _ability = ability;
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

    public void IterateDown(Action<Node> callback)
    {
        callback.Invoke(this);

        if (HasChild(Direction.Left)) GetChild(Direction.Left).IterateDown(callback);
        if (HasChild(Direction.Right)) GetChild(Direction.Right).IterateDown(callback);
    }
}
