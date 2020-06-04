using System.Collections;
using UnityEngine;

public static class AudioSourceExtensions
{
    public static IEnumerator FadeInRoutine(this AudioSource audioSource, float fadeTime, float targetVolume)
    {
        audioSource.Play();
        audioSource.volume = 0f;

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public static IEnumerator FadeOutRoutine(this AudioSource audioSource, float fadeTime)
    {        
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
