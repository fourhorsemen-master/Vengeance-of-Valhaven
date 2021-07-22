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

    public EnumDictionary<Ability2, int> OwnedAbilities { get; }

    public EnumDictionary<Ability2, int> Inventory { get; private set; }

    public Direction DirectionLastWalked { get; private set; }

    protected AbilityTree(EnumDictionary<Ability2, int> ownedAbilities, Node rootNode)
    {
        OwnedAbilities = ownedAbilities;

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

    public Ability2 GetAbility(Direction direction)
    {
        return _currentNode.GetChild(direction).Ability;
    }

    public Ability2 Walk(Direction direction)
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

    public void AddToInventory(Ability2 abilityReference)
    {
        OwnedAbilities[abilityReference]++;
        UpdateInventory();
    }

    public SerializableAbilityTree Serialize()
    {
        return new SerializableAbilityTree(this);
    }

    private void UpdateInventory()
    {
        Inventory = new EnumDictionary<Ability2, int>(OwnedAbilities);

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
