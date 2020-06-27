using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private readonly Dictionary<Direction, Node> _children = new Dictionary<Direction, Node>();

    public bool IsRootNode => Parent == null;

    public Node Parent { get; set; }

    public AbilityReference Ability { get; private set; }

    private EnumDictionary<Direction, Subscription> childChangeSubscriptions = new EnumDictionary<Direction, Subscription>(defaultValue: null);

    private Subscription leftChildChangeSubscription = null;
    private Subscription rightChildChangeSubscription = null;
    public Subject ChangeSubject { get; } = new Subject();

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

    public void SetChild(Direction direction, Node child)
    {
        _children[direction] = child;
        child.Parent = this;

        Subscription subscription = childChangeSubscriptions[direction];
        if (subscription != null) subscription.Unsubscribe();
        subscription = child.ChangeSubject.Subscribe(ChangeSubject.Next);
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

    public void Insert(AbilityReference ability, InsertArea area)
    {
        Node newNode = AbilityTreeFactory.CreateNode(ability);

        switch (area)
        {
            case InsertArea.Centre:
                if (IsRootNode)
                {
                    Debug.LogError("Tried to insert ability into root node.");
                    return;
                }
                SetAbility(ability);
                break;

            case InsertArea.BottomLeft:
                if (HasChild(Direction.Left))
                {
                    Node child = GetChild(Direction.Left);
                    newNode.SetChild(Direction.Left, child);
                }

                SetChild(Direction.Left, newNode);
                break;

            case InsertArea.BottomRight:
                if (HasChild(Direction.Right))
                {
                    Node child = GetChild(Direction.Right);
                    newNode.SetChild(Direction.Right, child);
                }

                SetChild(Direction.Right, newNode);
                break;
        }

        ChangeSubject.Next();
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

    /// <summary>
    /// Runs the given action for this node and all decendent nodes.
    /// </summary>
    /// <param name="action"> The action to run </param>
    /// <param name="predicate"> Optional predicate, if not provided then the action will run for every node </param>
    public void IterateDown(Action<Node> action, Func<Node, bool> predicate = null)
    {
        if (predicate == null || predicate(this)) action(this);

        if (HasChild(Direction.Left))
            GetChild(Direction.Left).IterateDown(action, predicate);

        if (HasChild(Direction.Right))
            GetChild(Direction.Right).IterateDown(action, predicate);
    }
}
