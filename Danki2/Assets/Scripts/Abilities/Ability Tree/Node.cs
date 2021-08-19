using System;
using UnityEngine;

public abstract class Node
{
    private readonly EnumDictionary<Direction, Node> children = new EnumDictionary<Direction, Node>(defaultValue: null);

    public bool IsRootNode => Parent == null;

    public bool IsParent => HasChild(Direction.Left) || HasChild(Direction.Right);

    public Node Parent { get; set; }

    public Ability2 Ability { get; private set; }

    public int Depth => IsRootNode ? 0 : Parent.Depth + 1;

    private EnumDictionary<Direction, Subscription> childChangeSubscriptions = new EnumDictionary<Direction, Subscription>(defaultValue: null);
    public Subject ChangeSubject { get; } = new Subject();

    protected Node()
    {
    }

    protected Node(Ability2 ability)
    {
        Ability = ability;
    }

    public bool HasChild(Direction direction)
    {
        return children[direction] != null;
    }

    public Node GetChild(Direction direction)
    {
        return children[direction];
    }

    public void SetChild(Direction direction, Node child)
    {
        children[direction] = child;
        child.Parent = this;

        childChangeSubscriptions[direction]?.Unsubscribe();
        childChangeSubscriptions[direction] = child.ChangeSubject.Subscribe(ChangeSubject.Next);
    }

    public void RemoveChild(Direction direction)
    {
        children[direction] = null;

        childChangeSubscriptions[direction]?.Unsubscribe();
        childChangeSubscriptions[direction] = null;

        ChangeSubject.Next();
    }

    public Direction GetDirectionFromParent()
    {
        if (Parent.HasChild(Direction.Left) && Parent.GetChild(Direction.Left) == this)
            return Direction.Left;

        if (Parent.HasChild(Direction.Right) && Parent.GetChild(Direction.Right) == this)
            return Direction.Right;

        Debug.LogError("Parent doesn't recognise child.");
        return default;
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

    public void Insert(Ability2 ability, InsertArea area)
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
                Ability = ability;
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

    public void RemoveSelfAndDescendants()
    {
        Parent.RemoveChild(GetDirectionFromParent());
    }

    public bool CanSwapAbilitiesWith(Node otherNode)
    {
        return true;
        // return
        //     this != otherNode
        //     && !(otherNode.IsParent && AbilityLookup.Instance.IsFinisher(Ability))
        //     && !(IsParent && AbilityLookup.Instance.IsFinisher(otherNode.Ability));
    }

    public void SwapAbilitiesWith(Node node)
    {
        if (!CanSwapAbilitiesWith(node)) return;

        Ability2 otherAbility = node.Ability;
        node.Ability = Ability;
        Ability = otherAbility;

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
    /// Runs the given action for this node and all descendent nodes.
    /// </summary>
    /// <param name="action"> The action to run </param>
    /// <param name="predicate"> Optional predicate, if not provided then the action will run for every node </param>
    public void IterateDown(Action<Node> action, Func<Node, bool> predicate = null)
    {
        if (predicate == null || predicate(this)) action(this);

        EnumUtils.ForEach<Direction>(d => children[d]?.IterateDown(action, predicate));
    }
}
