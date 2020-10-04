﻿using System;
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
    private AudioSource selectedAudioSource;
    private bool playing = false;

    private void Start()
    {
        RoomManager.Instance.WaveStartSubject.Subscribe(_ => StopCombatMusic());
    }

    public void StartCombatMusic()
    {
        if (playing) return;
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        selectedAudioSource = audioSources[Random.Range(0, audioSources.Length)];
        fadeCoroutine = StartCoroutine(selectedAudioSource.FadeInRoutine(fadeTime, TargetVolume));
        playing = true;
    }

    private void StopCombatMusic()
    {
        if (!playing) return;
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(selectedAudioSource.FadeOutRoutine(fadeTime));
        playing = false;
    }
}
