using Abilities;

namespace AbilityTree
{
    public static class AbilityTreeFactory
    {
        private class AbilityTreeImplementation : AbilityTree
        {
            public AbilityTreeImplementation(Node rootNode) : base(rootNode) { }
        }

        private class NodeImplementation : Node
        {
            public NodeImplementation(Ability ability) : base(ability) { }
        }

        public static AbilityTree CreateTree(Node leftChild, Node rightChild)
        {
            Node rootNode = new NodeImplementation();
            rootNode.SetChild(Direction.LEFT, leftChild);
            rootNode.SetChild(Direction.RIGHT, rightChild);
            return new AbilityTreeImplementation(rootNode);
        }

        public static Node CreateNode(Ability ability)
        {
            return new NodeImplementation
            {
                Ability = ability
            };
        }

        public static Node CreateNode(Ability ability, Node leftChild, Node rightChild)
        {
            Node node = new NodeImplementation
            {
                Ability = ability
            };
            node.SetChild(Direction.LEFT, leftChild);
            node.SetChild(Direction.RIGHT, rightChild);
            return node;
        }
    }
}
