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
        actor.DeathSubject.Subscribe(FadeOut);
    }

    private void Update()
    {
        bool moving = actor.MovementManager.IsMoving;

        if (moving && !movingPreviousFrame) FadeIn();

        if (!moving && movingPreviousFrame) FadeOut();

        movingPreviousFrame = moving;
    }

    private void FadeIn()
    {
        if (fadeOutCoroutine != null) StopCoroutine(fadeOutCoroutine);
        fadeInCoroutine = StartCoroutine(movementAudioSource.FadeInRoutine(FadeInTime, targetVolume));
    }

    private void FadeOut()
    {
        if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
        fadeOutCoroutine = StartCoroutine(movementAudioSource.FadeOutRoutine(FadeOutTime));
    }
}
