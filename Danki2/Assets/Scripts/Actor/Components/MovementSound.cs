using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource movementAudioSource = null;
    [SerializeField]
    private Actor actor = null;

    private const float FadeInTime = 1f;
    private const float FadeOutTime = 0.1f;

    private bool movingPreviousFrame = false;
    private Coroutine fadeInCoroutine = null;
    private Coroutine fadeOutCoroutine = null;
    private float targetVolume;

    private void Start()
    {
        targetVolume = movementAudioSource.volume;
    }

    void Update()
    {
        bool moving = actor.MovementManager.IsMoving;

        if (moving && !movingPreviousFrame)
        {
            if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
            fadeInCoroutine = StartCoroutine(movementAudioSource.FadeInRoutine(FadeInTime, targetVolume));
        }
        
        if (!moving && movingPreviousFrame)
        {
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            fadeOutCoroutine = StartCoroutine(movementAudioSource.FadeOutRoutine(FadeOutTime));
        }

        movingPreviousFrame = moving;
    }
}
