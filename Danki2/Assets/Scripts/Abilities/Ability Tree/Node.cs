using System.Collections.Generic;

public abstract class Node
{
    private readonly Dictionary<Direction, Node> _children = new Dictionary<Direction, Node>();

    public AbilityReference Ability { get; }

    protected Node()
    {
    }

    protected Node(AbilityReference ability)
    {
        this.Ability = ability;
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
}
