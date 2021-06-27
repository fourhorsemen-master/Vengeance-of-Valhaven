using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModuleData
{
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private List<Zone> zones = new List<Zone>();
    [SerializeField] private List<ModuleTag> tags = new List<ModuleTag>();
    [SerializeField] private bool allowAnyFreeRotation = false;
    [SerializeField] private float minFreeRotation = 0;
    [SerializeField] private float maxFreeRotation = 0;
    [SerializeField] private List<float> distinctRotations = new List<float>();

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public List<Zone> Zones { get => zones; set => zones = value; }
    public List<ModuleTag> Tags { get => tags; set => tags = value; }
    public bool AllowAnyFreeRotation { get => allowAnyFreeRotation; set => allowAnyFreeRotation = value; }
    public float MinFreeRotation { get => minFreeRotation; set => minFreeRotation = value; }
    public float MaxFreeRotation { get => maxFreeRotation; set => maxFreeRotation = value; }
    public List<float> DistinctRotations { get => distinctRotations; set => distinctRotations = value; }
}
