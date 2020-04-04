public abstract class AbilityTree
{
    private readonly Node rootNode;

    private Node currentNode;

    private int currentDepth;

    public int MaxDepth { get; }

    public BehaviourSubject<Node> TreeWalkSubject { get; }

    public BehaviourSubject<int> CurrentDepthSubject { get; }

    protected AbilityTree(Node rootNode)
    {
        this.rootNode = rootNode;
        currentNode = this.rootNode;
        currentDepth = 0;

        // The root node counts itself when calculating depth, as it is just the same as any
        // other node. But in terms of the ability tree, we want to discount it.
        MaxDepth = this.rootNode.MaxDepth() - 1;

        TreeWalkSubject = new BehaviourSubject<Node>(currentNode);
        CurrentDepthSubject = new BehaviourSubject<int>(currentDepth);
    }

    public bool CanWalkDirection(Direction direction)
    {
        return currentNode.HasChild(direction);
    }

    public bool CanWalk()
    {
        return currentNode.HasChild(Direction.Left) || currentNode.HasChild(Direction.Right);
    }

    public Ability GetAbility(Direction direction)
    {
        return currentNode.GetChild(direction).Ability;
    }

    public Ability Walk(Direction direction)
    {
        currentNode = currentNode.GetChild(direction);
        TreeWalkSubject.Next(currentNode);

        currentDepth++;
        CurrentDepthSubject.Next(currentDepth);

        return currentNode.Ability;
    }

    public void Reset()
    {
        currentNode = rootNode;
        TreeWalkSubject.Next(currentNode);

        currentDepth = 0;
        CurrentDepthSubject.Next(currentDepth);
    }
}
