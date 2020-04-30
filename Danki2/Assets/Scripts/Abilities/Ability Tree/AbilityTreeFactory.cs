public static class AbilityTreeFactory
{
    // Root node must have two children for ability tree to be functional.
    public static AbilityTree CreateTree(Node leftChild, Node rightChild)
    {
        Node rootNode = new NodeImplementation();
        SetParentAndChild(rootNode, leftChild, Direction.Left);
        SetParentAndChild(rootNode, rightChild, Direction.Right);
        return new AbilityTreeImplementation(rootNode);
    }

    public static Node CreateNode(AbilityReference ability, Node leftChild = null, Node rightChild = null)
    {
        Node node = new NodeImplementation(ability);
        if (leftChild != null) SetParentAndChild(node, leftChild, Direction.Left);
        if (rightChild != null) SetParentAndChild(node, rightChild, Direction.Right);
        return node;
    }

    private static void SetParentAndChild(Node parent, Node child, Direction direction)
    {
        parent.SetChild(direction, child);
        child.Parent = parent;
    }

    // These private implementations of the abstract classes ensure that all instantiation
    // of ability trees and nodes happen from this factory class. See the factory pattern
    // for more information
    private class AbilityTreeImplementation : AbilityTree
    {
        public AbilityTreeImplementation(Node rootNode) : base(rootNode)
        {
        }
    }

    private class NodeImplementation : Node
    {
        public NodeImplementation() : base()
        {
        }

        public NodeImplementation(AbilityReference ability) : base(ability)
        {
        }
    }
}
