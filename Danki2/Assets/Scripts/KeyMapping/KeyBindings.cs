using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.KeyMapping
{
    // Serializable exposes the class to Unity's JsonUtility methods.
    [Serializable]
    public class KeyBindings
    {
        // SerializeField is required as by default JsonUtility ignores private fields.
        [SerializeField]
        private KeyCode up;
        [SerializeField]
        private KeyCode down;
        [SerializeField]
        private KeyCode left;
        [SerializeField]
        private KeyCode right;
        [SerializeField]
        private KeyCode leftAction;
        [SerializeField]
        private KeyCode rightAction;
        [SerializeField]
        private KeyCode dash;
        [SerializeField]
        private KeyCode pause;

        public KeyBindings(
            KeyCode up,
            KeyCode down,
            KeyCode left,
            KeyCode right,
            KeyCode leftAction,
            KeyCode rightAction,
            KeyCode dash,
            KeyCode pause
        )
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.leftAction = leftAction;
            this.rightAction = rightAction;
            this.dash = dash;
            this.pause = pause;
        }

        // With the method below, we can create KeyBindings using an existing one, and setting whichever optional parameters we like to be different.
        // This method will be useful when we are creating UI for changing keybinds.
        // Note that 0 is KeyCode.None which means no key.
        public KeyBindings(
            KeyBindings initialBindings,
            KeyCode up = 0,
            KeyCode down = 0,
            KeyCode left = 0,
            KeyCode right = 0,
            KeyCode leftAction = 0,
            KeyCode rightAction = 0,
            KeyCode dash = 0,
            KeyCode pause = 0
        )
        {
            this.up = up == 0 ? initialBindings.up : up;
            this.down = down == 0 ? initialBindings.down : down;
            this.left = left == 0 ? initialBindings.left : left;
            this.right = right == 0 ? initialBindings.right : right;
            this.leftAction = leftAction == 0 ? initialBindings.leftAction : leftAction;
            this.rightAction = rightAction == 0 ? initialBindings.rightAction : rightAction;
            this.dash = dash == 0 ? initialBindings.dash : dash;
            this.pause = pause == 0 ? initialBindings.pause : pause;
        }

        public KeyCode Up => this.up;
        public KeyCode Down => this.down;
        public KeyCode Left => this.left;
        public KeyCode Right => this.right;
        public KeyCode LeftAction => this.leftAction;
        public KeyCode RightAction => this.rightAction;
        public KeyCode Dash => this.dash;
        public KeyCode Pause => this.pause;
    }
}
