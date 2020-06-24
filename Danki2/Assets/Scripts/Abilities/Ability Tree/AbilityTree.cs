﻿public abstract class AbilityTree
{
    public Node RootNode { get; }

    private Node _currentNode;

    private int _currentDepth;

    public int MaxDepth { get; }

    public BehaviourSubject<Node> TreeWalkSubject { get; }

    public BehaviourSubject<int> CurrentDepthSubject { get; }

    public bool AtRoot => _currentDepth == 0;

    protected AbilityTree(Node rootNode)
    {
        RootNode = rootNode;
        _currentNode = RootNode;
        _currentDepth = 0;

        // The root node counts itself when calculating depth, as it is just the same as any
        // other node. But in terms of the ability tree, we want to discount it.
        MaxDepth = RootNode.MaxDepth() - 1;

        TreeWalkSubject = new BehaviourSubject<Node>(_currentNode);
        CurrentDepthSubject = new BehaviourSubject<int>(_currentDepth);
    }

    public bool CanWalkDirection(Direction direction)
    {
        return _currentNode.HasChild(direction);
    }

    public bool CanWalk()
    {
        return _currentNode.HasChild(Direction.Left) || _currentNode.HasChild(Direction.Right);
    }

    public AbilityReference GetAbility(Direction direction)
    {
        return _currentNode.GetChild(direction).Ability;
    }

    public AbilityReference Walk(Direction direction)
    {
        _currentNode = _currentNode.GetChild(direction);
        TreeWalkSubject.Next(_currentNode);

        _currentDepth++;
        CurrentDepthSubject.Next(_currentDepth);

        return _currentNode.Ability;
    }

    public void Reset()
    {
        _currentNode = RootNode;
        TreeWalkSubject.Next(_currentNode);

        _currentDepth = 0;
        CurrentDepthSubject.Next(_currentDepth);
    }
}
