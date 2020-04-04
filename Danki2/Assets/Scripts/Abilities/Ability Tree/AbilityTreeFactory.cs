public static class AbilityTreeFactory
{
    // Root node must have two children for ability tree to be functional.
    public static AbilityTree CreateTree(Node leftChild, Node rightChild)
    {
        Node rootNode = new NodeImplementation();
        rootNode.SetChild(Direction.Left, leftChild);
        rootNode.SetChild(Direction.Right, rightChild);
        return new AbilityTreeImplementation(rootNode);
    }

    public static Node CreateNode(Ability ability, Node leftChild = null, Node rightChild = null)
    {
        Node node = new NodeImplementation(ability);
        if (leftChild != null) node.SetChild(Direction.Left, leftChild);
        if (rightChild != null) node.SetChild(Direction.Right, rightChild);
        return node;
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

        public NodeImplementation(Ability ability) : base(ability)
        {
        }
    }
}
