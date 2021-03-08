using System;
using UnityEngine;

[Serializable]
public class SerializableSceneTransitioner
{
    [SerializeField] private int sceneTransitionerId;
    [SerializeField] private int nextSceneId;

    public int SceneTransitionerId { get => sceneTransitionerId; set => sceneTransitionerId = value; }
    public int NextSceneId { get => nextSceneId; set => nextSceneId = value; }
}
