public static class AbilityTreeFactory
{
    // Root node must have two children for ability tree to be functional.
    public static AbilityTree CreateTree(Node leftChild, Node rightChild)
    {
        Node rootNode = new Node();
        rootNode.SetChild(Direction.Left, leftChild);
        rootNode.SetChild(Direction.Right, rightChild);
        return new AbilityTree(rootNode);
    }

    public static Node CreateNode(AbilityReference ability, Node leftChild = null, Node rightChild = null)
    {
        Node node = new Node();
        node.SetAbility(ability);
        node.SetChild(Direction.Left, leftChild);
        node.SetChild(Direction.Right, rightChild);
        return node;
    }

    public static AbilityTree Default()
    {
        return CreateTree(CreateNode(default),CreateNode(default));
    }
}
