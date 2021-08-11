using FMODUnity;
using UnityEngine;

public class AmbientSoundManager : Singleton<AmbientSoundManager>
{
    [SerializeField] private StudioEventEmitter eventEmitter = null;
    [SerializeField] private AnimationCurve dayNightCurve = null;

    private void Start()
    {
        float depthProportion = DepthUtils.GetDepthProportion(PersistenceManager.Instance.SaveData.CurrentRoomNode);
        float dayNightCurveValue = dayNightCurve.Evaluate(depthProportion);

        eventEmitter.SetParameter("dayNight", dayNightCurveValue);
    }
}
