using System;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSocket : MonoBehaviour, IComparable<ModuleSocket>, IId
{
    [SerializeField] private int id = 0;
    [SerializeField] private GameObject navBlocker = null;
    [SerializeField] private SocketType socketType = default;
    [SerializeField] private List<ModuleTag> tags = new List<ModuleTag>();

    public int Id { get => id; set => id = value; }
    public GameObject NavBlocker { get => navBlocker; set => navBlocker = value; }
    public SocketType SocketType { get => socketType; set => socketType = value; }
    public List<ModuleTag> Tags { get => tags; set => tags = value; }

    public int CompareTo(ModuleSocket other) => id == other.id ? 0 : id < other.id ? -1 : 1;

    private void Start()
    {
        Destroy(navBlocker);
    }
}
