using UnityEngine;

public abstract class AbilityTree
{
    public Node RootNode { get; }

    private Node _currentNode;

    public int CurrentDepth { get; private set; }

    /// <summary>
    /// Includes the root node - so the result is greater than 0
    /// </summary>
    public int MaxDepth => RootNode.MaxDepth();

    public BehaviourSubject<Node> TreeWalkSubject { get; }

    public BehaviourSubject<int> CurrentDepthSubject { get; }

    public Subject ChangeSubject { get; } = new Subject();

    private EnumDictionary<AbilityReference, int> ownedAbilities;

    public EnumDictionary<AbilityReference, int> Inventory { get; private set; }

    public Direction DirectionLastWalked { get; private set; }

    protected AbilityTree(EnumDictionary<AbilityReference, int> ownedAbilities, Node rootNode)
    {
        this.ownedAbilities = ownedAbilities;

        RootNode = rootNode;
        _currentNode = RootNode;
        CurrentDepth = 0;

        TreeWalkSubject = new BehaviourSubject<Node>(_currentNode);
        CurrentDepthSubject = new BehaviourSubject<int>(CurrentDepth);

        UpdateInventory();

        RootNode.ChangeSubject.Subscribe(() => {
            UpdateInventory();
            ChangeSubject.Next();
        });
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

        CurrentDepth++;
        CurrentDepthSubject.Next(CurrentDepth);

        DirectionLastWalked = direction;

        return _currentNode.Ability;
    }

    public bool WalkingEndsCombo(Direction direction)
    {
        return _currentNode.HasChild(direction) && !_currentNode.GetChild(direction).IsParent;
    }

    public void Reset()
    {
        _currentNode = RootNode;
        TreeWalkSubject.Next(_currentNode);

        CurrentDepth = 0;
        CurrentDepthSubject.Next(CurrentDepth);
    }

    private void UpdateInventory()
    {
        Inventory = new EnumDictionary<AbilityReference, int>(ownedAbilities);

        RootNode.IterateDown(
            n =>
            {
                Inventory[n.Ability] -= 1;
                if (Inventory[n.Ability] < 0) Debug.LogError("Tree abilities not subset of owned abilities.");
            },
            n => !n.IsRootNode
        );
    }
}
