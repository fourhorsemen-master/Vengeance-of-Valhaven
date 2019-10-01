using Abilities;

namespace AbilityTree
{
    public static class AbilityTreeFactory
    {
        // These private implementations of the abstract classes ensure that all instantiation
        // of ability trees and nodes happen from this factory class.
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

        public static AbilityTree CreateTree(Node leftChild = null, Node rightChild = null)
        {
            Node rootNode = new NodeImplementation();
            rootNode.SetChild(Direction.LEFT, leftChild);
            rootNode.SetChild(Direction.RIGHT, rightChild);
            return new AbilityTreeImplementation(rootNode);
        }

        public static Node CreateNode(Ability ability, Node leftChild = null, Node rightChild = null)
        {
            Node node = new NodeImplementation(ability);
            node.SetChild(Direction.LEFT, leftChild);
            node.SetChild(Direction.RIGHT, rightChild);
            return node;
        }
    }

    class Test
    {
        public void test()
        {
            AbilityTree tree = AbilityTreeFactory.CreateTree(
                AbilityTreeFactory.CreateNode(Ability.SHIELD_BASH),
                AbilityTreeFactory.CreateNode(Ability.SLASH)
            );
        }
    }
}
