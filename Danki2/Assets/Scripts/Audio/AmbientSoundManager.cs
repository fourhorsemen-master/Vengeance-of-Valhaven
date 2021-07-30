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

    [SerializeField] private StudioEventEmitter eventEmitter = null;
    [SerializeField] private AnimationCurve dayNightCurve = null;

    private AmbientSoundType ambientSoundType;
    public AmbientSoundType AmbientSoundType { get => ambientSoundType; }

    private void Start()
    {
        float depthProportion = DepthUtils.GetDepthProportion(PersistenceManager.Instance.SaveData.CurrentRoomNode);
        float dayNightCurveValue = dayNightCurve.Evaluate(depthProportion);
        ambientSoundType = dayNightCurveValue > 0 ? AmbientSoundType.Day : AmbientSoundType.Night;

        eventEmitter.SetParameter("dayNight", parameterLookup[ambientSoundType]);
    }
}
