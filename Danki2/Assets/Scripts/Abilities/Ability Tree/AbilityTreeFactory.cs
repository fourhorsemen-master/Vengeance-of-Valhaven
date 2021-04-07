public static class AbilityTreeFactory
{
    // Root node must have two children for ability tree to be functional.
    public static AbilityTree CreateTree(EnumDictionary<AbilityReference, int> ownedAbilities, Node leftChild, Node rightChild)
    {
        Node rootNode = new NodeImplementation();
        if (leftChild != null) rootNode.SetChild(Direction.Left, leftChild);
        if (rightChild != null) rootNode.SetChild(Direction.Right, rightChild);
        return new AbilityTreeImplementation(ownedAbilities, rootNode);
    }

    public static Node CreateNode(AbilityReference ability, Node leftChild = null, Node rightChild = null)
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
        public AbilityTreeImplementation(EnumDictionary<AbilityReference, int> ownedAbilities, Node rootNode) : base(ownedAbilities, rootNode)
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
