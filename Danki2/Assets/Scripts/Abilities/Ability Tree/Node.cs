using Assets.Scripts.Abilities;
using System;
using System.Collections.Generic;

namespace Abilities
{
    public abstract class Node
    {
        private readonly Dictionary<Direction, Node> children = new Dictionary<Direction, Node>();

        public AbilityReference Ability { get; }

        protected Node()
        {
        }

        protected Node(AbilityReference ability)
        {
            this.Ability = ability;
        }

        public bool HasChild(Direction direction)
        {
            return children.TryGetValue(direction, out _);
        }

        public Node GetChild(Direction direction)
        {
            return children[direction];
        }

        public void SetChild(Direction direction, Node value)
        {
            children[direction] = value;
        }
    }
}
