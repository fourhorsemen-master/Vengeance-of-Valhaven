using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    private const float TargetVolume = 1;

    [SerializeField]
    private float fadeTime = 3f;
    
    [SerializeField]
    private AudioSource[] audioSources = null;

    private Coroutine fadeCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) StartCombatMusic();
        if (Input.GetKeyDown(KeyCode.L)) StopCombatMusic();
    }

    public void StartCombatMusic()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(SelectAudioSource().FadeInRoutine(fadeTime, TargetVolume));
    }

    public void StopCombatMusic()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(SelectAudioSource().FadeOutRoutine(fadeTime));
    }

    private AudioSource SelectAudioSource()
    {
        return audioSources[Random.Range(0, audioSources.Length)];
    }
}
