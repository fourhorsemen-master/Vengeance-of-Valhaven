using FMODUnity;
using UnityEngine;

public static class PauseUtils
{
    public static void Pause()
    {
        Time.timeScale = 0;
        RuntimeManager.PauseAllEvents(true);
    }

    public static void Unpause()
    {
        Time.timeScale = 1;
        RuntimeManager.PauseAllEvents(false);
    }
}
