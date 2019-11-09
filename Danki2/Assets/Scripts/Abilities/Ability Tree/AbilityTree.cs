using Assets.Scripts.Abilities;
using System;

namespace Abilities
{
    public abstract class AbilityTree
    {
        private readonly Node _rootNode;

        private Node _currentNode;

        protected AbilityTree(Node rootNode)
        {
            _rootNode = rootNode;
            _currentNode = _rootNode;
        }

        public bool CanWalk(Direction direction)
        {
            return _currentNode.HasChild(direction);
        }

        public AbilityReference GetAbility(Direction direction)
        {
            return _currentNode.GetChild(direction).Ability;
        }

        public void Walk(Direction direction)
        {
            _currentNode = _currentNode.GetChild(direction);
        }

        public void Reset()
        {
            _currentNode = _rootNode;
        }
    }
}
