﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameplaySceneData
{
    [SerializeField] private List<Pole> cameraOrientations = new List<Pole>();
    [SerializeField] private List<EntranceData> entranceData = new List<EntranceData>();
    [SerializeField] private List<ExitData> exitData = new List<ExitData>();

    public List<Pole> CameraOrientations { get => cameraOrientations; set => cameraOrientations = value; }
    public List<EntranceData> EntranceData { get => entranceData; set => entranceData = value; }
    public List<ExitData> ExitData { get => exitData; set => exitData = value; }
}
