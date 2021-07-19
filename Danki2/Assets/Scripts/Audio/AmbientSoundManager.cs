using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AmbientSoundManager : Singleton<AmbientSoundManager>
{
    private static readonly Dictionary<AmbientSoundType, float> parameterLookup = new Dictionary<AmbientSoundType, float>
    {
        [AmbientSoundType.Day] = 1,
        [AmbientSoundType.Night] = 0,
    };

    [SerializeField]
    private StudioEventEmitter eventEmitter = null;

    private void Start()
    {
        eventEmitter.SetParameter("dayNight", parameterLookup[AmbientSoundType.Day]);
    }
}
