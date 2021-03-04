using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField] private int version;
    [SerializeField] private Scene currentScene;
    [SerializeField] private int playerHealth;
    [SerializeField] private SerializableAbilityTree serializableAbilityTree;
    
    public int Version { get => version; set => version = value; }
    public Scene CurrentScene { get => currentScene; set => currentScene = value; }
    public int PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public SerializableAbilityTree SerializableAbilityTree { get => serializableAbilityTree; set => serializableAbilityTree = value; }

    public SaveData(int version, Scene currentScene, int playerHealth, SerializableAbilityTree serializableAbilityTree)
    {
        this.version = version;
        this.currentScene = currentScene;
        this.playerHealth = playerHealth;
        this.serializableAbilityTree = serializableAbilityTree;
    }
}
