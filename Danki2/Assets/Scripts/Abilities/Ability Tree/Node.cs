using System.Collections.Generic;

using Abilities;
using Utils;

namespace AbilityTree
{
    public abstract class Node
    {
        public Ability Ability { get; }

        private readonly Dictionary<Direction, Node> children = new Dictionary<Direction, Node>();

        protected Node()
        {
        }

        protected Node(Ability ability)
        {
            Ability = ability;
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
