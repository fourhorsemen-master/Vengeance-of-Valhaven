using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AmbientSoundManager : Singleton<AmbientSoundManager>
{
    [SerializeField, EventRef]
    public List<string> ambientEvents = new List<string>();

    [SerializeField, EventRef]
    public StudioEventEmitter eventEmitter = null;
    [SerializeField]
    public AnimationCurve dayNightCurve = null;
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
        eventEmitter.SetParameter("dayNight", GetDayNightCurveValue());

        foreach (string ambientEvent in ambientEvents)
        {
            this.ActOnRandomisedInterval(
                minInterval,
                maxInterval,
                _ => PlayAmbientEvent(ambientEvent, GetDayNightCurveValue())
            );
        }
    }

    private void PlayAmbientEvent(string ambientEvent, float dayNightCurveValue)
    {
        float distance = Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
        float angle = Random.Range(0f, 2 * Mathf.PI);

        Vector3 position = ActorCache.Instance.Player.transform.position;
        position.x += distance * Mathf.Sin(angle);
        position.z += distance * Mathf.Cos(angle);

        EventInstance eventInstance = FmodUtils.CreatePositionedInstance(ambientEvent, position, new FmodParameter("dayNight", dayNightCurveValue));
        eventInstance.start();
        eventInstance.release();
    }

    private float GetDayNightCurveValue()
    {
        float depthProportion = DepthUtils.GetDepthProportion(PersistenceManager.Instance.SaveData.CurrentRoomNode);
        return dayNightCurve.Evaluate(depthProportion);
    }
}
