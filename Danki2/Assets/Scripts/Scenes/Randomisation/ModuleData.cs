using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModuleData
{
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private List<ModuleTag> tags = new List<ModuleTag>();

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public List<ModuleTag> Tags { get => tags; set => tags = value; }
}
