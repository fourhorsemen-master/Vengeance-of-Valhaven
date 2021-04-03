using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public static class FmodUtils
{
    public static EventInstance CreatePositionedInstance(string fmodEvent, Vector3 position)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEvent);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));

        return eventInstance;
    }
}
