using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.KeyMapping
{
    public enum Control
    {
        Up,
        Down,
        Left,
        Right,
        LeftAction,
        RightAction,
        Dash,
        Pause,
    }

    // KeyMapper works as a global singleton accessed fro anywhere by KeyMapper.Mapper.
    // KeyMapper loads and saves keybindings as json in a text file.
    public class KeyMapper
    {
        private static string keyBindingsFilePath = Application.persistentDataPath + "/Settings/KeyBindings.txt";

        private static Dictionary<Control, KeyCode> defaultBindings = new Dictionary<Control, KeyCode>
        {
            { Control.Up, KeyCode.W },
            { Control.Down, KeyCode.S },
            { Control.Left, KeyCode.A },
            { Control.Right, KeyCode.D },
            { Control.LeftAction, KeyCode.Mouse0 },
            { Control.RightAction, KeyCode.Mouse1 },
            { Control.Dash, KeyCode.Space },
            { Control.Pause, KeyCode.Escape },
        };

        private Dictionary<Control, KeyCode> Bindings;

        private KeyMapper()
        {
            if (TryLoadBindings(out var bindings))
            {
                this.Bindings = bindings;
                return;
            }

            this.Bindings = defaultBindings;
            this.SaveBindings();
        }

        public static KeyMapper Mapper { get; } = new KeyMapper();

        // On first run, settings file won't exist, so we create a new one. Otherwise we return the loaded settings.
        private bool TryLoadBindings(out Dictionary<Control, KeyCode> bindings)
        {
            if (!File.Exists(keyBindingsFilePath))
            {
                bindings = default;
                return false;
            }

            var bindingsJsonString = File.ReadAllText(keyBindingsFilePath);
            bindings = JsonUtility.FromJson<Dictionary<Control,KeyCode>>(bindingsJsonString);
            return true;
        }

        private void SaveBindings()
        {
            File.Create(keyBindingsFilePath);
            File.WriteAllText(keyBindingsFilePath, JsonUtility.ToJson(this.Bindings));
        }

        public void SetBinding(Control control, KeyCode code)
        {
            this.Bindings[control] = code;
            this.SaveBindings();
        }

        public KeyCode GetBinding(Control control)
        {
            if (!this.Bindings.TryGetValue(control, out var keyCode))
            {
                throw new KeyNotFoundException($"No keycode registered for control {keyCode.ToString()}");
            }

            return keyCode;
        }
    }
}
