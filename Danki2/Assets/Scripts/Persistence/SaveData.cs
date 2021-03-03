using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField] private Scene currentScene;
    [SerializeField] private int playerHealth;
    
    public Scene CurrentScene { get => currentScene; set => currentScene = value; }
    public int PlayerHealth { get => playerHealth; set => playerHealth = value; }

    public SaveData(Scene currentScene, int playerHealth)
    {
        this.currentScene = currentScene;
        this.playerHealth = playerHealth;
    }
}
