using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.KeyMapping
{
    [Serializable]
    public class KeyBindings
    {
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

        public KeyCode Up => this.up;
        public KeyCode Down => this.up;
        public KeyCode Left => this.up;
        public KeyCode Right => this.up;
        public KeyCode LeftAction => this.up;
        public KeyCode RightAction => this.up;
        public KeyCode Dash => this.up;
        public KeyCode Pause => this.up;
    }
}
