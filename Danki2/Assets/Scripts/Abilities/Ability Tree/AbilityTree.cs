using UnityEngine;

public abstract class AbilityTree
{
    public Node RootNode { get; }

    private Node currentNode;

    public Node CurrentNode => currentNode;

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
        currentNode = RootNode;
        CurrentDepth = 0;

        TreeWalkSubject = new BehaviourSubject<Node>(currentNode);
        CurrentDepthSubject = new BehaviourSubject<int>(CurrentDepth);

        UpdateInventory();

        RootNode.ChangeSubject.Subscribe(() => {
            UpdateInventory();
            ChangeSubject.Next();
        });
    }

    public bool CanWalkDirection(Direction direction)
    {
        return currentNode.HasChild(direction);
    }

    public bool CanWalk()
    {
        return currentNode.HasChild(Direction.Left) || currentNode.HasChild(Direction.Right);
    }

    public Ability2 GetAbility(Direction direction)
    {
        return currentNode.GetChild(direction).Ability;
    }

    public Ability2 Walk(Direction direction)
    {
        currentNode = currentNode.GetChild(direction);
        TreeWalkSubject.Next(currentNode);

        CurrentDepth++;
        CurrentDepthSubject.Next(CurrentDepth);

        DirectionLastWalked = direction;

        return currentNode.Ability;
    }

    public bool WalkingEndsCombo(Direction direction)
    {
        return currentNode.HasChild(direction) && !currentNode.GetChild(direction).IsParent;
    }

    public void Reset()
    {
        currentNode = RootNode;
        TreeWalkSubject.Next(currentNode);

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
