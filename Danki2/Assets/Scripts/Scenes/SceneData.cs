using System;
using UnityEngine;

[Serializable]
public class SceneData
{
    [SerializeField] private string fileName;
    [SerializeField] private SceneType sceneType;

    public string FileName { get => fileName; set => fileName = value; }
    public SceneType SceneType { get => sceneType; set => sceneType = value; }
}
