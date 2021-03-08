using System;
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

    public static Coroutine ActOnInterval(this MonoBehaviour monoBehaviour, float interval, Action<int> action, float startDelay = 0f, int? numRepetitions = null)
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

    /// <summary>
    /// Invokes an action on an interval.
    /// </summary>
    /// <param name="interval">Time between each invocation.</param>
    /// <param name="action">The action to perform. Takes it's 0-based index as a parameter.</param>
    /// <param name="startDelay">Delay before first invocation.</param>
    /// <param name="numRepetitions">Optional. Limits the number of repetitions.</param>
    /// <returns></returns>
    private static IEnumerator IntervalAction(float interval, Action<int> action, float startDelay, int? numRepetitions = null)
    {
        yield return new WaitForSeconds(startDelay);

        int counter = 0;

        while(true)
        {
            action(counter);
            yield return new WaitForSeconds(interval);

            counter ++;

            if (numRepetitions.HasValue && counter == numRepetitions) break;
        }
    }
}
