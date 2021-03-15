using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameplaySceneData
{
    [SerializeField] private List<Pole> cameraOrientations = new List<Pole>();

    public List<Pole> CameraOrientations { get => cameraOrientations; set => cameraOrientations = value; }
}
