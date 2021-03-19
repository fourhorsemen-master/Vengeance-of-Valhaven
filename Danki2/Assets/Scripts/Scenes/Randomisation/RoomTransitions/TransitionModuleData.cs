using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TransitionModuleData
{
    [SerializeField] private TransitionModuleType type;
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private List<ModuleTag> tags = new List<ModuleTag>();

    public TransitionModuleType Type { get => type; set => type = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public List<ModuleTag> Tags { get => tags; set => tags = value; }
}
