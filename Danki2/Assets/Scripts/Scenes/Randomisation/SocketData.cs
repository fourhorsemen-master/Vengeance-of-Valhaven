using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SocketData
{
    [SerializeField] private SocketRotationType socketRotationType;
    [SerializeField] private List<ModuleData> moduleData = new List<ModuleData>();

    public SocketRotationType SocketRotationType { get => socketRotationType; set => socketRotationType = value; }
    public List<ModuleData> ModuleData { get => moduleData; set => moduleData = value; }
}
