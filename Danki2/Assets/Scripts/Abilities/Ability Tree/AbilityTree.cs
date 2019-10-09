using Assets.Scripts.Abilities;
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

        public AbilityBuilder GetAbilityBuilder(Direction direction)
        {
            var ability = currentNode.GetChild(direction).Ability;

            return AbilityBuilder.GetAbilityBuilder(ability);
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
