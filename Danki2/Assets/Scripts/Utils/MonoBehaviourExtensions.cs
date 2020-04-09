using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void WaitAndAct(this MonoBehaviour monoBehaviour, float waitTime, Action action)
    {
        monoBehaviour.StartCoroutine(DelayedAction(waitTime, action));
    }

    public static void ActOnInterval(this MonoBehaviour monoBehaviour, float interval, Action action, float startDelay = 0f)
    {
        monoBehaviour.StartCoroutine(IntervalAction(interval, action, startDelay));
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
}
