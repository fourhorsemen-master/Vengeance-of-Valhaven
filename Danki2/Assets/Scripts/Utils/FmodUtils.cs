using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public static class FmodUtils
{
    public static EventInstance CreatePositionedInstance(string fmodEvent, Vector3 position)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEvent);
        eventInstance.set3DAttributes(position.To3DAttributes());

        return eventInstance;
    }

    public static float GetDuration(string fmodEvent)
    {
        EventDescription description = RuntimeManager.GetEventDescription(fmodEvent);

        description.getLength(out int durationMilliseconds);

        return (float)durationMilliseconds / 1000;
    }
}
