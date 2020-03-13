using UnityEngine;

public abstract class Node
{
    [SerializeField]
    private AbilityTreeChildren _children = new AbilityTreeChildren((Node)null);

    private AbilityReference _ability;

    protected Node()
    {
    }

    protected Node(AbilityReference ability)
    {
        _ability = ability;
    }

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
}
