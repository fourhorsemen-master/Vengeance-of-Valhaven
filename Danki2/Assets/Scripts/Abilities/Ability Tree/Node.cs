using System.Collections.Generic;

using Abilities;
using Utils;

namespace AbilityTree
{
    public abstract class Node
    {
        public Ability Ability { get; private set; }

        private readonly Dictionary<Direction, Node> children = DictionaryUtils.EnumDictionary<Direction, Node>(null);

        protected Node(Ability ability)
        {
            Ability = ability;
        }

        public bool HasChild(Direction direction)
        {
            return children[direction] == null;
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
