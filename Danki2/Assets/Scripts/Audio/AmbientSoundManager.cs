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
    public List<string> ambientEvents = new List<string>();

    [SerializeField, EventRef]
    public StudioEventEmitter eventEmitter;
    [SerializeField]
    public AnimationCurve dayNightCurve;
    [SerializeField]
    public float minInterval;
    [SerializeField]
    public float maxInterval;
    [SerializeField]
    public float minDistanceFromPlayer;
    [SerializeField]
    public float maxDistanceFromPlayer;

    private void Start()
    {
        float depthProportion = DepthUtils.GetDepthProportion(PersistenceManager.Instance.SaveData.CurrentRoomNode);
        float dayNightCurveValue = dayNightCurve.Evaluate(depthProportion);
        AmbientSoundType ambientSoundType = dayNightCurveValue > 0 ? AmbientSoundType.Day : AmbientSoundType.Night;

        eventEmitter.SetParameter("dayNight", parameterLookup[ambientSoundType]);

        foreach (string ambientEvent in ambientEvents)
        {
            this.ActOnRandomisedInterval(
                minInterval,
                maxInterval,
                _ => PlayAmbientEvent(ambientEvent, ambientSoundType),
                Random.Range(minInterval, maxInterval)
            );
        }
    }

    private void PlayAmbientEvent(string ambientEvent, AmbientSoundType ambientSoundType)
    {
        float distance = Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
        float angle = Random.Range(0f, 2 * Mathf.PI);

        Vector3 position = ActorCache.Instance.Player.transform.position;
        position.x += distance * Mathf.Sin(angle);
        position.z += distance * Mathf.Cos(angle);

        EventInstance eventInstance = FmodUtils.CreatePositionedInstance(ambientEvent, position, new FmodParameter("dayNight", parameterLookup[ambientSoundType]));
        eventInstance.start();
        eventInstance.release();
    }
}
