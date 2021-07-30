using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AmbientEventsManager : Singleton<AmbientEventsManager>
{
    [SerializeField, EventRef]
    public List<string> AmbientEvents = new List<string>();

    [SerializeField]
    public float AmbientEventMinFrequency;
    [SerializeField]
    public float AmbientEventMaxFrequency;
    [SerializeField]
    public float MinAmbientEventDistanceFromPlayer;
    [SerializeField]
    public float MaxAmbientEventDistanceFromPlayer;

    private AmbientSoundType dayNightType => AmbientSoundManager.Instance.AmbientSoundType;

    private void Update()
    {
        
    }
}
