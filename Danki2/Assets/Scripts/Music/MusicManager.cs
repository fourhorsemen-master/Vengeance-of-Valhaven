using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicManager : Singleton<MusicManager>
{
    private const float TargetVolume = 1;

    [SerializeField]
    private float fadeTime = 3f;
    
    [SerializeField]
    private AudioSource[] audioSources = null;

    private Coroutine fadeCoroutine;
    private bool playing = false;

    private void Start()
    {
        RoomManager.Instance.WaveStartSubject.Subscribe(_ => StopCombatMusic());
    }

    public void StartCombatMusic()
    {
        if (playing) return;
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(SelectAudioSource().FadeInRoutine(fadeTime, TargetVolume));
        playing = true;
    }

    private void StopCombatMusic()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(SelectAudioSource().FadeOutRoutine(fadeTime));
        playing = false;
    }

    private AudioSource SelectAudioSource()
    {
        return audioSources[Random.Range(0, audioSources.Length)];
    }
}
