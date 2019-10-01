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

        public bool CanWalk()
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (!CanWalk(direction))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanWalk(Direction direction)
        {
            return currentNode.HasChild(direction);
        }

        public Ability GetAbility(Direction direction)
        {
            return currentNode.GetChild(direction).Ability;
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
