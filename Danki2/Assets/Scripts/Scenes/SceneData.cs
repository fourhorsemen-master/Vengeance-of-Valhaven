using System;
using UnityEngine;

[Serializable]
public class SceneData
{
    [SerializeField] private string fileName;
    [SerializeField] private SceneType sceneType;
    [SerializeField] private GameplaySceneData gameplaySceneData = new GameplaySceneData();

    public string FileName { get => fileName; set => fileName = value; }
    public SceneType SceneType { get => sceneType; set => sceneType = value; }
    public GameplaySceneData GameplaySceneData { get => gameplaySceneData; set => gameplaySceneData = value; }
}
