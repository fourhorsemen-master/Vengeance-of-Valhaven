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
        private KeyCode _up;
        [SerializeField]
        private KeyCode _down;
        [SerializeField]
        private KeyCode _left;
        [SerializeField]
        private KeyCode _right;
        [SerializeField]
        private KeyCode _leftAction;
        [SerializeField]
        private KeyCode _rightAction;
        [SerializeField]
        private KeyCode _dash;
        [SerializeField]
        private KeyCode _pause;

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
            _up = up;
            _down = down;
            _left = left;
            _right = right;
            _leftAction = leftAction;
            _rightAction = rightAction;
            _dash = dash;
            _pause = pause;
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
            _up = up == 0 ? initialBindings._up : up;
            _down = down == 0 ? initialBindings._down : down;
            _left = left == 0 ? initialBindings._left : left;
            _right = right == 0 ? initialBindings._right : right;
            _leftAction = leftAction == 0 ? initialBindings._leftAction : leftAction;
            _rightAction = rightAction == 0 ? initialBindings._rightAction : rightAction;
            _dash = dash == 0 ? initialBindings._dash : dash;
            _pause = pause == 0 ? initialBindings._pause : pause;
        }

        public KeyCode Up => _up;
        public KeyCode Down => _down;
        public KeyCode Left => _left;
        public KeyCode Right => _right;
        public KeyCode LeftAction => _leftAction;
        public KeyCode RightAction => _rightAction;
        public KeyCode Dash => _dash;
        public KeyCode Pause => _pause;
    }
}
