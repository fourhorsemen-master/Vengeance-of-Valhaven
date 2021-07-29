using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AmbientEventsManager : Singleton<AmbientEventsManager>
{
    public List<string> ambientEvents = new List<string>();

    [SerializeField, EventRef]
    public string Event = "";
}
