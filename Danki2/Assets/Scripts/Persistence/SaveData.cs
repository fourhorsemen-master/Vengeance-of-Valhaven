using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField]
    private Scene currentScene;

    public Scene CurrentScene { get => currentScene; set => currentScene = value; }
}
