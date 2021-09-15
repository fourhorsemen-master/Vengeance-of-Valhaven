using FMODUnity;
using System;
using UnityEngine;

[Serializable]
public class AbilityFmodEvents
{
    [SerializeField, EventRef]
    private string fmodStartEventRef = "";
    [SerializeField, EventRef]
    private string fmodSwingEventRef = "";
    [SerializeField, EventRef]
    private string fmodImpactEventRef = "";

    public string FmodStartEventRef { get => fmodStartEventRef; set => fmodStartEventRef = value; }
    public string FmodSwingEventRef { get => fmodSwingEventRef; set => fmodSwingEventRef = value; }
    public string FmodImpactEventRef { get => fmodImpactEventRef; set => fmodImpactEventRef = value; }
}
