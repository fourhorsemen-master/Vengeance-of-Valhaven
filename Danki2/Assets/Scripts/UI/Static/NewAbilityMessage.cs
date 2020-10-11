using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewAbilityMessage : MonoBehaviour
{
    [SerializeField]
    private Text text = null;

    [SerializeField]
    private Color baseColor = default;
    
    [SerializeField]
    private Color flashColor = default;

    [SerializeField]
    private AnimationCurve flashCurve = null;

    [SerializeField]
    private AnimationCurve sizeCurve = null;

    private Coroutine disappearCoroutine = null;
    private const float disappearTime = 4;

    private void Start()
    {
        text.enabled = false;

        GameStateController.Instance.GameStateTransitionSubject.Subscribe(gameState =>
        {
            if (gameState == GameState.InAbilityTreeEditor)
            {
                text.enabled = false;
            }
        });
        RoomManager.Instance.WaveStartSubject.Subscribe(OnWaveStart);
    }

    private void OnDisable()
    {
        if (disappearCoroutine == null) return;
        StopCoroutine(disappearCoroutine);
        text.enabled = false;
    }

    private void OnWaveStart(int wave)
    {
        if (wave % 2 == 1 || GameStateController.Instance.GameState == GameState.InAbilityTreeEditor) return;

        text.enabled = true;
        StartCoroutine(Flash());
        StartCoroutine(ChangeSize());

        if (disappearCoroutine != null)
        {
            StopCoroutine(disappearCoroutine);
        }

        disappearCoroutine = this.WaitAndAct(disappearTime, () => text.enabled = false);
    }

    private IEnumerator Flash()
    {
        float flashTime = flashCurve.keys[flashCurve.keys.Length - 1].time;
        float timeElapsed = 0;

        while (timeElapsed < flashTime)
        {
            float flashFactor = flashCurve.Evaluate(timeElapsed);
            text.color = baseColor + (flashColor - baseColor) * flashFactor;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        text.color = baseColor;
    }

    private IEnumerator ChangeSize()
    {
        float length = sizeCurve.keys[sizeCurve.keys.Length - 1].time;
        float timeElapsed = 0;

        while (timeElapsed < length)
        {
            float sizeFactor = sizeCurve.Evaluate(timeElapsed);
            transform.localScale = Vector3.one * sizeFactor;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}
