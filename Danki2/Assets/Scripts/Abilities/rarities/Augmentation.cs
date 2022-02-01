using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Augmentation
{
    [SerializeField] private string descriptor = "";
    [SerializeField] private List<Empowerment> empowerments = new List<Empowerment>();

    public string Descriptor { get => descriptor; set => descriptor = value; }
    public List<Empowerment> Empowerments { get => empowerments; set => empowerments = value; }
}
