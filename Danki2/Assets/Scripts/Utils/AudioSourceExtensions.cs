using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtensions
{
    public static IEnumerator FadeInRoutine(this AudioSource audioSource, float FadeTime, float TargetVolume)
    {
        audioSource.Play();
        audioSource.volume = 0f;

        while (audioSource.volume < TargetVolume)
        {
            audioSource.volume += TargetVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = TargetVolume;
    }

    public static IEnumerator FadeOutRoutine(this AudioSource audioSource, float FadeTime)
    {        
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
