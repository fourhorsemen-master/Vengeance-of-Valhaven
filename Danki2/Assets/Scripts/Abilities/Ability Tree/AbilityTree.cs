using System;

namespace Abilities
{
    public abstract class AbilityTree
    {
        private readonly Node rootNode;

        private Node currentNode;

        protected AbilityTree(Node rootNode)
        {
            this.rootNode = rootNode;
            currentNode = this.rootNode;
        }

        public bool CanWalk(Direction direction)
        {
            return currentNode.HasChild(direction);
        }

        public Action<AbilityContext, Ability> GetAbilityBuilder(Direction direction)
        {
            return currentNode.GetChild(direction).AbilityBuilder;
        }

        public void Walk(Direction direction)
        {
            currentNode = currentNode.GetChild(direction);
        }

        public void Reset()
        {
            currentNode = rootNode;
        }
    }
}
