﻿using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static Coroutine NextFrame(this MonoBehaviour monoBehaviour, Action action)
    {
        return monoBehaviour.StartCoroutine(DelayedAction(0, action));
    }

    public static Coroutine WaitAndAct(this MonoBehaviour monoBehaviour, float waitTime, Action action)
    {
        return monoBehaviour.StartCoroutine(DelayedAction(waitTime, action));
    }

    public static Coroutine ActOnInterval(this MonoBehaviour monoBehaviour, float interval, Action action, float startDelay = 0f, int? numRepetitions = null)
    {
        if (numRepetitions == null)
        {
            return monoBehaviour.StartCoroutine(IntervalAction(interval, action, startDelay));
        }
        else
        {
            return monoBehaviour.StartCoroutine(IntervalAction(interval, action, startDelay, numRepetitions.Value));
        }
    }

    private static IEnumerator DelayedAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action();
    }

    private static IEnumerator IntervalAction(float interval, Action action, float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        while(true)
        {
            action();
            yield return new WaitForSeconds(interval);
        }
    }

    private static IEnumerator IntervalAction(float interval, Action action, float startDelay, int numRepetitions)
    {
        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < numRepetitions; i++)
        {
            action();
            yield return new WaitForSeconds(interval);
        }
    }
}
