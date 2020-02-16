public abstract class AbilityTree
{
    private readonly Node _rootNode;

    private Node _currentNode;

    public BehaviourSubject<Node> TreeWalkSubject { get; }

    protected AbilityTree(Node rootNode)
    {
        _rootNode = rootNode;
        _currentNode = _rootNode;

        TreeWalkSubject = new BehaviourSubject<Node>(_currentNode);
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
        return _currentNode.Ability;
    }

    public void Reset()
    {
        _currentNode = _rootNode;
        TreeWalkSubject.Next(_currentNode);
    }
}
