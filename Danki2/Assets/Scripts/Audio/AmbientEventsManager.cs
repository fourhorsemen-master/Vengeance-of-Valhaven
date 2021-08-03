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

    private float dayNightType => AmbientSoundManager.Instance.AmbientSoundTypeLookup;

    private bool audioPlayed = true;

    private void Start()
    {
        this.WaitAndAct(Random.Range(AmbientEventMinFrequency, AmbientEventMaxFrequency), () => audioPlayed = false);
    }

    private void Update()
    {
        if (!audioPlayed)
        {
            PlayRandomAmbientEvent();
            audioPlayed = true;
            this.WaitAndAct(Random.Range(AmbientEventMinFrequency, AmbientEventMaxFrequency), () => audioPlayed = false);
        }        
    }

    private void PlayRandomAmbientEvent()
    {
        string ambientEvent = AmbientEvents[Random.Range(0, AmbientEvents.Count)];

        float distance = Random.Range(MinAmbientEventDistanceFromPlayer, MaxAmbientEventDistanceFromPlayer);
        float angle = Random.Range(0f, 2 * Mathf.PI);

        Vector3 position = ActorCache.Instance.Player.transform.position;
        position.x += distance * Mathf.Sin(angle);
        position.z += distance * Mathf.Cos(angle);

        RuntimeManager.PlayOneShot(ambientEvent, "dayNight", dayNightType, position);
    }
}
