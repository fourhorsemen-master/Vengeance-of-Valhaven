using System.Collections.Generic;
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

    public Dictionary<SerializableGuid, int> OwnedAbilities { get; }

    public Dictionary<SerializableGuid, int> Inventory { get; private set; }

    public Direction DirectionLastWalked { get; private set; }

    protected AbilityTree(Dictionary<SerializableGuid, int> ownedAbilities, Node rootNode)
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

    public SerializableGuid GetAbilityId(Direction direction)
    {
        return currentNode.GetChild(direction).AbilityId;
    }

    public void Walk(Direction direction)
    {
        currentNode = currentNode.GetChild(direction);
        TreeWalkSubject.Next(currentNode);

        CurrentDepth++;
        CurrentDepthSubject.Next(CurrentDepth);

        DirectionLastWalked = direction;
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

    public void AddToInventory(SerializableGuid abilityId)
    {
        OwnedAbilities[abilityId]++;
        UpdateInventory();
    }

    public SerializableAbilityTree Serialize()
    {
        return new SerializableAbilityTree(this);
    }

    private void UpdateInventory()
    {
        Inventory = new Dictionary<SerializableGuid, int>();
        AbilityLookup.Instance.ForEachAbilityId(abilityId =>
        {
            Inventory[abilityId] = OwnedAbilities[abilityId];
        });

        RootNode.IterateDown(
            n =>
            {
                Inventory[n.AbilityId] -= 1;
                if (Inventory[n.AbilityId] < 0) Debug.LogError("Tree abilities not subset of owned abilities.");
            },
            n => !n.IsRootNode
        );
    }
}
