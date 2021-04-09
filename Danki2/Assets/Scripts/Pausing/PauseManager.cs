using FMODUnity;
using UnityEngine;

public static class PauseManager
{
    public static void Pause()
    {
        Time.timeScale = 0;
        RuntimeManager.PauseAllEvents(true);
    }

    public static void UnPause()
    {
        Time.timeScale = 1;
        RuntimeManager.PauseAllEvents(false);
    }
}
