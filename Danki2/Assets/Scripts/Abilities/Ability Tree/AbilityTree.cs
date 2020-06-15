public abstract class AbilityTree
{
    public Node RootNode { get; }

    private Node _currentNode;

    private int _currentDepth;

    /// <summary>
    /// Includes the root node - so the result is greater than 0
    /// </summary>
    public int MaxDepth => RootNode.MaxDepth();

    public BehaviourSubject<Node> TreeWalkSubject { get; }

    public BehaviourSubject<int> CurrentDepthSubject { get; }

    public Subject ChangeSubject { get; } = new Subject();

    protected AbilityTree(Node rootNode)
    {
        RootNode = rootNode;
        _currentNode = RootNode;
        _currentDepth = 0;

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
