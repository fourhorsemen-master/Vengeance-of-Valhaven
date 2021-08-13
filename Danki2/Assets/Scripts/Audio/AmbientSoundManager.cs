using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AmbientSoundManager : Singleton<AmbientSoundManager>
{
    private static readonly Dictionary<AmbientSoundType, float> parameterLookup = new Dictionary<AmbientSoundType, float>
    {
        [AmbientSoundType.Day] = 1,
        [AmbientSoundType.Night] = 0,
    };

    [SerializeField, EventRef]
    public List<string> AmbientEvents = new List<string>();

    [SerializeField, EventRef]
    public StudioEventEmitter EventEmitter;
    [SerializeField]
    public AnimationCurve DayNightCurve;
    [SerializeField]
    public float MinInterval;
    [SerializeField]
    public float MaxInterval;
    [SerializeField]
    public float MinDistanceFromPlayer;
    [SerializeField]
    public float MaxDistanceFromPlayer;

    private void Start()
    {
        float depthProportion = DepthUtils.GetDepthProportion(PersistenceManager.Instance.SaveData.CurrentRoomNode);
        float dayNightCurveValue = DayNightCurve.Evaluate(depthProportion);
        AmbientSoundType ambientSoundType = dayNightCurveValue > 0 ? AmbientSoundType.Day : AmbientSoundType.Night;

        EventEmitter.SetParameter("dayNight", parameterLookup[ambientSoundType]);

        foreach (string ambientEvent in AmbientEvents)
        {
            this.ActOnRandomisedInterval(
                MinInterval,
                MaxInterval,
                _ => PlayAmbientEvent(ambientEvent, ambientSoundType),
                Random.Range(MinInterval, MaxInterval)
            );
        }
    }

    private void PlayAmbientEvent(string ambientEvent, AmbientSoundType ambientSoundType)
    {
        float distance = Random.Range(MinDistanceFromPlayer, MaxDistanceFromPlayer);
        float angle = Random.Range(0f, 2 * Mathf.PI);

        Vector3 position = ActorCache.Instance.Player.transform.position;
        position.x += distance * Mathf.Sin(angle);
        position.z += distance * Mathf.Cos(angle);

        EventInstance eventInstance = FmodUtils.CreatePositionedInstance(ambientEvent, position, new FmodParameter("dayNight", parameterLookup[ambientSoundType]));
        eventInstance.start();
        eventInstance.release();
    }
}
