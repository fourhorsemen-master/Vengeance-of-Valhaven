using System;
using UnityEngine;

[Serializable]
public class AbilityTree
{
    [SerializeField]
    private Node _rootNode;

    private Node _currentNode;

    private int _currentDepth;

    // The root node counts itself when calculating depth, as it is just the same as any
    // other node. But in terms of the ability tree, we want to discount it.
    public int MaxDepth => _rootNode.MaxDepth() - 1;

    public BehaviourSubject<Node> TreeWalkSubject { get; }

    public BehaviourSubject<int> CurrentDepthSubject { get; }

    public AbilityTree(Node rootNode)
    {
        _rootNode = rootNode;
        _currentNode = _rootNode;
        _currentDepth = 0;

        TreeWalkSubject = new BehaviourSubject<Node>(_currentNode);
        CurrentDepthSubject = new BehaviourSubject<int>(_currentDepth);
    }

    public Node GetChild(Direction direction)
    {
        return _rootNode.GetChild(direction);
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
        return _currentNode.GetChild(direction).GetAbility();
    }

    public AbilityReference Walk(Direction direction)
    {
        _currentNode = _currentNode.GetChild(direction);
        TreeWalkSubject.Next(_currentNode);

        _currentDepth++;
        CurrentDepthSubject.Next(_currentDepth);

        return _currentNode.GetAbility();
    }

    public void Reset()
    {
        _currentNode = _rootNode;
        TreeWalkSubject.Next(_currentNode);

        _currentDepth = 0;
        CurrentDepthSubject.Next(_currentDepth);
    }

    public void IterateDown(Action<Node> callback, bool includeRoot = false)
    {
        if (includeRoot) callback.Invoke(_rootNode);

        GetChild(Direction.Left).IterateDown(callback);
        GetChild(Direction.Right).IterateDown(callback);
    }
}
