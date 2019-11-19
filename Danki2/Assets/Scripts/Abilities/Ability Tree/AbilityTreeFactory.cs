using Assets.Scripts.Abilities;
using System;

namespace Abilities
{
    public static class AbilityTreeFactory
    {
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

        public static AbilityTree CreateTree(Node leftChild = null, Node rightChild = null)
        {
            Node rootNode = new NodeImplementation();
            rootNode.SetChild(Direction.Left, leftChild);
            rootNode.SetChild(Direction.Right, rightChild);
            return new AbilityTreeImplementation(rootNode);
        }

        public static Node CreateNode(AbilityReference ability, Node leftChild = null, Node rightChild = null)
        {
            Node node = new NodeImplementation(ability);
            node.SetChild(Direction.Left, leftChild);
            node.SetChild(Direction.Right, rightChild);
            return node;
        }
    }
}
