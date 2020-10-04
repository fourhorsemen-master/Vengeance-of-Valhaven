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

    private void Start()
    {
        RoomManager.Instance.WaveStartSubject.Subscribe(OnWaveStart);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) OnWaveStart(2);
    }

    private void OnWaveStart(int wave)
    {
        if (wave == 1) return;

        StartCoroutine(Flash());
        StartCoroutine(ChangeSize());
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
