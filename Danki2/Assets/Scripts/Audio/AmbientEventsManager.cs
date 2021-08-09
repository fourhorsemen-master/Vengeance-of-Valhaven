using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AmbientEventsManager : Singleton<AmbientEventsManager>
{
    [SerializeField, EventRef]
    public List<string> AmbientEvents = new List<string>();

    [SerializeField]
    public float MinInterval;
    [SerializeField]
    public float MaxInterval;
    [SerializeField]
    public float MinDistanceFromPlayer;
    [SerializeField]
    public float MaxDistanceFromPlayer;

    private float dayNightType => AmbientSoundManager.Instance.AmbientSoundTypeLookup;

    private bool audioPlayed = true;

    private void Start()
    {
        this.WaitAndAct(Random.Range(MinInterval, MaxInterval), () => audioPlayed = false);
    }

    private void Update()
    {
        if (!audioPlayed)
        {
            PlayRandomAmbientEvent();
            audioPlayed = true;
            this.WaitAndAct(Random.Range(MinInterval, MaxInterval), () => audioPlayed = false);
        }        
    }

    private void PlayRandomAmbientEvent()
    {
        string ambientEvent = AmbientEvents[Random.Range(0, AmbientEvents.Count)];

        float distance = Random.Range(MinDistanceFromPlayer, MaxDistanceFromPlayer);
        float angle = Random.Range(0f, 2 * Mathf.PI);

        Vector3 position = ActorCache.Instance.Player.transform.position;
        position.x += distance * Mathf.Sin(angle);
        position.z += distance * Mathf.Cos(angle);

        EventInstance eventInstance = FmodUtils.CreatePositionedInstance(ambientEvent, position, new FmodParameter("dayNight", dayNightType));
        eventInstance.start();
        eventInstance.release();
    }
}
