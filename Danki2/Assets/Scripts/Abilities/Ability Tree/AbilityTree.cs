public abstract class AbilityTree
{
    private readonly Node _rootNode;

    private Node _currentNode;

    private int _currentDepth;

    public int MaxDepth { get; }

    public BehaviourSubject<Node> TreeWalkSubject { get; }

    public BehaviourSubject<int> CurrentDepthSubject { get; }

    protected AbilityTree(Node rootNode)
    {
        _rootNode = rootNode;
        _currentNode = _rootNode;
        _currentDepth = 1;

        MaxDepth = _rootNode.MaxDepth();

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
        _currentNode = _rootNode;
        TreeWalkSubject.Next(_currentNode);

        _currentDepth = 1;
        CurrentDepthSubject.Next(_currentDepth);
    }
}
