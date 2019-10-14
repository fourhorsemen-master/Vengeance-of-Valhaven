using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.KeyMapping
{
    // KeyMapper works as a global singleton accessed from anywhere by KeyMapper.Mapper, which points to a static KeyMapper.
    // KeyMapper loads and saves keybindings using a json file.
    public class KeyMapper
    {
        private static string keyBindingsDirectory = Application.persistentDataPath;
        private static string keyBindingsFilePath = keyBindingsDirectory + "/keyBindings.json";

        // These default bindings are used if no keybindings file exists.
        private static KeyBindings defaultBindings = new KeyBindings
        (
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
            KeyCode.D,
            KeyCode.Mouse0,
            KeyCode.Mouse1,
            KeyCode.Space,
            KeyCode.Escape
        );

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

        public KeyBindings Bindings { get; private set; }

        public static KeyMapper Mapper { get; } = new KeyMapper();

        public void SetBindings(KeyBindings bindings)
        {
            this.Bindings = bindings;
            this.SaveBindings();
        }

        // On first run, settings file won't exist, so we create a new one. Otherwise we return the loaded settings.
        private bool TryLoadBindings(out KeyBindings bindings)
        {
            // We create the directory if it doesn't exist already.
            Directory.CreateDirectory(keyBindingsDirectory);

            if (!File.Exists(keyBindingsFilePath))
            {
                bindings = default;
                return false;
            }

            var bindingsJsonString = File.ReadAllText(keyBindingsFilePath);
            bindings = JsonUtility.FromJson<KeyBindings>(bindingsJsonString);
            return true;
        }

        // Save bindings to the json file.
        private void SaveBindings()
        {
            var bindingsJson = JsonUtility.ToJson(this.Bindings);
            File.WriteAllText(keyBindingsFilePath, bindingsJson);
        }
    }
}
