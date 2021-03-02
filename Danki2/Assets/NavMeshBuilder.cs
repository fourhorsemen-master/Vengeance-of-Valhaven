using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBuilder : MonoBehaviour
{
    public List<NavMeshSurface> surfaces = null;

    void Start()
    {
        surfaces.ForEach(s => s.BuildNavMesh());
    }
}
