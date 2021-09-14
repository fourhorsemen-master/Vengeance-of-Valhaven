using FMODUnity;
using System;
using UnityEngine;

[Serializable]
public class AbilityFmodEvents
{
    [SerializeField, EventRef]
    private string fmodStartEventRef = "";
    [SerializeField, EventRef]
    private string fmodEndEventRef = "";

    public string FmodStartEventRef { get => fmodStartEventRef; set => fmodStartEventRef = value; }
    public string FmodEndEventRef { get => fmodEndEventRef; set => fmodEndEventRef = value; }
}
