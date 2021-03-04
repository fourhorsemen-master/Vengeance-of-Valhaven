using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField] private int version;
    
    [SerializeField] private Scene currentScene;
    [SerializeField] private int playerHealth;
    
    public int Version { get => version; set => version = value; }
    
    public Scene CurrentScene { get => currentScene; set => currentScene = value; }
    public int PlayerHealth { get => playerHealth; set => playerHealth = value; }

    public SaveData(int version, Scene currentScene, int playerHealth)
    {
        this.version = version;
        this.currentScene = currentScene;
        this.playerHealth = playerHealth;
    }
}
