using System;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSocket : MonoBehaviour, IComparable<ModuleSocket>
{
    [SerializeField] private int id = 0;
    [SerializeField] private SocketType socketType = default;
    [SerializeField] private List<ModuleTag> tags = new List<ModuleTag>();

    public int Id { get => id; set => id = value; }
    public SocketType SocketType { get => socketType; set => socketType = value; }
    public List<ModuleTag> Tags { get => tags; set => tags = value; }

    public int CompareTo(ModuleSocket other) => id == other.id ? 0 : id < other.id ? -1 : 1;
}
