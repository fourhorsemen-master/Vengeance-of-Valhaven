﻿using UnityEngine;
using UnityEngine.UI;

public class WarningSign : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemy = null;
    [SerializeField]
    private Text _exclaimationMark = null;

    private Coroutine disableWarningCoroutine = null;

    public void Start()
    {
        if (_exclaimationMark == null)
        {
            Debug.LogError("No image found for exclaimation mark");
            return;
        }

        if (_exclaimationMark.canvas == null)
        {
            return;
        }

        _exclaimationMark.enabled = false;

        _enemy.OnTelegraph.Subscribe(tuple => ShowWarning(tuple.Item1, tuple.Item2));
    }

    private void ShowWarning(float duration, Color colour)
    {
        if (disableWarningCoroutine != null)
        {
            StopCoroutine(disableWarningCoroutine);
        }

        _exclaimationMark.enabled = true;
        _exclaimationMark.color = colour;

        disableWarningCoroutine = this.WaitAndAct(duration, () =>
        {
            _exclaimationMark.enabled = false;
        });
    }
}
