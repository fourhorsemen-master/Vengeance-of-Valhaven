using System.Collections.Generic;
using UnityEngine;

public class TransitionModuleLookup : Singleton<TransitionModuleLookup>
{
    [SerializeField]
    private List<GameObject> modules = new List<GameObject>();

    public List<GameObject> Modules => modules;
}
